using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.TypeGeneratePartFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    OrderedCollectionFields;

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct)]
public class BoolSpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<bool[]>
{
    public bool[] ComplexTypeCollectionFieldAlwaysAddAllBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public bool[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllBoolSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsNullableStruct)]
public class NullableBoolSpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<bool?[]>
{
    public bool?[] ComplexTypeCollectionFieldAlwaysAddAllNullableBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public bool?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableBoolSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SpanFormattableSpanAlwaysAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt[]>, ISupportsValueFormatString
    where TFmt : ISpanFormattable
{
    public TFmt[] ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableStructSpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmt[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableStructSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableStructSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SpanFormattableNullableClassSpanAlwaysAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt?[]>, ISupportsValueFormatString
    where TFmt : class, ISpanFormattable
{
    public TFmt?[] ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableNullableClassSpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmt?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableNullableClassSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableNullableClassSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SpanFormattableStructSpanAlwaysAddAllStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct[]?>
  , ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct[]? ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class NullableSpanFormattableSpanAlwaysAddAllStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>
  , ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class CloakedBearerSpanAlwaysAddAllStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<TCloaked[]>
  , ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked[] ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked[] Value { get; set; } = null!;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerSpan.AsSpan(), ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class CloakedBearerNullableRefSpanAlwaysAddAllStringBearer<TCloaked, TCloakedBase>
    : IStringBearer, IMoldSupportedValue<TCloaked?[]> where TCloaked : class, TCloakedBase
{
    public TCloaked?[] ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked?[] Value { get; set; } = null!;

    public PalantírReveal<TCloakedBase> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerNullableSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerNullableSpan.AsSpan(), PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class NullableCloakedBearerSpanAlwaysAddAllStringBearer<TCloakedStruct>
    : IStringBearer, IMoldSupportedValue<TCloakedStruct?[]?>, ISupportsValueRevealer<TCloakedStruct>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloakedStruct?[]? Value { get; set; }


    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerSpan.AsSpan(), ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class StringBearerSpanAlwaysAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer[]?>
    where TBearer : IStringBearer
{
    public TBearer[]? ComplexTypeCollectionFieldAlwaysAddAllStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBearerSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerNullableRefSpanAlwaysAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?[]?>
    where TBearer : class, IStringBearer
{
    public TBearer?[]? ComplexTypeCollectionFieldAlwaysAddAllStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public TBearer?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBearerSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerSpanAlwaysAddAllStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<TBearerStruct?[]?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class StringSpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<string[]?>, ISupportsValueFormatString
{
    public string[]? ComplexTypeCollectionFieldAlwaysAddAllStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public string[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllStringSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class StringNullableSpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<string?[]?>, ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldAlwaysAddAllStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllStringSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class CharSequenceSpanAlwaysAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq[]?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence
{
    public TCharSeq[]? ComplexTypeCollectionFieldAlwaysAddAllCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllCharSequenceSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class CharSequenceNullableSpanAlwaysAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq?[]?>
  , ISupportsValueFormatString where TCharSeq : ICharSequence
{
    public TCharSeq?[]? ComplexTypeCollectionFieldAlwaysAddAllCharSequenceNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeqNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceNullableSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllCharSequenceNullableSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class StringBuilderSpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder[]>, ISupportsValueFormatString
{
    public StringBuilder[] ComplexTypeCollectionFieldAlwaysAddAllStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBuilderSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class StringBuilderNullableSpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?[]>, ISupportsValueFormatString
{
    public StringBuilder?[] ComplexTypeCollectionFieldAlwaysAddAllStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBuilderSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsAny | SupportsValueFormatString)]
public class MatchSpanAlwaysAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<T[]?>, ISupportsValueFormatString
{
    public T[]? ComplexTypeCollectionFieldAlwaysAddAllMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public T[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatch
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllMatchSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsAny | SupportsValueFormatString)]
public class MatchNullableSpanAlwaysAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<T?[]?>, ISupportsValueFormatString
{
    public T?[]? ComplexTypeCollectionFieldAlwaysAddAllMatchNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public T?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatch
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchNullableSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllMatchNullableSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass | SupportsValueFormatString)]
public class ObjectSpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<object[]?>, ISupportsValueFormatString
{
    public object[]? ComplexTypeCollectionFieldAlwaysAddAllObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public object[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObject
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllObjectSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableClass | SupportsValueFormatString)]
public class ObjectNullableSpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<object?[]?>, ISupportsValueFormatString
{
    public object?[]? ComplexTypeCollectionFieldAlwaysAddAllObjectNullableRefSpan
    {
        get => Value;
        set => Value = value;
    }

    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObjectNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectNullableRefSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllObjectNullableRefSpan.AsSpan(), ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct)]
public class BoolReadOnlySpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<bool[]>
{
    public bool[] ComplexTypeCollectionFieldAlwaysAddAllBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public bool[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolReadOnlySpan)
              , (ReadOnlySpan<bool>)ComplexTypeCollectionFieldAlwaysAddAllBoolReadOnlySpan)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct)]
public class NullableBoolReadOnlySpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<bool?[]>
{
    public bool?[] ComplexTypeCollectionFieldAlwaysAddAllNullableBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public bool?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolReadOnlySpan)
              , (ReadOnlySpan<bool?>)ComplexTypeCollectionFieldAlwaysAddAllNullableBoolReadOnlySpan)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SpanFormattableReadOnlySpanAlwaysAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt[]>, ISupportsValueFormatString
    where TFmt : ISpanFormattable
{
    public TFmt[] ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmt[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmt>)ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableReadOnlySpan, ValueFormatString)
           .Complete();


    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SpanFormattableNullableRefReadOnlySpanAlwaysAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt?[]>
  , ISupportsValueFormatString where TFmt : class, ISpanFormattable
{
    public TFmt?[] ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableNullableRefReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmt?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableNullableRefReadOnlySpan)
              , (ReadOnlySpan<TFmt?>)ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableNullableRefReadOnlySpan, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | SupportsCustomHandling)]
public class SpanFormattableNullableStructReadOnlySpanAlwaysAddAllStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>
  , ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmtStruct?>)ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | SupportsCustomHandling)]
public class NullableReadOnlySpanFormattableSpanAlwaysAddAllStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>
  , ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmtStruct?>)ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class CloakedBearerReadOnlySpanAlwaysAddAllStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<TCloaked[]>
  , ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked[] ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked[] Value { get; set; } = null!;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloaked>)ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerReadOnlySpan
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class CloakedBearerNullableRefReadOnlySpanAlwaysAddAllStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<TCloaked?[]>
  , ISupportsValueRevealer<TCloakedBase> where TCloaked : class, TCloakedBase
{
    public TCloaked?[] ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked?[] Value { get; set; } = null!;

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerNullableSpan)
              , (ReadOnlySpan<TCloaked?>)ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerNullableSpan.AsSpan()
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class NullableCustomBearerReadOnlySpanAlwaysAddAllStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<TCloakedStruct?[]?>
  , ISupportsValueRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableCustomBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public TCloakedStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCustomBearerReadOnlySpan)
              , (ReadOnlySpan<TCloakedStruct?>)ComplexTypeCollectionFieldAlwaysAddAllNullableCustomBearerReadOnlySpan
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class StringBearerReadOnlySpanAlwaysAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer[]?>
    where TBearer : IStringBearer
{
    public TBearer[]? ComplexTypeCollectionFieldAlwaysAddAllStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearer>)ComplexTypeCollectionFieldAlwaysAddAllStringBearerReadOnlySpan)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerNullableRefReadOnlySpanAlwaysAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?[]?>
    where TBearer : class, IStringBearer
{
    public TBearer?[]? ComplexTypeCollectionFieldAlwaysAddAllStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TBearer?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearer?>)ComplexTypeCollectionFieldAlwaysAddAllStringBearerReadOnlySpan)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerReadOnlySpanAlwaysAddAllStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<TBearerStruct?[]?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearerStruct?>)ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerReadOnlySpan)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class StringReadOnlySpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<string[]?>, ISupportsValueFormatString
{
    public string[]? ComplexTypeCollectionFieldAlwaysAddAllStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringReadOnlySpan)
              , (ReadOnlySpan<string>)ComplexTypeCollectionFieldAlwaysAddAllStringReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class StringNullableReadOnlySpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<string?[]?>, ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldAlwaysAddAllStringNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringNullableReadOnlySpan)
              , (ReadOnlySpan<string?>)ComplexTypeCollectionFieldAlwaysAddAllStringNullableReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class CharSequenceReadOnlySpanAlwaysAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq[]?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence
{
    public TCharSeq[]? ComplexTypeCollectionFieldAlwaysAddAllCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceReadOnlySpan)
              , (ReadOnlySpan<TCharSeq>)ComplexTypeCollectionFieldAlwaysAddAllCharSequenceReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class CharSequenceNullableReadOnlySpanAlwaysAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq?[]?>
  , ISupportsValueFormatString
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? ComplexTypeCollectionFieldAlwaysAddAllCharSequenceNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeqNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceNullableSpan)
              , (ReadOnlySpan<TCharSeq?>)ComplexTypeCollectionFieldAlwaysAddAllCharSequenceNullableSpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class StringBuilderReadOnlySpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder[]>, ISupportsValueFormatString
{
    public StringBuilder[] ComplexTypeCollectionFieldAlwaysAddAllStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderReadOnlySpan)
              , (ReadOnlySpan<StringBuilder>)ComplexTypeCollectionFieldAlwaysAddAllStringBuilderReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class StringBuilderNullableReadOnlySpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?[]>
  , ISupportsValueFormatString
{
    public StringBuilder?[] ComplexTypeCollectionFieldAlwaysAddAllStringBuilderNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder?[] Value { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderNullableReadOnlySpan)
              , (ReadOnlySpan<StringBuilder?>)ComplexTypeCollectionFieldAlwaysAddAllStringBuilderNullableReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsClass | AcceptsStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueFormatString)]
public class MatchReadOnlySpanAlwaysAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<T[]?>, ISupportsValueFormatString
{
    public T[]? ComplexTypeCollectionFieldAlwaysAddAllMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public T[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatch
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchReadOnlySpan)
              , (ReadOnlySpan<T>)ComplexTypeCollectionFieldAlwaysAddAllMatchReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableClass | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueFormatString)]
public class MatchNullableReadOnlySpanAlwaysAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<T?[]?>, ISupportsValueFormatString
{
    public T?[]? ComplexTypeCollectionFieldAlwaysAddAllMatchNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public T?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatch
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchNullableReadOnlySpan)
              , (ReadOnlySpan<T?>)ComplexTypeCollectionFieldAlwaysAddAllMatchNullableReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsClass | AcceptsStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueFormatString)]
public class ObjectReadOnlySpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<object[]?>, ISupportsValueFormatString
{
    public object[]? ComplexTypeCollectionFieldAlwaysAddAllObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public object[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObject
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectSpan)
              , (ReadOnlySpan<object>)ComplexTypeCollectionFieldAlwaysAddAllObjectSpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableClass | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueFormatString)]
public class ObjectNullableReadOnlySpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<object?[]?>, ISupportsValueFormatString
{
    public object?[]? ComplexTypeCollectionFieldAlwaysAddAllObjectNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObjectNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectNullableReadOnlySpan)
              , (ReadOnlySpan<object?>)ComplexTypeCollectionFieldAlwaysAddAllObjectNullableReadOnlySpan
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct)]
public class BoolArrayAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<bool[]?>
{
    public bool[]? ComplexTypeCollectionFieldAlwaysAddAllBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public bool[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolArray)
              , ComplexTypeCollectionFieldAlwaysAddAllBoolArray)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct)]
public class NullableBoolArrayAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<bool?[]?>
{
    public bool?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public bool?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolArray)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableBoolArray)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SpanFormattableArrayAlwaysAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt?[]?>, ISupportsValueFormatString
    where TFmt : ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public TFmt?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableArray)
              , ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableArray
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class NullableSpanFormattableArrayAlwaysAddAllStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>
  , ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableArray)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableArray)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class CloakedBearerArrayAlwaysAddAllStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<TCloaked[]?>
  , ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public TCloaked[]? ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked[]? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerArray)
              , ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerArray
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class NullableCloakedBearerArrayAlwaysAddAllStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<TCloakedStruct?[]?>
  , ISupportsValueRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public TCloakedStruct?[]? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerArray)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerArray
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerArrayAlwaysAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer[]?>
    where TBearer : IStringBearer
{
    public TBearer[]? ComplexTypeCollectionFieldAlwaysAddAllStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public TBearer[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerArray)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBearerArray)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerArrayAlwaysAddAllStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<TBearerStruct?[]?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public TBearerStruct?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerArray)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerArray)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class StringArrayAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<string?[]?>, ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldAlwaysAddAllStringArray
    {
        get => Value;
        set => Value = value;
    }

    public string?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringArray)
              , ComplexTypeCollectionFieldAlwaysAddAllStringArray
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class CharSequenceArrayAlwaysAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq?[]?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? ComplexTypeCollectionFieldAlwaysAddAllCharSequenceArray
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceArray)
              , ComplexTypeCollectionFieldAlwaysAddAllCharSequenceArray)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class StringBuilderArrayAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?[]?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldAlwaysAddAllStringBuilderArray
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderArray)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBuilderArray
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsAny | SupportsValueFormatString)]
public class MatchArrayAlwaysAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<T[]?>, ISupportsValueFormatString
{
    public T[]? ComplexTypeCollectionFieldAlwaysAddAllMatchArray
    {
        get => Value;
        set => Value = value;
    }

    public T[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatch
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchArray)
              , ComplexTypeCollectionFieldAlwaysAddAllMatchArray
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsAny | SupportsValueFormatString)]
public class ObjectArrayAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<object?[]?>, ISupportsValueFormatString
{
    public object?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableObjectArray
    {
        get => Value;
        set => Value = value;
    }

    public object?[]? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObject
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableObjectArray)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableObjectArray
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct)]
public class BoolListAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<bool>?>
{
    public IReadOnlyList<bool>? ComplexTypeCollectionFieldAlwaysAddAllBoolList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolList)
              , ComplexTypeCollectionFieldAlwaysAddAllBoolList)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct)]
public class NullableBoolListAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<bool?>?>
{
    public IReadOnlyList<bool?>? ComplexTypeCollectionFieldAlwaysAddAllNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolList)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableBoolList)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SpanFormattableListAlwaysAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<IReadOnlyList<TFmt>?>, ISupportsValueFormatString
    where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt>? ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TFmt>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableList)
              , ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableList
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class NullableSpanFormattableListAlwaysAddAllStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<IReadOnlyList<TFmtStruct?>?>,
    ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableList)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableList
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class CustomBearerListAlwaysAddAllStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<IReadOnlyList<TCloaked>?>
  , ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public IReadOnlyList<TCloaked>? ComplexTypeCollectionFieldAlwaysAddAllCustomBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCustomBearerList)
              , ComplexTypeCollectionFieldAlwaysAddAllCustomBearerList
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable | AcceptsIntegerNumber
                | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer)]
public class NullableCustomBearerListAlwaysAddAllStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<IReadOnlyList<TCloakedStruct?>?>
  , ISupportsValueRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableCustomBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TCloakedStruct?>? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCustomBearerList)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableCustomBearerList
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerListAlwaysAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<IReadOnlyList<TBearer>?>
    where TBearer : IStringBearer
{
    public IReadOnlyList<TBearer>? ComplexTypeCollectionFieldAlwaysAddAllStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TBearer>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerList)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBearerList)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerListAlwaysAddAllStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<IReadOnlyList<TBearerStruct?>?>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerList)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerList)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class StringListAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<string?>?>, ISupportsValueFormatString
{
    public IReadOnlyList<string?>? ComplexTypeCollectionFieldAlwaysAddAllStringList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringList)
              , ComplexTypeCollectionFieldAlwaysAddAllStringList
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class CharSequenceListAlwaysAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<IReadOnlyList<TCharSeq?>?>
  , ISupportsValueFormatString
    where TCharSeq : ICharSequence
{
    public IReadOnlyList<TCharSeq?>? ComplexTypeCollectionFieldAlwaysAddAllCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceList)
              , ComplexTypeCollectionFieldAlwaysAddAllCharSequenceList
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class StringBuilderListAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<StringBuilder?>?>
  , ISupportsValueFormatString
{
    public IReadOnlyList<StringBuilder?>? ComplexTypeCollectionFieldAlwaysAddAllStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderList)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBuilderList
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsAny | SupportsValueFormatString)]
public class MatchListAlwaysAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<IReadOnlyList<T>?>, ISupportsValueFormatString
{
    public IReadOnlyList<T>? ComplexTypeCollectionFieldAlwaysAddAllMatchList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<T>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatch
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchList)
              , ComplexTypeCollectionFieldAlwaysAddAllMatchList
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsAny | SupportsValueFormatString)]
public class ObjectListAlwaysAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<IReadOnlyList<T>?>, ISupportsValueFormatString
    where T : class
{
    public IReadOnlyList<T>? ComplexTypeCollectionFieldAlwaysAddAllObjectList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<T>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObject
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectList)
              , ComplexTypeCollectionFieldAlwaysAddAllObjectList
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}
