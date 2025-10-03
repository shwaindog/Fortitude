using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.TypeGeneratePartFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    OrderedCollectionFields;

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct)]
public class BoolSpanWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<bool[]>
{
    public bool[] ComplexTypeCollectionFieldWhenPopulatedAddAllBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public bool[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllBoolSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsNullableStruct)]
public class NullableBoolSpanWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<bool?[]>
{
    public bool?[] ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public bool?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableSpanWhenPopulatedAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt[]>, ISupportsSingleFormatString
    where TFmt : ISpanFormattable
{
    public TFmt[] ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableStructSpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmt[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableStructSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableStructSpan.AsSpan(), FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableNullableClassSpanWhenPopulatedAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt?[]>
  , ISupportsSingleFormatString
    where TFmt : class, ISpanFormattable
{
    public TFmt?[] ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableClassSpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmt?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableClassSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableClassSpan.AsSpan()
              , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableStructSpanWhenPopulatedAddAllStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct[]?>
  , ISupportsSingleFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct[]? ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableSpan.AsSpan(), FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class NullableSpanFormattableSpanWhenPopulatedAddAllStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>
  , ISupportsSingleFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableSpan.AsSpan(), FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class CloakedBearerSpanWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<TCloaked[]>
  , ISupportsSingleRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked[] ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked[] Value { get; set; } = null!;

    public PalantírReveal<TCloakedBase> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerSpan.AsSpan(), PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class CloakedBearerNullableRefSpanWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase>
    : IStringBearer, IMoldSupportedValue<TCloaked?[]> where TCloaked : class, TCloakedBase
{
    public TCloaked?[] ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked?[] Value { get; set; } = null!;

    public PalantírReveal<TCloakedBase> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan.AsSpan()
              , PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class NullableCloakedBearerSpanWhenPopulatedAddAllStringBearer<TCloakedStruct>
    : IStringBearer, IMoldSupportedValue<TCloakedStruct?[]?>, ISupportsSingleRevealer<TCloakedStruct>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloakedStruct?[]? Value { get; set; }


    public PalantírReveal<TCloakedStruct> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerSpan.AsSpan(), PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class StringBearerSpanWhenPopulatedAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer[]?>
    where TBearer : IStringBearer
{
    public TBearer[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerNullableRefSpanWhenPopulatedAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?[]?>
    where TBearer : class, IStringBearer
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public TBearer?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerSpanWhenPopulatedAddAllStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<TBearerStruct?[]?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class StringSpanWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<string[]?>, ISupportsSingleFormatString
{
    public string[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public string[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan.AsSpan(), FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class StringNullableSpanWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<string?[]?>, ISupportsSingleFormatString
{
    public string?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan.AsSpan(), FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class CharSequenceSpanWhenPopulatedAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq[]?>, ISupportsSingleFormatString
    where TCharSeq : ICharSequence
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceSpan.AsSpan(), FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class CharSequenceNullableSpanWhenPopulatedAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq?[]?>
  , ISupportsSingleFormatString where TCharSeq : ICharSequence
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeqNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan.AsSpan()
              , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class StringBuilderSpanWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder[]>, ISupportsSingleFormatString
{
    public StringBuilder[] ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan.AsSpan(), FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class StringBuilderNullableSpanWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?[]>
  , ISupportsSingleFormatString
{
    public StringBuilder?[] ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan.AsSpan(), FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsAny | OneFormatString)]
public class MatchSpanWhenPopulatedAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<T[]?>, ISupportsSingleFormatString
{
    public T[]? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public T[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllMatchSpan.AsSpan(), FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsAny | OneFormatString)]
public class MatchNullableSpanWhenPopulatedAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<T?[]?>, ISupportsSingleFormatString
{
    public T?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public T?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchNullableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllMatchNullableSpan.AsSpan(), FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | OneFormatString)]
public class ObjectSpanWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<object[]?>, ISupportsSingleFormatString
{
    public object[]? ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public object[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan.AsSpan(), FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableClass | OneFormatString)]
public class ObjectNullableSpanWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<object?[]?>, ISupportsSingleFormatString
{
    public object?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableRefSpan
    {
        get => Value;
        set => Value = value;
    }

    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObjectNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableRefSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableRefSpan.AsSpan()
              , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct)]
public class BoolReadOnlySpanWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<bool[]>
{
    public bool[] ComplexTypeCollectionFieldWhenPopulatedAddAllBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public bool[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolReadOnlySpan)
              , (ReadOnlySpan<bool>)ComplexTypeCollectionFieldWhenPopulatedAddAllBoolReadOnlySpan)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct)]
public class NullableBoolReadOnlySpanWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<bool?[]>
{
    public bool?[] ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public bool?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolReadOnlySpan)
              , (ReadOnlySpan<bool?>)ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolReadOnlySpan)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableReadOnlySpanWhenPopulatedAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt[]>
  , ISupportsSingleFormatString
    where TFmt : ISpanFormattable
{
    public TFmt[] ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmt[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmt>)ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan
              , FormatString)
           .Complete();


    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableNullableRefReadOnlySpanWhenPopulatedAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt?[]>
  , ISupportsSingleFormatString where TFmt : class, ISpanFormattable
{
    public TFmt?[] ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableRefReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmt?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableRefReadOnlySpan)
              , (ReadOnlySpan<TFmt?>)ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableRefReadOnlySpan, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString | SupportsCustomHandling)]
public class SpanFormattableNullableStructReadOnlySpanWhenPopulatedAddAllStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>
  , ISupportsSingleFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmtStruct?>)ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan
              , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString | SupportsCustomHandling)]
public class NullableReadOnlySpanFormattableSpanWhenPopulatedAddAllStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>
  , ISupportsSingleFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmtStruct?>)ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableReadOnlySpan
              , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class CloakedBearerReadOnlySpanWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<TCloaked[]>
  , ISupportsSingleRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked[] ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked[] Value { get; set; } = null!;

    public PalantírReveal<TCloakedBase> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloaked>)ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerReadOnlySpan
              , PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class CloakedBearerNullableRefReadOnlySpanWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase> : IStringBearer
  , IMoldSupportedValue<TCloaked?[]>
  , ISupportsSingleRevealer<TCloakedBase> where TCloaked : class, TCloakedBase
{
    public TCloaked?[] ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked?[] Value { get; set; } = null!;

    public PalantírReveal<TCloakedBase> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan)
              , (ReadOnlySpan<TCloaked?>)ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan.AsSpan()
              , PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class NullableCustomBearerReadOnlySpanWhenPopulatedAddAllStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<TCloakedStruct?[]?>
  , ISupportsSingleRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCustomBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public PalantírReveal<TCloakedStruct> PalantirRevealer { get; set; } = null!;

    public TCloakedStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCustomBearerReadOnlySpan)
              , (ReadOnlySpan<TCloakedStruct?>)ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCustomBearerReadOnlySpan
              , PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class StringBearerReadOnlySpanWhenPopulatedAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer[]?>
    where TBearer : IStringBearer
{
    public TBearer[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearer>)ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerNullableRefReadOnlySpanWhenPopulatedAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?[]?>
    where TBearer : class, IStringBearer
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TBearer?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearer?>)ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerReadOnlySpanWhenPopulatedAddAllStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<TBearerStruct?[]?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearerStruct?>)ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerReadOnlySpan)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class StringReadOnlySpanWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<string[]?>, ISupportsSingleFormatString
{
    public string[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringReadOnlySpan)
              , (ReadOnlySpan<string>)ComplexTypeCollectionFieldWhenPopulatedAddAllStringReadOnlySpan, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class StringNullableReadOnlySpanWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<string?[]?>, ISupportsSingleFormatString
{
    public string?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringNullableReadOnlySpan)
              , (ReadOnlySpan<string?>)ComplexTypeCollectionFieldWhenPopulatedAddAllStringNullableReadOnlySpan
              , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class CharSequenceReadOnlySpanWhenPopulatedAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq[]?>
  , ISupportsSingleFormatString
    where TCharSeq : ICharSequence
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceReadOnlySpan)
              , (ReadOnlySpan<TCharSeq>)ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceReadOnlySpan
              , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class CharSequenceNullableReadOnlySpanWhenPopulatedAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq?[]?>
  , ISupportsSingleFormatString
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeqNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan)
              , (ReadOnlySpan<TCharSeq?>)ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan
              , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class StringBuilderReadOnlySpanWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder[]>
  , ISupportsSingleFormatString
{
    public StringBuilder[] ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderReadOnlySpan)
              , (ReadOnlySpan<StringBuilder>)ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderReadOnlySpan
              , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class StringBuilderNullableReadOnlySpanWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?[]>
  , ISupportsSingleFormatString
{
    public StringBuilder?[] ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderNullableReadOnlySpan)
              , (ReadOnlySpan<StringBuilder?>)ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderNullableReadOnlySpan
              , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsClass | AcceptsStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | OneFormatString)]
public class MatchReadOnlySpanWhenPopulatedAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<T[]?>, ISupportsSingleFormatString
{
    public T[]? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public T[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchReadOnlySpan)
              , (ReadOnlySpan<T>)ComplexTypeCollectionFieldWhenPopulatedAddAllMatchReadOnlySpan, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableClass | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | OneFormatString)]
public class MatchNullableReadOnlySpanWhenPopulatedAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<T?[]?>, ISupportsSingleFormatString
{
    public T?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public T?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchNullableReadOnlySpan)
              , (ReadOnlySpan<T?>)ComplexTypeCollectionFieldWhenPopulatedAddAllMatchNullableReadOnlySpan
              , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsClass | AcceptsStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | OneFormatString)]
public class ObjectReadOnlySpanWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<object[]?>, ISupportsSingleFormatString
{
    public object[]? ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public object[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan)
              , (ReadOnlySpan<object>)ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableClass | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | OneFormatString)]
public class ObjectNullableReadOnlySpanWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<object?[]?>, ISupportsSingleFormatString
{
    public object?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObjectNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableReadOnlySpan)
              , (ReadOnlySpan<object?>)ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableReadOnlySpan
              , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct)]
public class BoolArrayWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<bool[]?>
{
    public bool[]? ComplexTypeCollectionFieldWhenPopulatedAddAllBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public bool[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllBoolArray)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct)]
public class NullableBoolArrayWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<bool?[]?>
{
    public bool?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public bool?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolArray)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass |
                  AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableArrayWhenPopulatedAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt?[]?>, ISupportsSingleFormatString
    where TFmt : ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public TFmt?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableArray
              , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class NullableSpanFormattableArrayWhenPopulatedAddAllStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>
  , ISupportsSingleFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableArray)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class CloakedBearerArrayWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<TCloaked[]?>
  , ISupportsSingleRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked[]? Value { get; set; }

    public PalantírReveal<TCloakedBase> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerArray, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class NullableCloakedBearerArrayWhenPopulatedAddAllStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<TCloakedStruct?[]?>
  , ISupportsSingleRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public TCloakedStruct?[]? Value { get; set; }

    public PalantírReveal<TCloakedStruct> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerArray, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerArrayWhenPopulatedAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer[]?>
    where TBearer : IStringBearer
{
    public TBearer[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerArray)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerArrayWhenPopulatedAddAllStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<TBearerStruct?[]?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerArray)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class StringArrayWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<string?[]?>, ISupportsSingleFormatString
{
    public string?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringArray
    {
        get => Value;
        set => Value = value;
    }

    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringArray, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class CharSequenceArrayWhenPopulatedAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq?[]?>
  , ISupportsSingleFormatString
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceArray
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceArray)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class StringBuilderArrayWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?[]?>, ISupportsSingleFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderArray
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderArray, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsAny | OneFormatString)]
public class MatchArrayWhenPopulatedAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<T[]?>, ISupportsSingleFormatString
{
    public T[]? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchArray
    {
        get => Value;
        set => Value = value;
    }

    public T[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllMatchArray, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsAny | OneFormatString)]
public class ObjectArrayWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<object?[]?>, ISupportsSingleFormatString
{
    public object?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableObjectArray
    {
        get => Value;
        set => Value = value;
    }

    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableObjectArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableObjectArray, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct)]
public class BoolListWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<bool>?>
{
    public IReadOnlyList<bool>? ComplexTypeCollectionFieldWhenPopulatedAddAllBoolList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllBoolList)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct)]
public class NullableBoolListWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<bool?>?>
{
    public IReadOnlyList<bool?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolList)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass |
                  AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableListWhenPopulatedAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<IReadOnlyList<TFmt>?>
  , ISupportsSingleFormatString
    where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt>? ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TFmt>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableList, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class NullableSpanFormattableListWhenPopulatedAddAllStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<IReadOnlyList<TFmtStruct?>?>,
    ISupportsSingleFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableList, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class CustomBearerListWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<IReadOnlyList<TCloaked>?>
  , ISupportsSingleRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public IReadOnlyList<TCloaked>? ComplexTypeCollectionFieldWhenPopulatedAddAllCustomBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedBase> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCustomBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCustomBearerList, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct | AcceptsSpanFormattable | AcceptsIntegerNumber
                | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | OnePalantirRevealer)]
public class NullableCustomBearerListWhenPopulatedAddAllStringBearer<TCloakedStruct> : IStringBearer
  , IMoldSupportedValue<IReadOnlyList<TCloakedStruct?>?>
  , ISupportsSingleRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCustomBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TCloakedStruct?>? Value { get; set; }

    public PalantírReveal<TCloakedStruct> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCustomBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCustomBearerList, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerListWhenPopulatedAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<IReadOnlyList<TBearer>?>
    where TBearer : IStringBearer
{
    public IReadOnlyList<TBearer>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TBearer>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerList)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerListWhenPopulatedAddAllStringBearer<TBearerStruct> : IStringBearer
  , IMoldSupportedValue<IReadOnlyList<TBearerStruct?>?>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerList)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class StringListWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<string?>?>, ISupportsSingleFormatString
{
    public IReadOnlyList<string?>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringList, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class CharSequenceListWhenPopulatedAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<IReadOnlyList<TCharSeq?>?>
  , ISupportsSingleFormatString
    where TCharSeq : ICharSequence
{
    public IReadOnlyList<TCharSeq?>? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceList, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class StringBuilderListWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<StringBuilder?>?>
  , ISupportsSingleFormatString
{
    public IReadOnlyList<StringBuilder?>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderList, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsAny | OneFormatString)]
public class MatchListWhenPopulatedAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<IReadOnlyList<T>?>, ISupportsSingleFormatString
{
    public IReadOnlyList<T>? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<T>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllMatchList, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsAny | OneFormatString)]
public class ObjectListWhenPopulatedAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<IReadOnlyList<T>?>, ISupportsSingleFormatString
    where T : class
{
    public IReadOnlyList<T>? ComplexTypeCollectionFieldWhenPopulatedAddAllObjectList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<T>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllObjectList, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}
