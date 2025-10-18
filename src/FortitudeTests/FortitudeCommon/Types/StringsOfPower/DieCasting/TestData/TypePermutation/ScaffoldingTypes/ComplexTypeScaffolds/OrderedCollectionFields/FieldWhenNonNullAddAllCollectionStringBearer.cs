using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    OrderedCollectionFields;


[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsStruct)]
public class FieldBoolSpanWhenNonNullAddAllStringBearer : IMoldSupportedValue<bool[]>
{
    public bool[] ComplexTypeCollectionFieldWhenNonNullAddAllBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolSpan);
    public bool[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllBoolSpan.AsSpan())
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsStruct | AcceptsNullableStruct)]
public class FieldNullableBoolSpanWhenNonNullAddAllStringBearer : IMoldSupportedValue<bool?[]>
{
    public bool?[] ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolSpan);
    public bool?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolSpan.AsSpan())
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableSpanWhenNonNullAddAllStringBearer<TFmt> : IMoldSupportedValue<TFmt[]>, ISupportsValueFormatString
    where TFmt : ISpanFormattable
{
    public TFmt[] ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableSpan);
    public TFmt[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassSpanWhenNonNullAddAllStringBearer<TFmt> : IMoldSupportedValue<TFmt?[]>
  , ISupportsValueFormatString
    where TFmt : class, ISpanFormattable
{
    public TFmt?[] ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableNullableClassSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableNullableClassSpan);
    public TFmt?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableNullableClassSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableNullableClassSpan.AsSpan()
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableSpanWhenNonNullAddAllStringBearer<TFmtStruct> : IMoldSupportedValue<TFmtStruct[]?>
  , ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct[]? ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableSpan);
    public TFmtStruct[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerSpanWhenNonNullAddAllStringBearer<TCloaked, TCloakedBase> : IMoldSupportedValue<TCloaked[]>
  , ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked[] ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerSpan);
    public TCloaked[] Value { get; set; } = null!;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerSpan.AsSpan(), ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerNullableClassSpanWhenNonNullAddAllStringBearer<TCloaked, TCloakedBase>
    : IMoldSupportedValue<TCloaked?[]>, ISupportsValueRevealer<TCloakedBase> where TCloaked : class, TCloakedBase
{
    public TCloaked?[] ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerNullableSpan);
    public TCloaked?[] Value { get; set; } = null!;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerNullableSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerNullableSpan.AsSpan()
              , ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerSpanWhenNonNullAddAllStringBearer<TCloakedStruct>
    : IMoldSupportedValue<TCloakedStruct?[]?>, ISupportsValueRevealer<TCloakedStruct>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerSpan);
    public TCloakedStruct?[]? Value { get; set; }

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }


    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerSpan.AsSpan(), ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class FieldStringBearerSpanWhenNonNullAddAllStringBearer<TBearer> : IMoldSupportedValue<TBearer[]?>
    where TBearer : IStringBearer
{
    public TBearer[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerSpan);
    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerSpan.AsSpan())
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerNullableClassSpanWhenNonNullAddAllStringBearer<TBearer> : IMoldSupportedValue<TBearer?[]?>
    where TBearer : class, IStringBearer
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerSpan);
    public TBearer?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerSpan.AsSpan())
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerSpanWhenNonNullAddAllStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct?[]?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerSpan);
    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerSpan.AsSpan())
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsString | SupportsValueFormatString)]
public class FieldStringSpanWhenNonNullAddAllStringBearer : IMoldSupportedValue<string[]?>, ISupportsValueFormatString
{
    public string[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringSpan);
    public string[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsString | SupportsValueFormatString)]
public class FieldStringNullableSpanWhenNonNullAddAllStringBearer : IMoldSupportedValue<string?[]?>, ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringSpan);
    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceSpanWhenNonNullAddAllStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq[]?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceSpan);
    public TCharSeq[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceNullableSpanWhenNonNullAddAllStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq?[]?>
  , ISupportsValueFormatString where TCharSeq : ICharSequence
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceNullableSpan);
    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeqNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceNullableSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceNullableSpan.AsSpan()
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderSpanWhenNonNullAddAllStringBearer : IMoldSupportedValue<StringBuilder[]>, ISupportsValueFormatString
{
    public StringBuilder[] ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderSpan);
    public StringBuilder[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderNullableSpanWhenNonNullAddAllStringBearer : IMoldSupportedValue<StringBuilder?[]>
  , ISupportsValueFormatString
{
    public StringBuilder?[] ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderSpan);
    public StringBuilder?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchSpanWhenNonNullAddAllStringBearer<TAny> : IMoldSupportedValue<TAny[]?>, ISupportsValueFormatString
{
    public TAny[]? ComplexTypeCollectionFieldWhenNonNullAddAllMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchSpan);
    public TAny[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllMatchSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchNullableSpanWhenNonNullAddAllStringBearer<TAny> : IMoldSupportedValue<TAny?[]?>, ISupportsValueFormatString
{
    public TAny?[]? ComplexTypeCollectionFieldWhenNonNullAddAllMatchNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchNullableSpan);
    public TAny?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchNullableSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllMatchNullableSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsStruct | AcceptsClass | SupportsValueFormatString)]
public class FieldObjectSpanWhenNonNullAddAllStringBearer : IMoldSupportedValue<object[]?>, ISupportsValueFormatString
{
    public object[]? ComplexTypeCollectionFieldWhenNonNullAddAllObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectSpan);
    public object[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllObject
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllObjectSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldNullableObjectSpanWhenNonNullAddAllStringBearer : IMoldSupportedValue<object?[]?>, ISupportsValueFormatString
{
    public object?[]? ComplexTypeCollectionFieldWhenNonNullAddAllObjectNullableRefSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectNullableRefSpan);
    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllObjectNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectNullableRefSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllObjectNullableRefSpan.AsSpan()
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsStruct)]
public class FieldBoolReadOnlySpanWhenNonNullAddAllStringBearer : IMoldSupportedValue<bool[]>
{
    public bool[] ComplexTypeCollectionFieldWhenNonNullAddAllBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolReadOnlySpan);
    public bool[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolReadOnlySpan)
              , (ReadOnlySpan<bool>)ComplexTypeCollectionFieldWhenNonNullAddAllBoolReadOnlySpan)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsNullableStruct)]
public class FieldNullableBoolReadOnlySpanWhenNonNullAddAllStringBearer : IMoldSupportedValue<bool?[]>
{
    public bool?[] ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolReadOnlySpan);
    public bool?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolReadOnlySpan)
              , (ReadOnlySpan<bool?>)ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolReadOnlySpan)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableReadOnlySpanWhenNonNullAddAllStringBearer<TFmt> : IMoldSupportedValue<TFmt[]>
  , ISupportsValueFormatString
    where TFmt : ISpanFormattable
{
    public TFmt[] ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableReadOnlySpan);
    public TFmt[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmt>)ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableReadOnlySpan
              , ValueFormatString)
           .Complete();


    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassReadOnlySpanWhenNonNullAddAllStringBearer<TFmt> : IMoldSupportedValue<TFmt?[]>
  , ISupportsValueFormatString where TFmt : class, ISpanFormattable
{
    public TFmt?[] ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableNullableRefReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableNullableRefReadOnlySpan);
    public TFmt?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableNullableRefReadOnlySpan)
              , (ReadOnlySpan<TFmt?>)ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableNullableRefReadOnlySpan, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldNullableSpanFormattableReadOnlySpanWhenNonNullAddAllStringBearer<TFmtStruct> : IMoldSupportedValue<TFmtStruct?[]?>
  , ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableReadOnlySpan);
    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmtStruct?>)ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerReadOnlySpanWhenNonNullAddAllStringBearer<TCloaked, TCloakedBase> : IMoldSupportedValue<TCloaked[]>
  , ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked[] ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerReadOnlySpan);
    public TCloaked[] Value { get; set; } = null!;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloaked>)ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerReadOnlySpan
              , ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerNullableClassReadOnlySpanWhenNonNullAddAllStringBearer<TCloaked, TCloakedBase> : 
  IMoldSupportedValue<TCloaked?[]>
  , ISupportsValueRevealer<TCloakedBase> where TCloaked : class, TCloakedBase
{
    public TCloaked?[] ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerNullableSpan);
    public TCloaked?[] Value { get; set; } = null!;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerNullableSpan)
              , (ReadOnlySpan<TCloaked?>)ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerNullableSpan.AsSpan()
              , ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerReadOnlySpanWhenNonNullAddAllStringBearer<TCloakedStruct> : IMoldSupportedValue<TCloakedStruct?[]?>
  , ISupportsValueRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerReadOnlySpan);
    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public TCloakedStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloakedStruct?>)ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerReadOnlySpan
              , ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class FieldStringBearerReadOnlySpanWhenNonNullAddAllStringBearer<TBearer> : IMoldSupportedValue<TBearer[]?>
    where TBearer : IStringBearer
{
    public TBearer[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerReadOnlySpan);
    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearer>)ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerReadOnlySpan)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerNullableClassReadOnlySpanWhenNonNullAddAllStringBearer<TBearer> : IMoldSupportedValue<TBearer?[]?>
    where TBearer : class, IStringBearer
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerReadOnlySpan);
    public TBearer?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearer?>)ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerReadOnlySpan)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerReadOnlySpanWhenNonNullAddAllStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct?[]?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerReadOnlySpan);
    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearerStruct?>)ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerReadOnlySpan)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsString | SupportsValueFormatString)]
public class FieldStringReadOnlySpanWhenNonNullAddAllStringBearer : IMoldSupportedValue<string[]?>, ISupportsValueFormatString
{
    public string[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringReadOnlySpan);
    public string[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringReadOnlySpan)
              , (ReadOnlySpan<string>)ComplexTypeCollectionFieldWhenNonNullAddAllStringReadOnlySpan, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsString | SupportsValueFormatString)]
public class FieldStringNullableReadOnlySpanWhenNonNullAddAllStringBearer : IMoldSupportedValue<string?[]?>, ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringNullableReadOnlySpan);
    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringNullableReadOnlySpan)
              , (ReadOnlySpan<string?>)ComplexTypeCollectionFieldWhenNonNullAddAllStringNullableReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceReadOnlySpanWhenNonNullAddAllStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq[]?>
  , ISupportsValueFormatString
    where TCharSeq : ICharSequence
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceReadOnlySpan);
    public TCharSeq[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceReadOnlySpan)
              , (ReadOnlySpan<TCharSeq>)ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceNullableReadOnlySpanWhenNonNullAddAllStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq?[]?>
  , ISupportsValueFormatString
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceNullableSpan);
    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeqNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceNullableSpan)
              , (ReadOnlySpan<TCharSeq?>)ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceNullableSpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderReadOnlySpanWhenNonNullAddAllStringBearer : IMoldSupportedValue<StringBuilder[]>
  , ISupportsValueFormatString
{
    public StringBuilder[] ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderReadOnlySpan);
    public StringBuilder[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderReadOnlySpan)
              , (ReadOnlySpan<StringBuilder>)ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderNullableReadOnlySpanWhenNonNullAddAllStringBearer : IMoldSupportedValue<StringBuilder?[]>
  , ISupportsValueFormatString
{
    public StringBuilder?[] ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderNullableReadOnlySpan);
    public StringBuilder?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderNullableReadOnlySpan)
              , (ReadOnlySpan<StringBuilder?>)ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderNullableReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsClass | AcceptsStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldMatchReadOnlySpanWhenNonNullAddAllStringBearer<TAny> : IMoldSupportedValue<TAny[]?>, ISupportsValueFormatString
{
    public TAny[]? ComplexTypeCollectionFieldWhenNonNullAddAllMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchReadOnlySpan);
    public TAny[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchReadOnlySpan)
              , (ReadOnlySpan<TAny>)ComplexTypeCollectionFieldWhenNonNullAddAllMatchReadOnlySpan, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsNullableClass | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldMatchNullableReadOnlySpanWhenNonNullAddAllStringBearer<TAny> : IMoldSupportedValue<TAny?[]?>, ISupportsValueFormatString
{
    public TAny?[]? ComplexTypeCollectionFieldWhenNonNullAddAllMatchNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchNullableReadOnlySpan);
    public TAny?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchNullableReadOnlySpan)
              , (ReadOnlySpan<TAny?>)ComplexTypeCollectionFieldWhenNonNullAddAllMatchNullableReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsClass | AcceptsStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldObjectReadOnlySpanWhenNonNullAddAllStringBearer : IMoldSupportedValue<object[]?>, ISupportsValueFormatString
{
    public object[]? ComplexTypeCollectionFieldWhenNonNullAddAllObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectSpan);
    public object[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllObject
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectSpan)
              , (ReadOnlySpan<object>)ComplexTypeCollectionFieldWhenNonNullAddAllObjectSpan, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsNullableClass | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableObjectReadOnlySpanWhenNonNullAddAllStringBearer : IMoldSupportedValue<object?[]?>, ISupportsValueFormatString
{
    public object?[]? ComplexTypeCollectionFieldWhenNonNullAddAllObjectNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectNullableReadOnlySpan);
    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllObjectNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectNullableReadOnlySpan)
              , (ReadOnlySpan<object?>)ComplexTypeCollectionFieldWhenNonNullAddAllObjectNullableReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | AcceptsStruct)]
public class FieldBoolArrayWhenNonNullAddAllStringBearer : IMoldSupportedValue<bool[]?>
{
    public bool[]? ComplexTypeCollectionFieldWhenNonNullAddAllBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolArray);
    public bool[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolArray)
              , ComplexTypeCollectionFieldWhenNonNullAddAllBoolArray)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | AcceptsNullableStruct)]
public class FieldNullableBoolArrayWhenNonNullAddAllStringBearer : IMoldSupportedValue<bool?[]?>
{
    public bool?[]? ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolArray);
    public bool?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll(nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolArray)
                                            , ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolArray)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableArrayWhenNonNullAddAllStringBearer<TFmt> : IMoldSupportedValue<TFmt[]?>, ISupportsValueFormatString
    where TFmt : ISpanFormattable
{
    public TFmt[]? ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableArray);
    public TFmt[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll(nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableArray)
                                            , ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableArray
                                            , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableArrayWhenNonNullAddAllStringBearer<TStructFmt> : IMoldSupportedValue<TStructFmt?[]?>
  , ISupportsValueFormatString where TStructFmt : struct, ISpanFormattable
{
    public TStructFmt?[]? ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableArray);
    public TStructFmt?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableArray)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableArray, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable
                | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer)]
public class FieldCloakedBearerArrayWhenNonNullAddAllStringBearer<TCloaked, TCloakedBase> : IMoldSupportedValue<TCloaked[]?>
, ISupportsValueRevealer<TCloakedBase>
    where TCloaked : TCloakedBase
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerArray);
    public TCloaked[]? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerArray
              , ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | AcceptsNullableStruct | AcceptsSpanFormattable | AcceptsIntegerNumber
                | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer)]
public class FieldNullableCloakedBearerArrayWhenNonNullAddAllStringBearer<TCloakedStruct> : IMoldSupportedValue<TCloakedStruct?[]?>
  , ISupportsValueRevealer<TCloakedStruct>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerArray);
    public TCloakedStruct?[]? Value { get; set; }

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerArray
              , ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerArrayWhenNonNullAddAllStringBearer<TBearer> : IMoldSupportedValue<TBearer[]?>
    where TBearer : IStringBearer
{
    public TBearer[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerArray);
    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll(nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerArray)
                                               , ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerArray)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerArrayWhenNonNullAddAllStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct?[]?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerArray);
    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerArray)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerArray)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | AcceptsString | SupportsValueFormatString)]
public class FieldStringArrayWhenNonNullAddAllStringBearer : IMoldSupportedValue<string?[]?>, ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringArray);
    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringArray)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringArray, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceArrayWhenNonNullAddAllStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq?[]?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceArray);
    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceArray)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceArray, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderArrayWhenNonNullAddAllStringBearer : IMoldSupportedValue<StringBuilder?[]?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderArray);
    public StringBuilder?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderArray)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderArray, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchArrayWhenNonNullAddAllStringBearer<TAny> : IMoldSupportedValue<TAny[]?>, ISupportsValueFormatString
{
    public TAny[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderArray);
    public TAny[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderArray)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderArray, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldObjectArrayWhenNonNullAddAllStringBearer : IMoldSupportedValue<object?[]?>, ISupportsValueFormatString
{
    public object?[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderArray);
    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllObject
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderArray)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderArray, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | AcceptsStruct)]
public class FieldBoolListWhenNonNullAddAllStringBearer : IMoldSupportedValue<IReadOnlyList<bool>?>
{
    public IReadOnlyList<bool>? ComplexTypeCollectionFieldWhenNonNullAddAllBoolList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolList);
    public IReadOnlyList<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll(nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolList)
                                            , ComplexTypeCollectionFieldWhenNonNullAddAllBoolList)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | AcceptsNullableStruct)]
public class FieldNullableBoolListWhenNonNullAddAllStringBearer : IMoldSupportedValue<IReadOnlyList<bool?>?>
{
    public IReadOnlyList<bool?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolList);
    public IReadOnlyList<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolList)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableListWhenNonNullAddAllStringBearer<TFmt> : IMoldSupportedValue<IReadOnlyList<TFmt>?>
  , ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt>? ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableList);
    public IReadOnlyList<TFmt>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableList, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableListWhenNonNullAddAllStringBearer<TFmtStruct> : IMoldSupportedValue<IReadOnlyList<TFmtStruct?>?>
  , ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableList);
    public IReadOnlyList<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableList, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable
                | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer)]
public class FieldCloakedBearerListWhenNonNullAddAllStringBearer<TCloaked, TCloakedBase> : IMoldSupportedValue<IReadOnlyList<TCloaked>?>
  , ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public IReadOnlyList<TCloaked>? ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerList);
    public IReadOnlyList<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerList, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | AcceptsNullableStruct | AcceptsSpanFormattable | AcceptsIntegerNumber
                | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer)]
public class FieldNullableCloakedBearerListWhenNonNullAddAllStringBearer<TCloakedStruct> : 
  IMoldSupportedValue<IReadOnlyList<TCloakedStruct?>?>, ISupportsValueRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerList);
    public IReadOnlyList<TCloakedStruct?>? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerList, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerListWhenNonNullAddAllStringBearer<TBearer> : IMoldSupportedValue<IReadOnlyList<TBearer>?>
    where TBearer : IStringBearer
{
    public IReadOnlyList<TBearer>? ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerList);
    public IReadOnlyList<TBearer>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerList)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerListWhenNonNullAddAllStringBearer<TBearerStruct> : IMoldSupportedValue<IReadOnlyList<TBearerStruct?>?>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerList);
    public IReadOnlyList<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerList)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | AcceptsString | SupportsValueFormatString)]
public class FieldStringListWhenNonNullAddAllStringBearer : IMoldSupportedValue<IReadOnlyList<string?>?>, ISupportsValueFormatString
{
    public IReadOnlyList<string?>? ComplexTypeCollectionFieldWhenNonNullAddAllStringList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringList);
    public IReadOnlyList<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringList, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceListWhenNonNullAddAllStringBearer<TCharSeq> : IMoldSupportedValue<IReadOnlyList<TCharSeq?>?>
  , ISupportsValueFormatString where TCharSeq : ICharSequence
{
    public IReadOnlyList<TCharSeq?>? ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceList);
    public IReadOnlyList<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceList, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderListWhenNonNullAddAllStringBearer : IMoldSupportedValue<IReadOnlyList<StringBuilder?>?>
  , ISupportsValueFormatString
{
    public IReadOnlyList<StringBuilder?>? ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderList);
    public IReadOnlyList<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderList, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsAnyGeneric | NonNullWrites | SupportsValueFormatString)]
public class FieldMatchListWhenNonNullAddAllStringBearer<TAny> : IMoldSupportedValue<IReadOnlyList<TAny>?>, ISupportsValueFormatString
{
    public IReadOnlyList<TAny>? ComplexTypeCollectionFieldWhenNonNullAddAllMatchList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchList);
    public IReadOnlyList<TAny>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllMatchList, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsAnyGeneric | NonNullWrites | SupportsValueFormatString)]
public class FieldObjectListWhenNonNullAddAllStringBearer : IMoldSupportedValue<IReadOnlyList<object?>?>, ISupportsValueFormatString
{
    public IReadOnlyList<object?>? ComplexTypeCollectionFieldWhenNonNullAddAllMatchList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchList);
    public IReadOnlyList<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllObject
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllMatchList, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}
