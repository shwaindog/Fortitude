using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    OrderedCollectionFields;

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolEnumerableAlwaysAddAllStringBearer : IMoldSupportedValue<IEnumerable<bool>?>, ISupportsValueFormatString
{
    public IEnumerable<bool>? ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerable);
    public IEnumerable<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolEnumerableAlwaysAddAllStringBearer : IMoldSupportedValue<IEnumerable<bool?>?>, ISupportsValueFormatString
{
    public IEnumerable<bool?>? ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerable);
    public IEnumerable<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class FieldSpanFormattableEnumerableAlwaysAddAllStringBearer<TFmt> : IMoldSupportedValue<IEnumerable<TFmt?>?>,
    ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public IEnumerable<TFmt?>? ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerable);
    public IEnumerable<TFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumerableAlwaysAddAllStringBearer<TFmtStruct> : IMoldSupportedValue<IEnumerable<TFmtStruct?>?>
  , ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerable);
    public IEnumerable<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyExceptNullableStruct |
                  SupportsValueRevealer)]
public class FieldCloakedBearerEnumerableAlwaysAddAllStringBearer<TCloaked, TCloakedBase> : 
  IMoldSupportedValue<IEnumerable<TCloaked>?>, ISupportsValueRevealer<TCloakedBase>
    where TCloaked : TCloakedBase
{
    public IEnumerable<TCloaked>? ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerable);
    public IEnumerable<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerable, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyNullableStruct |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerEnumerableAlwaysAddAllStringBearer<TCloakedStruct> : 
  IMoldSupportedValue<IEnumerable<TCloakedStruct?>?>, ISupportsValueRevealer<TCloakedStruct>
    where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerable);
    public IEnumerable<TCloakedStruct?>? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerable, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsTypeAllButNullableStruct | AcceptsStringBearer)]
public class FieldStringBearerEnumerableAlwaysAddAllStringBearer<TBearer> : IMoldSupportedValue<IEnumerable<TBearer>?>
    where TBearer : IStringBearer
{
    public IEnumerable<TBearer>? ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerable);
    public IEnumerable<TBearer>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerable)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerEnumerableAlwaysAddAllStringBearer<TBearerStruct> : IMoldSupportedValue<IEnumerable<TBearerStruct?>?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerable);
    public IEnumerable<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerable)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsString | SupportsValueFormatString)]
public class FieldStringEnumerableAlwaysAddAllStringBearer : IMoldSupportedValue<IEnumerable<string?>?>, ISupportsValueFormatString
{
    public IEnumerable<string?>? ComplexTypeCollectionFieldAlwaysAddAllStringEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringEnumerable);
    public IEnumerable<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllStringEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceEnumerableAlwaysAddAllStringBearer<TCharSeq> : IMoldSupportedValue<IEnumerable<TCharSeq?>?>
  , ISupportsValueFormatString
    where TCharSeq : ICharSequence
{
    public IEnumerable<TCharSeq?>? ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerable);
    public IEnumerable<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderEnumerableAlwaysAddAllStringBearer : IMoldSupportedValue<IEnumerable<StringBuilder?>?>
  , ISupportsValueFormatString
{
    public IEnumerable<StringBuilder?>? ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerable);
    public IEnumerable<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchEnumerableAlwaysAddAllStringBearer<TAny> : IMoldSupportedValue<IEnumerable<TAny>?>, ISupportsValueFormatString
{
    public IEnumerable<TAny>? ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerable);
    public IEnumerable<TAny>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatchEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldObjectEnumerableAlwaysAddAllStringBearer : IMoldSupportedValue<IEnumerable<object?>?>, ISupportsValueFormatString
{
    public IEnumerable<object?>? ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerable);
    public IEnumerable<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObjectEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolEnumeratorAlwaysAddAllStringBearer : IMoldSupportedValue<IEnumerator<bool>?>, ISupportsValueFormatString
{
    public IEnumerator<bool>? ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerator);
    public IEnumerator<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolEnumeratorAlwaysAddAllStringBearer : IMoldSupportedValue<IEnumerator<bool?>?>, ISupportsValueFormatString
{
    public IEnumerator<bool?>? ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerator);
    public IEnumerator<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsSpanFormattableExceptNullableStruct 
                | SupportsValueFormatString)]
public class FieldSpanFormattableEnumeratorAlwaysAddAllStringBearer<TFmt> : IMoldSupportedValue<IEnumerator<TFmt>?>
  , ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public IEnumerator<TFmt>? ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerator);
    public IEnumerator<TFmt>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumeratorAlwaysAddAllStringBearer<TFmtStruct> : IMoldSupportedValue<IEnumerator<TFmtStruct?>?>
  , ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerator);
    public IEnumerator<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyExceptNullableStruct |
                  SupportsValueRevealer)]
public class FieldCloakedBearerEnumeratorAlwaysAddAllStringBearer<TCloaked, TCloakedBase> : IMoldSupportedValue<IEnumerator<TCloaked>?>
  , ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public IEnumerator<TCloaked>? ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerator);
    public IEnumerator<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerator, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyNullableStruct |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerEnumeratorAlwaysAddAllStringBearer<TCloakedStruct> : 
  IMoldSupportedValue<IEnumerator<TCloakedStruct?>?>, ISupportsValueRevealer<TCloakedStruct>
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerator);
    public IEnumerator<TCloakedStruct?>? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerator, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsTypeAllButNullableStruct | AcceptsStringBearer)]
public class FieldStringBearerEnumeratorAlwaysAddAllStringBearer<TBearer> : IMoldSupportedValue<IEnumerator<TBearer>?>
    where TBearer : IStringBearer
{
    public IEnumerator<TBearer>? ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerator);
    public IEnumerator<TBearer>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerator)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerEnumeratorAlwaysAddAllStringBearer<TBearerStruct> : IMoldSupportedValue<IEnumerator<TBearerStruct?>?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerator);
    public IEnumerator<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerator)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsString | SupportsValueFormatString)]
public class FieldStringEnumeratorAlwaysAddAllStringBearer : IMoldSupportedValue<IEnumerator<string?>?>, ISupportsValueFormatString
{
    public IEnumerator<string?>? ComplexTypeCollectionFieldAlwaysAddAllStringEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringEnumerator);
    public IEnumerator<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllStringEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceEnumeratorAlwaysAddAllStringBearer<TCharSeq> : IMoldSupportedValue<IEnumerator<TCharSeq?>?>
  , ISupportsValueFormatString where TCharSeq : ICharSequence
{
    public IEnumerator<TCharSeq?>? ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerator);
    public IEnumerator<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderEnumeratorAlwaysAddAllStringBearer : IMoldSupportedValue<IEnumerator<StringBuilder?>?>
  , ISupportsValueFormatString
{
    public IEnumerator<StringBuilder?>? ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerator);
    public IEnumerator<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchEnumeratorAlwaysAddAllStringBearer<TAny> : IMoldSupportedValue<IEnumerator<TAny>?>, ISupportsValueFormatString
{
    public IEnumerator<TAny>? ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerator);
    public IEnumerator<TAny>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatchEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites 
                | AcceptsNullableObject  | SupportsValueFormatString)]
public class FieldObjectEnumeratorAlwaysAddAllStringBearer : IMoldSupportedValue<IEnumerator<object?>?>, ISupportsValueFormatString
{
    public IEnumerator<object?>? ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerator);
    public IEnumerator<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObjectEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}
