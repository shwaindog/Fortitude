#region

using System.Collections;
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
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , Span<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , ReadOnlySpan<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeys<TKey, TValue, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , IReadOnlyList<TKSelectDerived> selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectDerived : TKey =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable
        where TKey : notnull  =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate
                (fieldName, value, selectKeys, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : notnull  =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate
                (fieldName, value, selectKeys, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable<TKSelectDerived>
        where TKey : notnull 
        where TKSelectDerived : TKey =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl, TKSelectDerived>
                (fieldName, value, selectKeys, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull 
        where TKSelectDerived : TKey =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerate<TKey, TValue, TKSelectEnumbl, TKSelectDerived>
                (fieldName, value, selectKeys, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysIterate<TKey, TValue, TKSelectEnumtr>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator
        where TKey : notnull
        =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysIterate(fieldName, value, selectKeys, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysIterate<TKey, TValue, TKSelectEnumtr>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator?
        where TKey : notnull
        =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysIterate(fieldName, value, selectKeys, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysIterate<TKey, TValue, TKSelectEnumtr, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator<TKSelectDerived>
        where TKey : notnull
        where TKSelectDerived : TKey
        =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysIterate<TKey, TValue, TKSelectEnumtr, TKSelectDerived>
                (fieldName, value, selectKeys, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysIterate<TKey, TValue, TKSelectEnumtr, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKey : notnull
        where TKSelectDerived : TKey
        =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysIterate<TKey, TValue, TKSelectEnumtr, TKSelectDerived>
                (fieldName, value, selectKeys, valueFormatString  ?? "", keyFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerateValueRevealer<TKey, TValue, TKSelectEnumbl, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable
        where TKey : notnull 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerateValueRevealer(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerateValueRevealer<TKey, TValue, TKSelectEnumbl, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : notnull 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerateValueRevealer(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerateValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable<TKSelectDerived>
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerateValueRevealer(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerateValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerateValueRevealer(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, TKSelectEnumbl>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable
        where TKey : notnull 
        where TValue : struct  =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerateNullValueRevealer(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, TKSelectEnumbl>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : notnull 
        where TValue : struct  =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerateNullValueRevealer(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable<TKSelectDerived>
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerateNullValueRevealer(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, TKSelectEnumbl, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerateNullValueRevealer(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysIterateValueRevealer<TKey, TValue, TKSelectEnumtr, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator
        where TKey : notnull 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysIterateValueRevealer(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysIterateValueRevealer<TKey, TValue, TKSelectEnumtr, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator?
        where TKey : notnull 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysIterateValueRevealer(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysIterateValueRevealer<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator<TKSelectDerived>
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysIterateValueRevealer(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysIterateValueRevealer<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysIterateValueRevealer(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysIterateNullValueRevealer<TKey, TValue, TKSelectEnumtr>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator
        where TKey : notnull 
        where TValue : struct  =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysIterateNullValueRevealer(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysIterateNullValueRevealer<TKey, TValue, TKSelectEnumtr>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator?
        where TKey : notnull 
        where TValue : struct  =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysIterateNullValueRevealer(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysIterateNullValueRevealer<TKey, TValue, TKSelectEnumtr, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator<TKSelectDerived>
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysIterateNullValueRevealer(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysIterateNullValueRevealer<TKey, TValue, TKSelectEnumtr, TKSelectDerived>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKey : notnull 
        where TKSelectDerived : TKey 
        where TValue : struct  =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysIterateNullValueRevealer(fieldName, value, selectKeys, valueRevealer, keyFormatString  ?? "", valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
      !Mws.SkipFields && value != null
        ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
        : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

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
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeys(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerateBothRevealers<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerateBothRevealers(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerateBothRevealers<TKey, TValue, TKSelectEnumbl, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerateBothRevealers(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerateBothRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable<TKSelectDerived>
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerateBothRevealers(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerateBothRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerateBothRevealers(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerateBothWithNulLValueRevealers<TKey, TValue, TKSelectEnumbl, TKRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerateBothWithNullValueRevealers(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerateBothWithNulLValueRevealers<TKey, TValue, TKSelectEnumbl, TKRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable?
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerateBothWithNullValueRevealers(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerateBothWithNulLValueRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : struct, IEnumerable<TKSelectDerived>
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerateBothWithNullValueRevealers(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysEnumerateBothWithNulLValueRevealers<TKey, TValue, TKSelectEnumbl, TKSelectDerived, TKRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumbl? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumbl : IEnumerable<TKSelectDerived>?
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysEnumerateBothWithNullValueRevealers(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysIterateBothRevealers<TKey, TValue, TKSelectEnumtr, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysIterateBothRevealers(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysIterateBothRevealers<TKey, TValue, TKSelectEnumtr, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator?
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysIterateBothRevealers(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysIterateBothRevealers<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator<TKSelectDerived>
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysIterateBothRevealers(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysIterateBothRevealers<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase, TVRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysIterateBothRevealers(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysIterateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumtr, TKRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysIterateBothWithNullValueRevealers(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysIterateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumtr, TKRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator?
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysIterateBothWithNullValueRevealers(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysIterateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : struct, IEnumerator<TKSelectDerived>
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysIterateBothWithNullValueRevealers(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);

    public TExt WhenConditionMetAddWithSelectKeysIterateBothWithNullValueRevealers<TKey, TValue, TKSelectEnumtr, TKSelectDerived, TKRevealBase>(
        bool condition
      , string fieldName
      , IReadOnlyDictionary<TKey, TValue?>? value
      , TKSelectEnumtr? selectKeys
      , PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TKSelectEnumtr : IEnumerator<TKSelectDerived>?
        where TKSelectDerived : TKey 
        where TKey : TKRevealBase 
        where TValue : struct 
        where TKRevealBase : notnull =>
        !Mws.SkipFields && value != null
            ? AlwaysWithSelectKeysIterateBothWithNullValueRevealers(fieldName, value, selectKeys, valueRevealer, keyRevealer, valueFormatString  ?? "", formatFlags)
            : Mws.WasSkipped(value?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>), fieldName, formatFlags);
}
