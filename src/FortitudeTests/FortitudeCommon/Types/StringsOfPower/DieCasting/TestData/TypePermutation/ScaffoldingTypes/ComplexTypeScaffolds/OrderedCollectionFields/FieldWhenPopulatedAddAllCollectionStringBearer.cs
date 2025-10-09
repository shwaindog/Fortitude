using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    OrderedCollectionFields;

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsStruct)]
public class FieldBoolSpanWhenPopulatedAddAllStringBearer : IMoldSupportedValue<bool[]>
{
    public bool[] ComplexTypeCollectionFieldWhenPopulatedAddAllBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolSpan);
    public bool[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllBoolSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsNullableStruct)]
public class FieldNullableBoolSpanWhenPopulatedAddAllStringBearer : IMoldSupportedValue<bool?[]>
{
    public bool?[] ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolSpan);
    public bool?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableSpanWhenPopulatedAddAllStringBearer<TFmt> : IMoldSupportedValue<TFmt[]>, ISupportsValueFormatString
    where TFmt : ISpanFormattable
{
    public TFmt[] ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableSpan);
    public TFmt[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassSpanWhenPopulatedAddAllStringBearer<TFmt> : IMoldSupportedValue<TFmt?[]>
  , ISupportsValueFormatString
    where TFmt : class, ISpanFormattable
{
    public TFmt?[] ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableClassSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableClassSpan);
    public TFmt?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableClassSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableClassSpan.AsSpan()
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableSpanWhenPopulatedAddAllStringBearer<TFmtStruct> : IMoldSupportedValue<TFmtStruct[]?>
  , ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableSpan);
    public TFmtStruct[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerSpanWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase> : IMoldSupportedValue<TCloaked[]>
  , ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked[] ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerSpan);
    public TCloaked[] Value { get; set; } = null!;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerSpan.AsSpan(), ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerNullableClassSpanWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase>
    : IMoldSupportedValue<TCloaked?[]>, ISupportsValueRevealer<TCloakedBase> where TCloaked : class, TCloakedBase
{
    public TCloaked?[] ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan);
    public TCloaked?[] Value { get; set; } = null!;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan.AsSpan()
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerSpanWhenPopulatedAddAllStringBearer<TCloakedStruct>
    : IMoldSupportedValue<TCloakedStruct?[]?>, ISupportsValueRevealer<TCloakedStruct>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerSpan);
    public TCloakedStruct?[]? Value { get; set; }


    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerSpan.AsSpan(), ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class FieldStringBearerSpanWhenPopulatedAddAllStringBearer<TBearer> : IMoldSupportedValue<TBearer[]?>
    where TBearer : IStringBearer
{
    public TBearer[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan);
    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerNullableClassSpanWhenPopulatedAddAllStringBearer<TBearer> : IMoldSupportedValue<TBearer?[]?>
    where TBearer : class, IStringBearer
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan);
    public TBearer?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerSpanWhenPopulatedAddAllStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct?[]?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerSpan);
    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringSpanWhenPopulatedAddAllStringBearer : IMoldSupportedValue<string[]?>, ISupportsValueFormatString
{
    public string[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan);
    public string[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringNullableSpanWhenPopulatedAddAllStringBearer : IMoldSupportedValue<string?[]?>, ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan);
    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceSpanWhenPopulatedAddAllStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq[]?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceSpan);
    public TCharSeq[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceNullableSpanWhenPopulatedAddAllStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq?[]?>
  , ISupportsValueFormatString where TCharSeq : ICharSequence
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan);
    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeqNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan.AsSpan()
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderSpanWhenPopulatedAddAllStringBearer : IMoldSupportedValue<StringBuilder[]>, ISupportsValueFormatString
{
    public StringBuilder[] ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan);
    public StringBuilder[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderNullableSpanWhenPopulatedAddAllStringBearer : IMoldSupportedValue<StringBuilder?[]>
  , ISupportsValueFormatString
{
    public StringBuilder?[] ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan);
    public StringBuilder?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchSpanWhenPopulatedAddAllStringBearer<T> : IMoldSupportedValue<T[]?>, ISupportsValueFormatString
{
    public T[]? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchSpan);
    public T[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllMatchSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchNullableSpanWhenPopulatedAddAllStringBearer<T> : IMoldSupportedValue<T?[]?>, ISupportsValueFormatString
{
    public T?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchNullableSpan);
    public T?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchNullableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllMatchNullableSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | SupportsValueFormatString)]
public class FieldObjectSpanWhenPopulatedAddAllStringBearer : IMoldSupportedValue<object[]?>, ISupportsValueFormatString
{
    public object[]? ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan);
    public object[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldNullableObjectSpanWhenPopulatedAddAllStringBearer : IMoldSupportedValue<object?[]?>, ISupportsValueFormatString
{
    public object?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableRefSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableRefSpan);
    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObjectNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableRefSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableRefSpan.AsSpan()
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsStruct)]
public class FieldBoolReadOnlySpanWhenPopulatedAddAllStringBearer : IMoldSupportedValue<bool[]>
{
    public bool[] ComplexTypeCollectionFieldWhenPopulatedAddAllBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolReadOnlySpan);
    public bool[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolReadOnlySpan)
              , (ReadOnlySpan<bool>)ComplexTypeCollectionFieldWhenPopulatedAddAllBoolReadOnlySpan)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsNullableStruct)]
public class FieldNullableBoolReadOnlySpanWhenPopulatedAddAllStringBearer : IMoldSupportedValue<bool?[]>
{
    public bool?[] ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolReadOnlySpan);
    public bool?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolReadOnlySpan)
              , (ReadOnlySpan<bool?>)ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolReadOnlySpan)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableReadOnlySpanWhenPopulatedAddAllStringBearer<TFmt> : IMoldSupportedValue<TFmt[]>
  , ISupportsValueFormatString
    where TFmt : ISpanFormattable
{
    public TFmt[] ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan);
    public TFmt[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmt>)ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan
              , ValueFormatString)
           .Complete();


    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassReadOnlySpanWhenPopulatedAddAllStringBearer<TFmt> : IMoldSupportedValue<TFmt?[]>
  , ISupportsValueFormatString where TFmt : class, ISpanFormattable
{
    public TFmt?[] ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableRefReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableRefReadOnlySpan);
    public TFmt?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableRefReadOnlySpan)
              , (ReadOnlySpan<TFmt?>)ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableRefReadOnlySpan, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldNullableSpanFormattableReadOnlySpanWhenPopulatedAddAllStringBearer<TFmtStruct> : IMoldSupportedValue<TFmtStruct?[]?>
  , ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan);
    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmtStruct?>)ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerReadOnlySpanWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase> : IMoldSupportedValue<TCloaked[]>
  , ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked[] ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerReadOnlySpan);
    public TCloaked[] Value { get; set; } = null!;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloaked>)ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerReadOnlySpan
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerNullableClassReadOnlySpanWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase> : 
  IMoldSupportedValue<TCloaked?[]>
  , ISupportsValueRevealer<TCloakedBase> where TCloaked : class, TCloakedBase
{
    public TCloaked?[] ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan);
    public TCloaked?[] Value { get; set; } = null!;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan)
              , (ReadOnlySpan<TCloaked?>)ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan.AsSpan()
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerReadOnlySpanWhenPopulatedAddAllStringBearer<TCloakedStruct> : IMoldSupportedValue<TCloakedStruct?[]?>
  , ISupportsValueRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerReadOnlySpan);
    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public TCloakedStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloakedStruct?>)ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerReadOnlySpan
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class FieldStringBearerReadOnlySpanWhenPopulatedAddAllStringBearer<TBearer> : IMoldSupportedValue<TBearer[]?>
    where TBearer : IStringBearer
{
    public TBearer[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan);
    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearer>)ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerNullableClassReadOnlySpanWhenPopulatedAddAllStringBearer<TBearer> : IMoldSupportedValue<TBearer?[]?>
    where TBearer : class, IStringBearer
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan);
    public TBearer?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearer?>)ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerReadOnlySpanWhenPopulatedAddAllStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct?[]?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerReadOnlySpan);
    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearerStruct?>)ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerReadOnlySpan)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringReadOnlySpanWhenPopulatedAddAllStringBearer : IMoldSupportedValue<string[]?>, ISupportsValueFormatString
{
    public string[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringReadOnlySpan);
    public string[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringReadOnlySpan)
              , (ReadOnlySpan<string>)ComplexTypeCollectionFieldWhenPopulatedAddAllStringReadOnlySpan, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringNullableReadOnlySpanWhenPopulatedAddAllStringBearer : IMoldSupportedValue<string?[]?>, ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringNullableReadOnlySpan);
    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringNullableReadOnlySpan)
              , (ReadOnlySpan<string?>)ComplexTypeCollectionFieldWhenPopulatedAddAllStringNullableReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceReadOnlySpanWhenPopulatedAddAllStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq[]?>
  , ISupportsValueFormatString
    where TCharSeq : ICharSequence
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceReadOnlySpan);
    public TCharSeq[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceReadOnlySpan)
              , (ReadOnlySpan<TCharSeq>)ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceNullableReadOnlySpanWhenPopulatedAddAllStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq?[]?>
  , ISupportsValueFormatString
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan);
    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeqNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan)
              , (ReadOnlySpan<TCharSeq?>)ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderReadOnlySpanWhenPopulatedAddAllStringBearer : IMoldSupportedValue<StringBuilder[]>
  , ISupportsValueFormatString
{
    public StringBuilder[] ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderReadOnlySpan);
    public StringBuilder[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderReadOnlySpan)
              , (ReadOnlySpan<StringBuilder>)ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderNullableReadOnlySpanWhenPopulatedAddAllStringBearer : IMoldSupportedValue<StringBuilder?[]>
  , ISupportsValueFormatString
{
    public StringBuilder?[] ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderNullableReadOnlySpan);
    public StringBuilder?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderNullableReadOnlySpan)
              , (ReadOnlySpan<StringBuilder?>)ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderNullableReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsClass | AcceptsStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldMatchReadOnlySpanWhenPopulatedAddAllStringBearer<T> : IMoldSupportedValue<T[]?>, ISupportsValueFormatString
{
    public T[]? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchReadOnlySpan);
    public T[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchReadOnlySpan)
              , (ReadOnlySpan<T>)ComplexTypeCollectionFieldWhenPopulatedAddAllMatchReadOnlySpan, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsNullableClass | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldMatchNullableReadOnlySpanWhenPopulatedAddAllStringBearer<T> : IMoldSupportedValue<T?[]?>, ISupportsValueFormatString
{
    public T?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchNullableReadOnlySpan);
    public T?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchNullableReadOnlySpan)
              , (ReadOnlySpan<T?>)ComplexTypeCollectionFieldWhenPopulatedAddAllMatchNullableReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsClass | AcceptsStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldObjectReadOnlySpanWhenPopulatedAddAllStringBearer : IMoldSupportedValue<object[]?>, ISupportsValueFormatString
{
    public object[]? ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan);
    public object[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan)
              , (ReadOnlySpan<object>)ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsNullableClass | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableObjectReadOnlySpanWhenPopulatedAddAllStringBearer : IMoldSupportedValue<object?[]?>, ISupportsValueFormatString
{
    public object?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableReadOnlySpan);
    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObjectNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableReadOnlySpan)
              , (ReadOnlySpan<object?>)ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | AcceptsStruct)]
public class FieldBoolArrayWhenPopulatedAddAllStringBearer : IMoldSupportedValue<bool[]?>
{
    public bool[]? ComplexTypeCollectionFieldWhenPopulatedAddAllBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolArray);
    public bool[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllBoolArray)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | AcceptsNullableStruct)]
public class FieldNullableBoolArrayWhenPopulatedAddAllStringBearer : IMoldSupportedValue<bool?[]?>
{
    public bool?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolArray);
    public bool?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolArray)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass |
                  AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableArrayWhenPopulatedAddAllStringBearer<TFmt> : IMoldSupportedValue<TFmt?[]?>, ISupportsValueFormatString
    where TFmt : ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableArray);
    public TFmt?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableArray
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableArrayWhenPopulatedAddAllStringBearer<TFmtStruct> : IMoldSupportedValue<TFmtStruct?[]?>
  , ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableArray);
    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableArray)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerArrayWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase> : IMoldSupportedValue<TCloaked[]?>
  , ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerArray);
    public TCloaked[]? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerArray, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerArrayWhenPopulatedAddAllStringBearer<TCloakedStruct> : IMoldSupportedValue<TCloakedStruct?[]?>
  , ISupportsValueRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerArray);
    public TCloakedStruct?[]? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerArray, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerArrayWhenPopulatedAddAllStringBearer<TBearer> : IMoldSupportedValue<TBearer[]?>
    where TBearer : IStringBearer
{
    public TBearer[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerArray);
    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerArray)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerArrayWhenPopulatedAddAllStringBearer<TBearerStruct> : IMoldSupportedValue<TBearerStruct?[]?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerArray);
    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerArray)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringArrayWhenPopulatedAddAllStringBearer : IMoldSupportedValue<string?[]?>, ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringArray);
    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringArray, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceArrayWhenPopulatedAddAllStringBearer<TCharSeq> : IMoldSupportedValue<TCharSeq?[]?>
  , ISupportsValueFormatString
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceArray);
    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceArray)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderArrayWhenPopulatedAddAllStringBearer : IMoldSupportedValue<StringBuilder?[]?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderArray);
    public StringBuilder?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderArray, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchArrayWhenPopulatedAddAllStringBearer<T> : IMoldSupportedValue<T[]?>, ISupportsValueFormatString
{
    public T[]? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchArray);
    public T[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllMatchArray, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldObjectArrayWhenPopulatedAddAllStringBearer : IMoldSupportedValue<object?[]?>, ISupportsValueFormatString
{
    public object?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableObjectArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableObjectArray);
    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableObjectArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableObjectArray, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | AcceptsStruct)]
public class FieldBoolListWhenPopulatedAddAllStringBearer : IMoldSupportedValue<IReadOnlyList<bool>?>
{
    public IReadOnlyList<bool>? ComplexTypeCollectionFieldWhenPopulatedAddAllBoolList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolList);
    public IReadOnlyList<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllBoolList)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | AcceptsNullableStruct)]
public class FieldNullableBoolListWhenPopulatedAddAllStringBearer : IMoldSupportedValue<IReadOnlyList<bool?>?>
{
    public IReadOnlyList<bool?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolList);
    public IReadOnlyList<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolList)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass |
                  AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableListWhenPopulatedAddAllStringBearer<TFmt> : IMoldSupportedValue<IReadOnlyList<TFmt>?>
  , ISupportsValueFormatString
    where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt>? ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableList);
    public IReadOnlyList<TFmt>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableList, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableListWhenPopulatedAddAllStringBearer<TFmtStruct> : IMoldSupportedValue<IReadOnlyList<TFmtStruct?>?>,
    ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableList);
    public IReadOnlyList<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableList, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerListWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase> : IMoldSupportedValue<IReadOnlyList<TCloaked>?>
  , ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public IReadOnlyList<TCloaked>? ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerList);
    public IReadOnlyList<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerList, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | AcceptsNullableStruct | AcceptsSpanFormattable | AcceptsIntegerNumber
                | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer)]
public class FieldNullableCloakedBearerListWhenPopulatedAddAllStringBearer<TCloakedStruct> : 
  IMoldSupportedValue<IReadOnlyList<TCloakedStruct?>?>
  , ISupportsValueRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerList);
    public IReadOnlyList<TCloakedStruct?>? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerList, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerListWhenPopulatedAddAllStringBearer<TBearer> : IMoldSupportedValue<IReadOnlyList<TBearer>?>
    where TBearer : IStringBearer
{
    public IReadOnlyList<TBearer>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerList);
    public IReadOnlyList<TBearer>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerList)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerListWhenPopulatedAddAllStringBearer<TBearerStruct> : 
  IMoldSupportedValue<IReadOnlyList<TBearerStruct?>?>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerList);
    public IReadOnlyList<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerList)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringListWhenPopulatedAddAllStringBearer : IMoldSupportedValue<IReadOnlyList<string?>?>, ISupportsValueFormatString
{
    public IReadOnlyList<string?>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringList);
    public IReadOnlyList<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringList, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceListWhenPopulatedAddAllStringBearer<TCharSeq> : IMoldSupportedValue<IReadOnlyList<TCharSeq?>?>
  , ISupportsValueFormatString
    where TCharSeq : ICharSequence
{
    public IReadOnlyList<TCharSeq?>? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceList);
    public IReadOnlyList<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceList, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderListWhenPopulatedAddAllStringBearer : IMoldSupportedValue<IReadOnlyList<StringBuilder?>?>
  , ISupportsValueFormatString
{
    public IReadOnlyList<StringBuilder?>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderList);
    public IReadOnlyList<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderList, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchListWhenPopulatedAddAllStringBearer<T> : IMoldSupportedValue<IReadOnlyList<T>?>, ISupportsValueFormatString
{
    public IReadOnlyList<T>? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchList);
    public IReadOnlyList<T>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllMatchList, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldObjectListWhenPopulatedAddAllStringBearer<T> : IMoldSupportedValue<IReadOnlyList<T>?>, ISupportsValueFormatString
    where T : class
{
    public IReadOnlyList<T>? ComplexTypeCollectionFieldWhenPopulatedAddAllObjectList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectList);
    public IReadOnlyList<T>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllObjectList, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}
