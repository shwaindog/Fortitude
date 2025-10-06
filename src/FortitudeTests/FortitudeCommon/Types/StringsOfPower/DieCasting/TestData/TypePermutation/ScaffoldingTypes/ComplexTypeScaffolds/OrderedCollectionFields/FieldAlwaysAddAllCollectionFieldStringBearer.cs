using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.TypeGeneratePartFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    OrderedCollectionFields;

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsStruct)]
public class FieldBoolSpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<bool[]>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsStruct | AcceptsNullableStruct)]
public class FieldNullableBoolSpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<bool?[]>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableSpanAlwaysAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt[]>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassSpanAlwaysAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt?[]>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableStructSpanAlwaysAddAllStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct[]?>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableSpanAlwaysAddAllStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerSpanAlwaysAddAllStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<TCloaked[]>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerNullableRefSpanAlwaysAddAllStringBearer<TCloaked, TCloakedBase>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerSpanAlwaysAddAllStringBearer<TCloakedStruct>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class FieldStringBearerSpanAlwaysAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer[]?>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerNullableRefSpanAlwaysAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?[]?>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerSpanAlwaysAddAllStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<TBearerStruct?[]?>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringSpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<string[]?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringNullableSpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<string?[]?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceSpanAlwaysAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq[]?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceNullableSpanAlwaysAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq?[]?>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderSpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder[]>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderNullableSpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?[]>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchSpanAlwaysAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<T[]?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchNullableSpanAlwaysAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<T?[]?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsStruct | AcceptsClass | SupportsValueFormatString)]
public class FieldObjectSpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<object[]?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldObjectNullableSpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<object?[]?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsStruct)]
public class FieldBoolReadOnlySpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<bool[]>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsNullableStruct)]
public class FieldNullableBoolReadOnlySpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<bool?[]>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableReadOnlySpanAlwaysAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt[]>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableNullableRefReadOnlySpanAlwaysAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt?[]>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldSpanFormattableNullableStructReadOnlySpanAlwaysAddAllStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldNullableReadOnlySpanFormattableSpanAlwaysAddAllStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerReadOnlySpanAlwaysAddAllStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<TCloaked[]>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerNullableRefReadOnlySpanAlwaysAddAllStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<TCloaked?[]>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCustomBearerReadOnlySpanAlwaysAddAllStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<TCloakedStruct?[]?>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class FieldStringBearerReadOnlySpanAlwaysAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer[]?>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerNullableRefReadOnlySpanAlwaysAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer?[]?>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerReadOnlySpanAlwaysAddAllStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<TBearerStruct?[]?>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringReadOnlySpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<string[]?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringNullableReadOnlySpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<string?[]?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceReadOnlySpanAlwaysAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq[]?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceNullableReadOnlySpanAlwaysAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq?[]?>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderReadOnlySpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder[]>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderNullableReadOnlySpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?[]>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsClass | AcceptsStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldMatchReadOnlySpanAlwaysAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<T[]?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsNullableClass | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldMatchNullableReadOnlySpanAlwaysAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<T?[]?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsClass | AcceptsStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldObjectReadOnlySpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<object[]?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsNullableClass | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldObjectNullableReadOnlySpanAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<object?[]?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | AlwaysWrites | AcceptsStruct)]
public class FieldBoolArrayAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<bool[]?>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | AlwaysWrites | AcceptsNullableStruct)]
public class FieldNullableBoolArrayAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<bool?[]?>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableArrayAlwaysAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt?[]?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableArrayAlwaysAddAllStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerArrayAlwaysAddAllStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<TCloaked[]?>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | AlwaysWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerArrayAlwaysAddAllStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<TCloakedStruct?[]?>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerArrayAlwaysAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<TBearer[]?>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerArrayAlwaysAddAllStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<TBearerStruct?[]?>
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringArrayAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<string?[]?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceArrayAlwaysAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<TCharSeq?[]?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderArrayAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder?[]?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | AlwaysWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchArrayAlwaysAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<T[]?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType |  AcceptsCollection | AcceptsArray | AlwaysWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldObjectArrayAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<object?[]?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | AcceptsStruct)]
public class FieldBoolListAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<bool>?>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | AcceptsNullableStruct)]
public class FieldNullableBoolListAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<bool?>?>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableListAlwaysAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<IReadOnlyList<TFmt>?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableListAlwaysAddAllStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<IReadOnlyList<TFmtStruct?>?>,
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCustomBearerListAlwaysAddAllStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<IReadOnlyList<TCloaked>?>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable | AcceptsIntegerNumber
                | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer)]
public class FieldNullableCustomBearerListAlwaysAddAllStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<IReadOnlyList<TCloakedStruct?>?>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerListAlwaysAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<IReadOnlyList<TBearer>?>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerListAlwaysAddAllStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<IReadOnlyList<TBearerStruct?>?>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringListAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<string?>?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceListAlwaysAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<IReadOnlyList<TCharSeq?>?>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderListAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<StringBuilder?>?>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchListAlwaysAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<IReadOnlyList<T>?>, ISupportsValueFormatString
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | AcceptsAny | SupportsValueFormatString)]
public class FieldObjectListAlwaysAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<IReadOnlyList<T>?>, ISupportsValueFormatString
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
