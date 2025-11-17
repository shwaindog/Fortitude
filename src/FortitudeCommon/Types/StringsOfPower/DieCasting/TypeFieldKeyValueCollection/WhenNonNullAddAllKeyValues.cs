// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenNonNullAddAll<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
      !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
      !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
      !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
      !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, valueFormatString, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
       where TValue : TVRevealBase  
       where TVRevealBase : notnull =>
      !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue, TVRevealBase>
    (string fieldName, KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : TVRevealBase 
        where TVRevealBase : notnull =>
      !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue, TVRevealBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : TVRevealBase 
        where TVRevealBase : notnull =>
      !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, valueRevealer, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue, TVRevealBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
       where TValue : TVRevealBase 
       where TVRevealBase : notnull =>
      !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue, TVRevealBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : TVRevealBase 
        where TVRevealBase : notnull =>
      !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyFormatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue?>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer) 
       where TKey : TKRevealBase 
       where TValue : TVRevealBase 
       where TKRevealBase : notnull
       where TVRevealBase : notnull =>
      !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue, TKRevealBase, TVRevealBase>
    (string fieldName, KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
      !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAll<TKey, TValue, TKRevealBase, TVRevealBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
      !stb.SkipFields && value != null ? AlwaysAddAll(fieldName, value, valueRevealer, keyRevealer) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer) 
       where TKey : TKRevealBase 
       where TValue : TVRevealBase 
       where TKRevealBase : notnull
       where TVRevealBase : notnull =>
      !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
      !stb.SkipFields && value != null ? AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer) : stb.StyleTypeBuilder;


}
