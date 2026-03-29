// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenNonNullAddAll<TKey, TValue>(
      string fieldName
    , IReadOnlyDictionary<TKey, TValue>? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      WhenConditionMetAddAll(value != null, fieldName, value, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue>(
      string fieldName
    , KeyValuePair<TKey, TValue>[]? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      WhenConditionMetAddAll(value != null, fieldName, value, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue>(
      string fieldName
    , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      WhenConditionMetAddAll(value != null, fieldName, value, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TEnumbl>(
      string fieldName
    , TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable =>
      WhenConditionMetAddAllEnumerate(value != null, fieldName, value, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TEnumbl>(
      string fieldName
    , TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable? =>
      WhenConditionMetAddAllEnumerate(value != null, fieldName, value, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TEnumbl, TKey, TValue>(
      string fieldName
    , TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>> =>
      WhenConditionMetAddAllEnumerate<TEnumbl, TKey, TValue>
        (value != null, fieldName, value, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerate<TEnumbl, TKey, TValue>(
      string fieldName
    , TEnumbl? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>? =>
      WhenConditionMetAddAllEnumerate<TEnumbl, TKey, TValue>
        (value != null, fieldName, value, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterate<TEnumtr>(
      string fieldName
    , TEnumtr? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator =>
      WhenConditionMetAddAllIterate(value != null, fieldName, value, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterate<TEnumtr>(
      string fieldName
    , TEnumtr? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator? =>
      WhenConditionMetAddAllIterate(value != null, fieldName, value, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterate<TEnumtr, TKey, TValue>(
      string fieldName
    , TEnumtr? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>> =>
      WhenConditionMetAddAllIterate<TEnumtr, TKey, TValue>
        (value != null, fieldName, value, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterate<TEnumtr, TKey, TValue>(
      string fieldName
    , TEnumtr? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>? =>
      WhenConditionMetAddAllIterate<TEnumtr, TKey, TValue>
        (value != null, fieldName, value, valueFormatString, keyFormatString, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TVRevealBase>(
      string fieldName
    , IReadOnlyDictionary<TKey, TValue>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TValue : TVRevealBase?  
       where TVRevealBase : notnull =>
      WhenConditionMetAddAll(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue>(
      string fieldName
    , IReadOnlyDictionary<TKey, TValue?>? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TValue : struct =>
      WhenConditionMetAddAll(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TVRevealBase>(
      string fieldName
    , KeyValuePair<TKey, TValue>[]? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      WhenConditionMetAddAll(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue>(
      string fieldName
    , KeyValuePair<TKey, TValue?>[]? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TValue : struct  =>
      WhenConditionMetAddAll(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TVRevealBase>(
      string fieldName
    , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      WhenConditionMetAddAll(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue>(
      string fieldName
    , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TValue : struct  =>
      WhenConditionMetAddAll(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateValueRevealer<TEnumbl, TVRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable 
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateValueRevealer(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateValueRevealer<TEnumbl, TVRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable? 
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateValueRevealer(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue>>
       where TValue : TVRevealBase? 
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue>>?
       where TValue : TVRevealBase? 
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateValueRevealer<TEnumbl, TKey, TValue, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateNullValueRevealer<TEnumbl, TValue>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable
       where TValue : struct  =>
      WhenConditionMetAddAllEnumerateNullValueRevealer(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateNullValueRevealer<TEnumbl, TValue>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable?
       where TValue : struct  =>
      WhenConditionMetAddAllEnumerateNullValueRevealer(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue?>>
       where TValue : struct  =>
      WhenConditionMetAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>
        (value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
       where TValue : struct  =>
      WhenConditionMetAddAllEnumerateNullValueRevealer<TEnumbl, TKey, TValue>
        (value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateValueRevealer<TEnumtr, TVRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator 
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateValueRevealer(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateValueRevealer<TEnumtr, TVRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator? 
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateValueRevealer(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue>>
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateValueRevealer<TEnumtr, TKey, TValue, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateNullValueRevealer<TEnumtr, TValue>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator
        where TValue : struct  =>
      WhenConditionMetAddAllIterateNullValueRevealer(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateNullValueRevealer<TEnumtr, TValue>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator?
        where TValue : struct  =>
      WhenConditionMetAddAllIterateNullValueRevealer(value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TValue : struct  =>
      WhenConditionMetAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>
        (value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct  =>
      WhenConditionMetAddAllIterateNullValueRevealer<TEnumtr, TKey, TValue>
        (value != null, fieldName, value, valueRevealer, keyFormatString, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
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
      WhenConditionMetAddAll(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TKRevealBase>(
      string fieldName
    , IReadOnlyDictionary<TKey, TValue?>? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TKey : TKRevealBase 
       where TValue : struct 
       where TKRevealBase : notnull =>
      WhenConditionMetAddAll(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
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
      WhenConditionMetAddAll(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TVRevealBase>(
      string fieldName
    , KeyValuePair<TKey?, TValue>[]? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      WhenConditionMetAddAll(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TKRevealBase>(
      string fieldName
    , KeyValuePair<TKey, TValue?>[]? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : TKRevealBase? 
        where TValue : struct 
        where TKRevealBase : notnull =>
      WhenConditionMetAddAll(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue>(
      string fieldName
    , KeyValuePair<TKey?, TValue?>[]? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : struct 
        where TValue : struct  =>
      WhenConditionMetAddAll(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
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
      WhenConditionMetAddAll(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TVRevealBase>(
      string fieldName
    , IReadOnlyList<KeyValuePair<TKey?, TValue>>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      WhenConditionMetAddAll(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue, TKRevealBase>(
      string fieldName
    , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TKey : TKRevealBase? 
      where TValue : struct 
      where TKRevealBase : notnull =>
      WhenConditionMetAddAll(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAll<TKey, TValue>(
      string fieldName
    , IReadOnlyList<KeyValuePair<TKey?, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TKey : struct 
      where TValue : struct  =>
      WhenConditionMetAddAll(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateBothRevealers<TEnumbl, TKRevealBase, TVRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable 
       where TKRevealBase : notnull
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateBothRevealers<TEnumbl, TKRevealBase, TVRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable? 
       where TKRevealBase : notnull
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(
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
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>(
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
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothRevealers<TEnumbl, TKey, TValue, TKRevealBase, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable
       where TKey : struct  
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothWithNullKeyRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TVRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable?
       where TKey : struct  
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothWithNullKeyRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable<KeyValuePair<TKey?, TValue>>
       where TKey : struct 
       where TValue : TVRevealBase? 
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue>>?
       where TKey : struct 
       where TValue : TVRevealBase? 
       where TVRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothWithNullKeyRevealers<TEnumbl, TKey, TValue, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable 
       where TValue : struct 
       where TKRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothWithNullValueRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TValue, TKRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable? 
       where TValue : struct 
       where TKRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothWithNullValueRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable<KeyValuePair<TKey, TValue?>>
       where TKey : TKRevealBase? 
       where TValue : struct 
       where TKRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable<KeyValuePair<TKey, TValue?>>?
       where TKey : TKRevealBase? 
       where TValue : struct 
       where TKRevealBase : notnull =>
      WhenConditionMetAddAllEnumerateBothWithNullValueRevealers<TEnumbl, TKey, TValue, TKRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : struct, IEnumerable<KeyValuePair<TKey?, TValue?>>
       where TKey : struct 
       where TValue : struct  =>
      WhenConditionMetAddAllEnumerateBothNullRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllEnumerateBothNullRevealers<TEnumbl, TKey, TValue>(
      string fieldName
    , TEnumbl? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumbl : IEnumerable<KeyValuePair<TKey?, TValue?>>?
       where TKey : struct 
       where TValue : struct  =>
      WhenConditionMetAddAllEnumerateBothNullRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateBothRevealers<TEnumtr, TKRevealBase, TVRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateBothRevealers<TEnumtr, TKRevealBase, TVRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(
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
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(
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
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateBothNullKeyRevealers<TEnumtr, TKey, TVRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator
        where TKey : struct  
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothWithNullKeyRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateBothNullKeyRevealers<TEnumtr, TKey, TVRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator?
        where TKey : struct  
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothWithNullKeyRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateBothNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue>>
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateBothNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue>>?
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateBothWithNullValueRevealers<TEnumtr, TValue, TKRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator 
        where TValue : struct 
        where TKRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothWithNullValueRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateBothWithNullValueRevealers<TEnumtr, TValue, TKRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator? 
        where TValue : struct 
        where TKRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothWithNullValueRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator<KeyValuePair<TKey, TValue?>>
        where TKey : TKRevealBase? 
        where TValue : struct 
        where TKRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase? 
        where TValue : struct 
        where TKRevealBase : notnull =>
      WhenConditionMetAddAllIterateBothWithNullValueRevealers<TEnumtr, TKey, TValue, TKRevealBase>
        (value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateBothNullRevealers<TEnumtr, TKey, TValue>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : struct, IEnumerator<KeyValuePair<TKey?, TValue?>>
        where TKey : struct 
        where TValue : struct  =>
      WhenConditionMetAddAllIterateBothNullRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);

    public TExt WhenNonNullAddAllIterateBothNullRevealers<TEnumtr, TKey, TValue>(
      string fieldName
    , TEnumtr? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct 
        where TValue : struct  =>
      WhenConditionMetAddAllIterateBothNullRevealers(value != null, fieldName, value, valueRevealer, keyRevealer, valueFormatString, formatFlags);
}
