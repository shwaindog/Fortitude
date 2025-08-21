using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKDerived[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull where TKDerived : TKey =>
    value != null 
    ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
    : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull  where TKDerived : TKey =>
        value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull  where TKDerived : TKey =>
        value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull where TKDerived : TKey =>
        value != null 
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull where TKDerived : TKey =>
        value != null 
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKDerived[] selectKeys
      , CustomTypeStyler<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TValue : struct where TKDerived : TKey =>
        value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKDerived> selectKeys
      , CustomTypeStyler<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
      where TKDerived : TKey where TValue : struct =>
        value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , CustomTypeStyler<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKDerived : TKey where TValue : struct =>
        value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys, CustomTypeStyler<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKDerived : TKey where TValue : struct =>
        value != null 
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKDerived> selectKeys, CustomTypeStyler<TValue> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKDerived : TKey where TValue : struct =>
        value != null 
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKey[] selectKeys
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueStyler, keyStyler) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKey> selectKeys
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueStyler, keyStyler) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKey> selectKeys
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueStyler, keyStyler) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKey> selectKeys
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueStyler, keyStyler) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKey> selectKeys
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler) where TKey : struct where TValue : struct =>
        value != null 
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueStyler, keyStyler) 
            : stb.StyleTypeBuilder;
}
