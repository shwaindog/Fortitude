// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TExt> where TExt : TypeMolder
{
    public TExt AlwaysAddAll<TKey, TValue>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags);
            ekcm.AddAll(value, valueFormatString, keyFormatString, formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue>(
        string fieldName
      , KeyValuePair<TKey, TValue>[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<KeyValuePair<TKey, TValue>[], TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString  ?? "", keyFormatString);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyList<KeyValuePair<TKey, TValue>>, TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString  ?? "", keyFormatString);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TEnumbl>(
        string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        if (value != null)
        {
            return AlwaysAddAllEnumerate(fieldName, value.Value, valueFormatString, keyFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TEnumbl>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddAllEnumerate(value, valueFormatString, keyFormatString, formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TEnumbl, TKey, TValue>(
        string fieldName
      , TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        if (value != null)
        {
            return AlwaysAddAllEnumerate(fieldName, value.Value, valueFormatString, keyFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TEnumbl, TKey, TValue>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags);
            ekcm.AddAllEnumerate<TEnumbl, TKey, TValue>(value, valueFormatString, keyFormatString, formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterate<TEnumtr>(
        string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
    {
        if (value != null)
        {
            return AlwaysAddAllIterate(fieldName, value.Value, valueFormatString, keyFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterate<TEnumtr>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm     = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddAllIterate(value, valueFormatString, keyFormatString, formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterate<TEnumtr, TKey, TValue>(
        string fieldName
      , TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
    {
        if (value != null)
        {
            return AlwaysAddAllIterate<TEnumtr, TKey, TValue>(fieldName, value.Value, valueFormatString, keyFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterate<TEnumtr, TKey, TValue>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm     = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags);
            ekcm.AddAllIterate<TEnumtr, TKey, TValue>(value, valueFormatString, keyFormatString, formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull 
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags);
            ekcm.AddAll(value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct 
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue>(value, createFormatFlags);
            ekcm.AddAll(value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TVRevealBase>(
        string fieldName
      , KeyValuePair<TKey, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<KeyValuePair<TKey, TValue>[], TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue>(
        string fieldName
      , KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<KeyValuePair<TKey, TValue?>[], TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TVRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyList<KeyValuePair<TKey, TValue>>, TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyList<KeyValuePair<TKey, TValue?>>, TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerateValueRevealer<TEnumbl, TVRevealBase>(
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
            return AlwaysAddAllEnumerateValueRevealer(fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerateValueRevealer<TEnumbl, TVRevealBase>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddAllEnumerateValueRevealer(value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>(
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
            return AlwaysAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>
                (fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags);
            ekcm.AddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>
                (value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }
    
    public TExt AlwaysAddAllEnumerateNullValueRevealer<TEnumbl, TValue>(
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
            return AlwaysAddAllEnumerateNullValueRevealer(fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }
    
    public TExt AlwaysAddAllEnumerateNullValueRevealer<TEnumbl, TValue>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddAllEnumerateNullValueRevealer(value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>(
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
            return AlwaysAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>
                (fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags);
            ekcm.AddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>
                (value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateValueRevealer<TEnumtr, TVRevealBase>(
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
            return AlwaysAddAllIterateValueRevealer(fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateValueRevealer<TEnumtr, TVRevealBase>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm     = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddAllIterateValueRevealer(value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>(
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
            return AlwaysAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>
                (fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm     = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags);
            ekcm.AddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>
                (value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateNullValueRevealer<TEnumtr, TValue>(
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
            return AlwaysAddAllIterateNullValueRevealer(fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateNullValueRevealer<TEnumtr, TValue>(
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
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddAllIterateNullValueRevealer(value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>(
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
            return AlwaysAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>
                (fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm     = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags);
            ekcm.AddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>
                (value, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull 
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>(value, createFormatFlags);
            ekcm.AddAll(value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TKRevealBase>(
        string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase
        where TValue : struct
        where TKRevealBase : notnull 
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyDictionary<TKey, TValue?>, TKey, TValue>(value, createFormatFlags);
            ekcm.AddAll(value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
        string fieldName
      , KeyValuePair<TKey, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<KeyValuePair<TKey, TValue>[], TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TVRevealBase>(
        string fieldName
      , KeyValuePair<TKey?, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<KeyValuePair<TKey?, TValue>[], TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TKRevealBase>(
        string fieldName
      , KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<KeyValuePair<TKey, TValue?>[], TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue>(
        string fieldName
      , KeyValuePair<TKey?, TValue?>[]? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<KeyValuePair<TKey?, TValue?>[], TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyList<KeyValuePair<TKey, TValue>>, TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TVRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey?, TValue>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyList<KeyValuePair<TKey?, TValue>>, TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TKRevealBase>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyList<KeyValuePair<TKey, TValue?>>, TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue>(
        string fieldName
      , IReadOnlyList<KeyValuePair<TKey?, TValue?>>? value
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKey : struct
        where TValue : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<IReadOnlyList<KeyValuePair<TKey?, TValue?>>, TKey, TValue>(value, createFormatFlags);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];

                ekcm.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            }
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerateBothRevealers<TEnumbl, TKRevealBase, TVRevealBase>(
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
            return AlwaysAddAllEnumerateBothRevealers(fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerateBothRevealers<TEnumbl, TKRevealBase, TVRevealBase>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddAllEnumerateBothRevealers(value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(
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
            return AlwaysAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>
                (fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags);
            ekcm.AddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>
                (value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVRevealBase>(
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
            return AlwaysAddAllEnumerateBothWithNullKeyRevealers(fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVRevealBase>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddAllEnumerateBothWithNullKeyRevealers(value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>(
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
            return AlwaysAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>
                (fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags);
            ekcm.AddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>
                (value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKRevealBase>(
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
            return AlwaysAddAllEnumerateBothWithNullValueRevealers(fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKRevealBase>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddAllEnumerateBothWithNullValueRevealers(value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>(
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
            return AlwaysAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>
                (fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags);
            ekcm.AddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>
                (value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
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
            return AlwaysAddAllEnumerateBothNullRevealers(fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
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
        var actualType = value?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);
        if (Mws.HasSkipField(actualType, fieldName, formatFlags))
            return Mws.WasSkipped(actualType, fieldName, formatFlags);
        var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumbl, TKey, TValue>(value, createFormatFlags);
            ekcm.AddAllEnumerateBothNullRevealers(value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateBothRevealers<TEnumtr, TKRevealBase, TVRevealBase>(
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
            return AlwaysAddAllIterateBothRevealers(fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateBothRevealers<TEnumtr, TKRevealBase, TVRevealBase>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddAllIterateBothRevealers(value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(
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
            return AlwaysAddAllIterateBothRevealers(fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags);
            ekcm.AddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>
                (value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVRevealBase>(
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
            return AlwaysAddAllIterateBothWithNullKeyRevealers(fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVRevealBase>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddAllIterateBothWithNullKeyRevealers(value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>(
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
            return AlwaysAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>
                (fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags);
            ekcm.AddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>
                (value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateBothWithNullValueRevealers<TEnumtr, TValue, TKRevealBase>(
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
            return AlwaysAddAllIterateBothWithNullValueRevealers(fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateBothWithNullValueRevealers<TEnumtr, TValue, TKRevealBase>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
            ekcm.AddAllIterateBothWithNullValueRevealers(value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.Complete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>(
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
            return AlwaysAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>
                (fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags);
            ekcm.AddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>
                (value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateBothNullRevealers<TEnumtr, TKey, TValue>(
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
            return AlwaysAddAllIterateBothNullRevealers(fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
        }
        Mws.FieldNameJoin(fieldName);
        Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }

    public TExt AlwaysAddAllIterateBothNullRevealers<TEnumtr, TKey, TValue>(
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
        Mws.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcm = Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags);
            ekcm.AddAllIterateBothNullRevealers(value, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags);
            ekcm.AppendCollectionComplete();
        }
        else
            Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
        return Mws.AddGoToNext();
    }
}
