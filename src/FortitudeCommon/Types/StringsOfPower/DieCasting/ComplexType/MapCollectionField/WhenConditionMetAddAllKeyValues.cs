// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TMold> where TMold : TypeMolder
{
    public TMold WhenConditionMetAddAll<TKey, TValue>(
      bool condition
    , string fieldName
    , IReadOnlyDictionary<TKey, TValue>? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      condition 
          ? AlwaysAddAll(fieldName, value, valueFormatString ?? "", keyFormatString ?? "", formatFlags) 
          : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TKey, TValue>(
      bool condition
    , string fieldName
    , KeyValuePair<TKey, TValue>[]? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueFormatString ?? "", keyFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TKey, TValue>(
      bool condition
    , string fieldName
    , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueFormatString ?? "", keyFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);
    
    public TMold WhenConditionMetAddAll<TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , IReadOnlyDictionary<TKey, TValue>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TValue : TVRevealBase?  
       where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TKey, TValue>(
      bool condition
    , string fieldName
    , IReadOnlyDictionary<TKey, TValue?>? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TValue : struct   =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , KeyValuePair<TKey, TValue>[]? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TKey, TValue>(
      bool condition
    , string fieldName
    , KeyValuePair<TKey, TValue?>[]? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TValue : struct  =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TKey, TValue>(
      bool condition
    , string fieldName
    , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TValue : struct  =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString ?? "", valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
      bool condition
    , string fieldName
    , IReadOnlyDictionary<TKey, TValue>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TKey : TKRevealBase 
       where TValue : TVRevealBase? 
       where TKRevealBase : notnull
       where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TKey, TValue, TKRevealBase>(
      bool condition
    , string fieldName
    , IReadOnlyDictionary<TKey, TValue?>? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
       where TKey : TKRevealBase 
       where TValue : struct 
       where TKRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
      bool condition
    , string fieldName
    , KeyValuePair<TKey, TValue>[]? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , KeyValuePair<TKey?, TValue>[]? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TKey, TValue, TKRevealBase>(
      bool condition
    , string fieldName
    , KeyValuePair<TKey, TValue?>[]? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : TKRevealBase? 
        where TValue : struct 
        where TKRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TKey, TValue>(
      bool condition
    , string fieldName
    , KeyValuePair<TKey?, TValue?>[]? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : struct 
        where TValue : struct  =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(
      bool condition
    , string fieldName
    , IReadOnlyList<KeyValuePair<TKey, TValue>>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TKey, TValue, TVRevealBase>(
      bool condition
    , string fieldName
    , IReadOnlyList<KeyValuePair<TKey?, TValue>>? value
    , PalantírReveal<TVRevealBase> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TKey : struct 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TKey, TValue, TKRevealBase>(
      bool condition
    , string fieldName
    , IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKRevealBase> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TKey : TKRevealBase? 
      where TValue : struct 
      where TKRevealBase : notnull =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);

    public TMold WhenConditionMetAddAll<TKey, TValue>(
      bool condition
    , string fieldName
    , IReadOnlyList<KeyValuePair<TKey?, TValue?>>? value
    , PalantírReveal<TValue> valueRevealer
    , PalantírReveal<TKey> keyRevealer
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags) 
      where TKey : struct 
      where TValue : struct  =>
      condition 
        ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer, valueFormatString ?? "", formatFlags) 
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>), fieldName, formatFlags);
}
