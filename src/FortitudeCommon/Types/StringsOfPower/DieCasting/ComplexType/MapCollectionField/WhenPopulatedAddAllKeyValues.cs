// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
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
    //
    // public TExt WhenPopulatedAddAllIterate<TEnumtr>(
    //     string fieldName
    //   , TEnumtr? value
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : IEnumerator?
    // {
    //     var actualType = value?.GetType() ?? typeof(TEnumtr);
    //     if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
    //         return Mws.WasSkipped(actualType, fieldName, formatFlags);
    //     var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags, keyFormatString ?? "");
    //     if (value != null)
    //     {
    //         var hasValue  = value.MoveNext();
    //         if (hasValue)
    //         {
    //             Mws.FieldNameJoin(fieldName);
    //             var ekct = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
    //             ekct.AddAllIterate(value, valueFormatString, keyFormatString, formatFlags);
    //             ekct.Complete();
    //         }
    //     }
    //     return Mws.Mold;
    // }
    //
    // public TExt WhenPopulatedAddAllIterate<TEnumtr, TKey, TValue>(
    //     string fieldName
    //   , TEnumtr? value
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
    // {
    //     var actualType = value?.GetType() ?? typeof(TEnumtr);
    //     if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
    //         return Mws.WasSkipped(actualType, fieldName, formatFlags);
    //     var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags, keyFormatString ?? "");
    //     if (value != null)
    //     {
    //         var hasValue  = value.MoveNext();
    //         if (hasValue)
    //         {
    //             Mws.FieldNameJoin(fieldName);
    //             var ekct = 
    //                 Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags);
    //             ekct.AddAllIterate<TEnumtr, TKey, TValue>(value, valueFormatString, keyFormatString, formatFlags);
    //             ekct.AppendCollectionComplete();
    //             return Mws.AddGoToNext();
    //         }
    //     }
    //     return Mws.Mold;
    // }

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

    // public TExt WhenPopulatedAddAllIterateValueRevealer<TEnumtr, TVRevealBase>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TVRevealBase> valueRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : struct, IEnumerator
    //     where TVRevealBase : notnull 
    // {
    //     if (value != null)
    //     {
    //         return WhenPopulatedAddAllIterateValueRevealer(fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    //     }
    //     Mws.FieldNameJoin(fieldName);
    //     Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
    //     return Mws.AddGoToNext();
    // }
    //
    // public TExt WhenPopulatedAddAllIterateValueRevealer<TEnumtr, TVRevealBase>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TVRevealBase> valueRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : IEnumerator?
    //     where TVRevealBase : notnull 
    // {
    //     var actualType = value?.GetType() ?? typeof(TEnumtr);
    //     if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
    //         return Mws.WasSkipped(actualType, fieldName, formatFlags);
    //     var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags, keyFormatString ?? "");
    //     if (value != null)
    //     {
    //         var hasValue = value.MoveNext();
    //         if (hasValue)
    //         {
    //             Mws.FieldNameJoin(fieldName);
    //             var ekct = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
    //             ekct.AddAllIterate(value, valueFormatString, keyFormatString, formatFlags);
    //             ekct.Complete();
    //             return Mws.AddGoToNext();
    //         }
    //     }
    //     return Mws.Mold;
    // }
    //
    // public TExt WhenPopulatedAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TVRevealBase> valueRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
    //     where TValue : TVRevealBase?
    //     where TVRevealBase : notnull 
    // {
    //     if (value != null)
    //     {
    //         return WhenPopulatedAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>
    //             (fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    //     }
    //     Mws.FieldNameJoin(fieldName);
    //     Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
    //     return Mws.AddGoToNext();
    // }
    //
    // public TExt WhenPopulatedAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TVRevealBase> valueRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
    //     where TValue : TVRevealBase?
    //     where TVRevealBase : notnull 
    // {
    //     var actualType = value?.GetType() ?? typeof(TEnumtr);
    //     if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
    //         return Mws.WasSkipped(actualType, fieldName, formatFlags);
    //     var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags, keyFormatString ?? "");
    //     if (value != null)
    //     {
    //         var hasValue = value.MoveNext();
    //         if (hasValue)
    //         {
    //             Mws.FieldNameJoin(fieldName);
    //             var ekct = 
    //                 Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags);
    //             ekct.AddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>(value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    //             ekct.AppendCollectionComplete();
    //             return Mws.AddGoToNext();
    //         }
    //     }
    //     return Mws.Mold;
    // }
    //
    // public TExt WhenPopulatedAddAllIterateNullValueRevealer<TEnumtr, TValue>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TValue> valueRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : struct, IEnumerator
    //     where TValue : struct 
    // {
    //     if (value != null)
    //     {
    //         return WhenPopulatedAddAllIterateNullValueRevealer(fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    //     }
    //     Mws.FieldNameJoin(fieldName);
    //     Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
    //     return Mws.AddGoToNext();
    // }
    //
    // public TExt WhenPopulatedAddAllIterateNullValueRevealer<TEnumtr, TValue>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TValue> valueRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : IEnumerator?
    //     where TValue : struct 
    // {
    //     var actualType = value?.GetType() ?? typeof(TEnumtr);
    //     if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
    //         return Mws.WasSkipped(actualType, fieldName, formatFlags);
    //     var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags, keyFormatString ?? "");
    //     if (value != null)
    //     {
    //         var hasValue = value.MoveNext();
    //         if (hasValue)
    //         {
    //             Mws.FieldNameJoin(fieldName);
    //             var ekct = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
    //             ekct.AddAllIterateNullValueRevealer(value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    //             ekct.Complete();
    //             return Mws.AddGoToNext();
    //         }
    //     }
    //     return Mws.Mold;
    // }
    //
    // public TExt WhenPopulatedAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TValue> valueRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
    //     where TValue : struct 
    // {
    //     if (value != null)
    //     {
    //         return WhenPopulatedAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>
    //             (fieldName, value.Value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    //     }
    //     Mws.FieldNameJoin(fieldName);
    //     Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
    //     return Mws.AddGoToNext();
    // }
    //
    // public TExt WhenPopulatedAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TValue> valueRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
    //     where TValue : struct 
    // {
    //     var actualType = value?.GetType() ?? typeof(TEnumtr);
    //     if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
    //         return Mws.WasSkipped(actualType, fieldName, formatFlags);
    //     var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags, keyFormatString ?? "");
    //     if (value != null)
    //     {
    //         var hasValue = value.MoveNext();
    //         if (hasValue)
    //         {
    //             Mws.FieldNameJoin(fieldName);
    //             var ekct = 
    //                 Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags);
    //             ekct.AddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>(value, valueRevealer, keyFormatString, valueFormatString, formatFlags);
    //             ekct.AppendCollectionComplete();
    //             return Mws.AddGoToNext();
    //         }
    //     }
    //     return Mws.Mold;
    // }

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

    // public TExt WhenPopulatedAddAllIterateBothRevealers<TEnumtr, TKRevealBase, TVRevealBase>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TVRevealBase> valueRevealer
    //   , PalantírReveal<TKRevealBase> keyRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : struct, IEnumerator
    //     where TKRevealBase : notnull 
    //     where TVRevealBase : notnull 
    // {
    //     if (value != null)
    //     {
    //         return WhenPopulatedAddAllIterateBothRevealers(fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
    //     }
    //     Mws.FieldNameJoin(fieldName);
    //     Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
    //     return Mws.AddGoToNext();
    // }
    //
    // public TExt WhenPopulatedAddAllIterateBothRevealers<TEnumtr, TKRevealBase, TVRevealBase>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TVRevealBase> valueRevealer
    //   , PalantírReveal<TKRevealBase> keyRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : IEnumerator?
    //     where TKRevealBase : notnull 
    //     where TVRevealBase : notnull 
    // {
    //     var actualType = value?.GetType() ?? typeof(TEnumtr);
    //     if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
    //         return Mws.WasSkipped(actualType, fieldName, formatFlags);
    //     var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
    //     if (value != null)
    //     {
    //         var hasValue = value.MoveNext();
    //         if (hasValue)
    //         {
    //             Mws.FieldNameJoin(fieldName);
    //             var ekct = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
    //             ekct.AddAllIterateBothRevealers(value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
    //             ekct.Complete();
    //             return Mws.AddGoToNext();
    //         }
    //     }
    //     return Mws.Mold;
    // }
    //
    // public TExt WhenPopulatedAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TVRevealBase> valueRevealer
    //   , PalantírReveal<TKRevealBase> keyRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
    //     where TKey : TKRevealBase? 
    //     where TValue : TVRevealBase?
    //     where TKRevealBase : notnull 
    //     where TVRevealBase : notnull 
    // {
    //     if (value != null)
    //     {
    //         return WhenPopulatedAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>
    //             (fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
    //     }
    //     Mws.FieldNameJoin(fieldName);
    //     Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
    //     return Mws.AddGoToNext();
    // }
    //
    // public TExt WhenPopulatedAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TVRevealBase> valueRevealer
    //   , PalantírReveal<TKRevealBase> keyRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
    //     where TKey : TKRevealBase? 
    //     where TValue : TVRevealBase?
    //     where TKRevealBase : notnull 
    //     where TVRevealBase : notnull 
    // {
    //     var actualType = value?.GetType() ?? typeof(TEnumtr);
    //     if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
    //         return Mws.WasSkipped(actualType, fieldName, formatFlags);
    //     var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
    //     if (value != null)
    //     {
    //         var hasValue = value.MoveNext();
    //         if (hasValue)
    //         {
    //             Mws.FieldNameJoin(fieldName);
    //             var ekct = 
    //                 Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags);
    //             ekct.AddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
    //             ekct.AppendCollectionComplete();
    //             return Mws.AddGoToNext();
    //         }
    //     }
    //     return Mws.Mold;
    // }
    //
    // public TExt WhenPopulatedAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVRevealBase>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TVRevealBase> valueRevealer
    //   , PalantírReveal<TKey> keyRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : struct, IEnumerator
    //     where TKey : struct 
    //     where TVRevealBase : notnull 
    // {
    //     if (value != null)
    //     {
    //         return WhenPopulatedAddAllIterateBothWithNullKeyRevealers(fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
    //     }
    //     Mws.FieldNameJoin(fieldName);
    //     Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
    //     return Mws.AddGoToNext();
    // }
    //
    // public TExt WhenPopulatedAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TVRevealBase>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TVRevealBase> valueRevealer
    //   , PalantírReveal<TKey> keyRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : IEnumerator?
    //     where TKey : struct 
    //     where TVRevealBase : notnull 
    // {
    //     var actualType = value?.GetType() ?? typeof(TEnumtr);
    //     if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
    //         return Mws.WasSkipped(actualType, fieldName, formatFlags);
    //     var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
    //     if (value != null)
    //     {
    //         var hasValue = value.MoveNext();
    //         if (hasValue)
    //         {
    //             Mws.FieldNameJoin(fieldName);
    //             var ekct      = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
    //             ekct.AddAllIterateBothWithNullKeyRevealers(value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
    //             ekct.Complete();
    //             return Mws.AddGoToNext();
    //         }
    //     }
    //     return Mws.Mold;
    // }
    //
    // public TExt WhenPopulatedAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TVRevealBase> valueRevealer
    //   , PalantírReveal<TKey> keyRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue>>
    //     where TKey : struct 
    //     where TValue : TVRevealBase?
    //     where TVRevealBase : notnull 
    // {
    //     if (value != null)
    //     {
    //         return WhenPopulatedAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>
    //             (fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
    //     }
    //     Mws.FieldNameJoin(fieldName);
    //     Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
    //     return Mws.AddGoToNext();
    // }
    //
    // public TExt WhenPopulatedAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TVRevealBase> valueRevealer
    //   , PalantírReveal<TKey> keyRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue>>?
    //     where TKey : struct 
    //     where TValue : TVRevealBase?
    //     where TVRevealBase : notnull 
    // {
    //     var actualType = value?.GetType() ?? typeof(TEnumtr);
    //     if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
    //         return Mws.WasSkipped(actualType, fieldName, formatFlags);
    //     var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
    //     if (value != null)
    //     {
    //         var hasValue = value.MoveNext();
    //         if (hasValue)
    //         {
    //             Mws.FieldNameJoin(fieldName);
    //             var ekct      = 
    //                 Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags);
    //             ekct.AddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>
    //                 (value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
    //             ekct.AppendCollectionComplete();
    //             return Mws.AddGoToNext();
    //         }
    //     }
    //     return Mws.Mold;
    // }
    //
    // public TExt WhenPopulatedAddAllIterateBothWithNullValueRevealers<TEnumtr, TValue, TKRevealBase>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TValue> valueRevealer
    //   , PalantírReveal<TKRevealBase> keyRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : struct, IEnumerator 
    //     where TValue : struct
    //     where TKRevealBase : notnull 
    // {
    //     if (value != null)
    //     {
    //         return WhenPopulatedAddAllIterateBothWithNullValueRevealers(fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
    //     }
    //     Mws.FieldNameJoin(fieldName);
    //     Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
    //     return Mws.AddGoToNext();
    // }
    //
    // public TExt WhenPopulatedAddAllIterateBothWithNullValueRevealers<TEnumtr, TValue, TKRevealBase>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TValue> valueRevealer
    //   , PalantírReveal<TKRevealBase> keyRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : IEnumerator? 
    //     where TValue : struct
    //     where TKRevealBase : notnull 
    // {
    //     var actualType = value?.GetType() ?? typeof(TEnumtr);
    //     if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
    //         return Mws.WasSkipped(actualType, fieldName, formatFlags);
    //     var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
    //     if (value != null)
    //     {
    //         var hasValue = value.MoveNext();
    //         if (hasValue)
    //         {
    //             Mws.FieldNameJoin(fieldName);
    //             var ekct      = Mws.Master.StartKeyedCollectionType(value, createFormatFlags);
    //             ekct.AddAllIterateBothWithNullValueRevealers(value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
    //             ekct.Complete();
    //             return Mws.AddGoToNext();
    //         }
    //     }
    //     return Mws.Mold;
    // }
    //
    // public TExt WhenPopulatedAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TValue> valueRevealer
    //   , PalantírReveal<TKRevealBase> keyRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
    //     where TKey : TKRevealBase? 
    //     where TValue : struct
    //     where TKRevealBase : notnull 
    // {
    //     if (value != null)
    //     {
    //         return WhenPopulatedAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>
    //             (fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
    //     }
    //     Mws.FieldNameJoin(fieldName);
    //     Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
    //     return Mws.AddGoToNext();
    // }
    //
    // public TExt WhenPopulatedAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TValue> valueRevealer
    //   , PalantírReveal<TKRevealBase> keyRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
    //     where TKey : TKRevealBase? 
    //     where TValue : struct
    //     where TKRevealBase : notnull 
    // {
    //     var actualType = value?.GetType() ?? typeof(TEnumtr);
    //     if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
    //         return Mws.WasSkipped(actualType, fieldName, formatFlags);
    //     var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
    //     if (value != null)
    //     {
    //         var hasValue = value.MoveNext();
    //         if (hasValue)
    //         {
    //             Mws.FieldNameJoin(fieldName);
    //             var ekct      = 
    //                 Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags);
    //             ekct.AddAllIterateBothWithNullValueRevealers(value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
    //             ekct.AppendCollectionComplete();
    //             return Mws.AddGoToNext();
    //         }
    //     }
    //     return Mws.Mold;
    // }
    //
    // public TExt WhenPopulatedAddAllIterateBothNullRevealers<TEnumtr, TKey, TValue>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TValue> valueRevealer
    //   , PalantírReveal<TKey> keyRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue?>>
    //     where TKey : struct 
    //     where TValue : struct 
    // {
    //     if (value != null)
    //     {
    //         return WhenPopulatedAddAllIterateBothNullRevealers(fieldName, value.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
    //     }
    //     Mws.FieldNameJoin(fieldName);
    //     Mws.StyleFormatter.AppendFormattedNull(Mws.Sb, valueFormatString  ?? "", formatFlags);
    //     return Mws.AddGoToNext();
    // }
    //
    // public TExt WhenPopulatedAddAllIterateBothNullRevealers<TEnumtr, TKey, TValue>(
    //     string fieldName
    //   , TEnumtr? value
    //   , PalantírReveal<TValue> valueRevealer
    //   , PalantírReveal<TKey> keyRevealer
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    //   , FormatFlags formatFlags = DefaultCallerTypeFlags)
    //     where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue?>>?
    //     where TKey : struct 
    //     where TValue : struct 
    // {
    //     var actualType = value?.GetType() ?? typeof(TEnumtr);
    //     if (Mws.HasSkipField(actualType, fieldName, formatFlags)) 
    //         return Mws.WasSkipped(actualType, fieldName, formatFlags);
    //     var createFormatFlags = Mws.StyleFormatter.ResolveContentFormatFlags(Mws.Sb, value, formatFlags);
    //     if (value != null)
    //     {
    //         var hasValue = value.MoveNext();
    //         if (hasValue)
    //         {
    //             Mws.FieldNameJoin(fieldName);
    //             var ekct      = 
    //                 Mws.Master.StartExplicitKeyedCollectionType<TEnumtr, TKey, TValue>(value, createFormatFlags);
    //             while (hasValue)
    //             {
    //                 var kvp = value.Current;
    //                 ekct.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
    //                 hasValue = value.MoveNext();
    //             }
    //             ekct.AppendCollectionComplete();
    //             return Mws.AddGoToNext();
    //         }
    //     }
    //     return Mws.Mold;
    // }
}
