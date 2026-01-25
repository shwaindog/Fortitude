#region

using System.Diagnostics.CodeAnalysis;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#endregion

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public partial class SelectTypeKeyedCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectDerived[] selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , Span<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerable<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectDerived[] selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectDerived[] selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
    , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , Span<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , Span<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerable<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , IEnumerable<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , IEnumerator<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectDerived[] selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectDerived[] selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , Span<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
      where TKSelectDerived : TKey 
      where TKey : TKRevealBase 
      where TValue : TVRevealBase? 
      where TKRevealBase : notnull
      where TVRevealBase : notnull =>
      !stb.SkipFields && value != null
        ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
        : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , Span<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived, TKRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerable<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , IEnumerable<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IEnumerator<TKSelectDerived> selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerate<TKey, TValue, TKSelectDerived, TKRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , IEnumerator<TKSelectDerived> selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !stb.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : stb.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);
}
