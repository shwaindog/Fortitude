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
      !stb.SkipFields && value != null 
    ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
    : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull  where TKDerived : TKey =>
      !stb.SkipFields && value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull  where TKDerived : TKey =>
      !stb.SkipFields && value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull where TKDerived : TKey =>
      !stb.SkipFields && value != null 
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKDerived>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKey : notnull where TKDerived : TKey =>
      !stb.SkipFields && value != null 
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueFormatString, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKDerived, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKDerived[] selectKeys
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
        where TKDerived : TKey where TValue : TVBase =>
      !stb.SkipFields && value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKDerived, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
      where TKDerived : TKey where TValue : TVBase =>
      !stb.SkipFields && value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKDerived, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKDerived : TKey where TValue : TVBase =>
      !stb.SkipFields && value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKDerived, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys, CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKDerived : TKey where TValue : TVBase =>
      !stb.SkipFields && value != null 
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKDerived, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKDerived> selectKeys, CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) 
      where TKDerived : TKey where TValue : TVBase =>
      !stb.SkipFields && value != null 
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueStyler, keyFormatString) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKDerived, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, TKDerived[] selectKeys
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) 
      where TKDerived : TKey where TKey : TKBase where TValue : TVBase =>
      !stb.SkipFields && value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueStyler, keyStyler) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKDerived, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, ReadOnlySpan<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) 
      where TKDerived : TKey where TKey : TKBase where TValue : TVBase  =>
      !stb.SkipFields && value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueStyler, keyStyler) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeys<TKey, TValue, TKDerived, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IReadOnlyList<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) 
      where TKDerived : TKey where TKey : TKBase where TValue : TVBase  =>
      !stb.SkipFields && value != null 
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueStyler, keyStyler) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKDerived, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerable<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
      where TKDerived : TKey where TKey : TKBase where TValue : TVBase  =>
      !stb.SkipFields && value != null 
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueStyler, keyStyler) 
            : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddWithSelectKeysEnumerate<TKey, TValue, TKDerived, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, IEnumerator<TKDerived> selectKeys
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) 
      where TKDerived : TKey where TKey : TKBase where TValue : TVBase  =>
      !stb.SkipFields && value != null 
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueStyler, keyStyler) 
            : stb.StyleTypeBuilder;
}
