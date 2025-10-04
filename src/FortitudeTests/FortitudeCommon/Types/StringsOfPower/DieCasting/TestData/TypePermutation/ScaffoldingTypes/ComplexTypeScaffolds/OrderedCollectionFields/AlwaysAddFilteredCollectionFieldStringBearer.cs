using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.TypeGeneratePartFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    OrderedCollectionFields;

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsStruct)]
public class BoolSpanAlwaysAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<bool[]?>, ISupportsOrderedCollectionPredicate<bool>
{
    public bool[]? ComplexTypeCollectionFieldAlwaysAddFilteredBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public bool[]? Value { get; set; }

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredBoolSpan.AsSpan()
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class NullableBoolSpanAlwaysAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<bool?[]?>, ISupportsOrderedCollectionPredicate<bool?>
{
    public bool?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public bool?[]? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolSpan.AsSpan()
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SpanFormattableSpanAlwaysAddFilteredStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt[]?>
  , ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public TFmt[]? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmt[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SpanFormattableNullableSpanAlwaysAddFilteredStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt?[]?>
  , ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString where TFmt : class, ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmt?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableNullableSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableNullableSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class NullableSpanFormattableSpanAlwaysAddFilteredStringBearer<TFmtStruct>
    : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class CloakedBearerSpanAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase> : IStringBearer,
    IMoldSupportedValue<TCloaked[]?>, ISupportsValueRevealer<TCloakedRevealBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked[]? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> ValueRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerSpan.AsSpan()
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsNullableClass | AcceptsSpanFormattable
                | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer)]
public class CloakedBearerNullableSpanAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase> : IStringBearer
  , IMoldSupportedValue<TCloaked?[]?>, ISupportsValueRevealer<TCloakedRevealBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : class, TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked?[]? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked?[]? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> ValueRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableSpan.AsSpan()
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class NullableCloakedBearerSpanAlwaysAddFilteredStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<TCloakedStruct?[]?>
  , ISupportsValueRevealer<TCloakedStruct>, ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloakedStruct?[]? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerSpan.AsSpan()
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class StringBearerSpanAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : IStringBearer, IMoldSupportedValue<TBearer[]?>, ISupportsOrderedCollectionPredicate<TBearerBase>
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public TBearer[]? Value { get; set; }

    public OrderedCollectionPredicate<TBearerBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerSpan.AsSpan(), ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerNullableSpanAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : IStringBearer, IMoldSupportedValue<TBearer?[]?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TBearer?[]? Value { get; set; }

    public OrderedCollectionPredicate<TBearerBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerNullableSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerNullableSpan.AsSpan()
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerSpanAlwaysAddFilteredStringBearer<TBearerStruct>
    : IStringBearer, IMoldSupportedValue<TBearerStruct?[]?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public TBearerStruct?[]? Value { get; set; }

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerSpan.AsSpan()
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class StringSpanAlwaysAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<string[]?>, ISupportsOrderedCollectionPredicate<string?>
  , ISupportsValueFormatString
{
    public string[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public string[]? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class StringNullableSpanAlwaysAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<string?[]?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public string?[]? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .AlwaysAddFilteredNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class CharSequenceSpanAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<TCharSeq[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq[]? ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq[]? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeq
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class CharSequenceNullableSpanAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<TCharSeq?[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq?[]? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeqNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCharSequenceSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableCharSequenceSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class StringBuilderSpanAlwaysAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder[]?>
  , ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class StringBuilderNullableSpanAlwaysAddFilteredStringBearer
    : IStringBearer, IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder?[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBuilderSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBuilderSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class MatchSpanAlwaysAddFilteredStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<T[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
    where T : TFilterBase
{
    public T[]? ComplexTypeCollectionFieldAlwaysAddFilteredMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public T[]? Value { get; set; }

    public OrderedCollectionPredicate<TFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatch
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredMatchSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class MatchNullableSpanAlwaysAddFilteredStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<T?[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
    where T : TFilterBase
{
    public T?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public T?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .AlwaysAddFilteredMatchNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableMatchSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableMatchSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass | SupportsValueFormatString)]
public class ObjectSpanAlwaysAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<object[]?>, ISupportsOrderedCollectionPredicate<object>
  , ISupportsValueFormatString
{
    public object[]? ComplexTypeCollectionFieldAlwaysAddFilteredObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public object[]? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObject
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredObjectSpan.AsSpan()
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsNullableClass | SupportsValueFormatString)]
public class NullableObjectSpanAlwaysAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<object?[]?>
  , ISupportsOrderedCollectionPredicate<object?>, ISupportsValueFormatString
{
    public object?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public object?[]? Value { get; set; }

    public OrderedCollectionPredicate<object?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObjectNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableObjectSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableObjectSpan.AsSpan()
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsStruct)]
public class BoolReadOnlySpanAlwaysAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<bool[]?>, ISupportsOrderedCollectionPredicate<bool>
{
    public bool[]? ComplexTypeCollectionFieldAlwaysAddFilteredBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public bool[]? Value { get; set; }

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolReadOnlySpan)
              , (ReadOnlySpan<bool>)ComplexTypeCollectionFieldAlwaysAddFilteredBoolReadOnlySpan
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class NullableBoolReadOnlySpanSpanAlwaysAddFilteredStringBearer
    : IStringBearer, IMoldSupportedValue<bool?[]?>, ISupportsOrderedCollectionPredicate<bool?>
{
    public bool?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public bool?[]? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolReadOnlySpan)
              , (ReadOnlySpan<bool?>)ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolReadOnlySpan
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SpanFormattableReadOnlySpanSpanAlwaysAddFilteredStringBearer<TFmt>
    : IStringBearer, IMoldSupportedValue<TFmt[]?>, ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString
    where TFmt : struct, ISpanFormattable
{
    public TFmt[]? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmt[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmt>)ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SpanFormattableNullableReadOnlySpanAlwaysAddFilteredStringBearer<TFmt>
    : IStringBearer, IMoldSupportedValue<TFmt?[]?>, ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString
    where TFmt : class, ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmt?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableNullableReadOnlySpan)
              , (ReadOnlySpan<TFmt?>)ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableNullableReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class NullableSpanFormattableReadOnlySpanAlwaysAddFilteredStringBearer<TFmtStruct>
    : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmtStruct?>)ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class CloakedBearerReadOnlySpanAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IStringBearer, IMoldSupportedValue<TCloaked[]?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked[]? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> ValueRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloaked>)ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerReadOnlySpan
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class CustomBearerNullableReadOnlySpanAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IStringBearer, IMoldSupportedValue<TCloaked?[]?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : class, TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked?[]? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked?[]? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> ValueRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableReadOnlySpan)
              , (ReadOnlySpan<TCloaked?>)ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableReadOnlySpan
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class NullableCustomBearerReadOnlySpanAlwaysAddFilteredStringBearer<TCloakedStruct>
    : IStringBearer, IMoldSupportedValue<TCloakedStruct?[]?>, ISupportsValueRevealer<TCloakedStruct>
      , ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloakedStruct?[]? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloakedStruct?>)ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerReadOnlySpan
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class StringBearerReadOnlySpanAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : IStringBearer, IMoldSupportedValue<TBearer[]?>, ISupportsOrderedCollectionPredicate<TBearerBase>
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TBearer[]? Value { get; set; }

    public OrderedCollectionPredicate<TBearerBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearer>)ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerReadOnlySpan
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerNullableReadOnlySpanAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : IStringBearer, IMoldSupportedValue<TBearer?[]?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TBearer?[]? Value { get; set; }

    public OrderedCollectionPredicate<TBearerBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerNullableReadOnlySpan)
              , (ReadOnlySpan<TBearer?>)ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerNullableReadOnlySpan
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerReadOnlySpanAlwaysAddFilteredStringBearer<TBearerStruct>
    : IStringBearer, IMoldSupportedValue<TBearerStruct?[]?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TBearerStruct?[]? Value { get; set; }

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerReadOnlySpan),
                (ReadOnlySpan<TBearerStruct?>)ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerReadOnlySpan
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class StringReadOnlySpanAlwaysAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<string[]?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public string[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string[]? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringReadOnlySpan)
              , (ReadOnlySpan<string>)ComplexTypeCollectionFieldAlwaysAddFilteredStringReadOnlySpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class StringNullableReadOnlySpanAlwaysAddFilteredStringBearer
    : IStringBearer, IMoldSupportedValue<string?[]?>, ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string?[]? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringReadOnlySpan),
                (ReadOnlySpan<string?>)ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class CharSequenceReadOnlySpanAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<TCharSeq[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq[]? ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq[]? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeq
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceReadOnlySpan)
              , (ReadOnlySpan<TCharSeq>)ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class CharSequenceNullableReadOnlySpanAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<TCharSeq?[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq?[]? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeqNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCharSequenceReadOnlySpan),
                (ReadOnlySpan<TCharSeq?>)ComplexTypeCollectionFieldAlwaysAddFilteredNullableCharSequenceReadOnlySpan
              , ElementPredicate)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class StringBuilderReadOnlySpanAlwaysAddFilteredStringBearer
    : IStringBearer, IMoldSupportedValue<StringBuilder[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderReadOnlySpan)
              , (ReadOnlySpan<StringBuilder>)ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class StringBuilderNullableReadOnlySpanAlwaysAddFilteredStringBearer
    : IStringBearer, IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder?[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBuilderReadOnlySpan)
              , (ReadOnlySpan<StringBuilder?>)ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBuilderReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class MatchReadOnlySpanAlwaysAddFilteredStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<T[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
    where T : TFilterBase
{
    public T[]? ComplexTypeCollectionFieldAlwaysAddFilteredMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public T[]? Value { get; set; }

    public OrderedCollectionPredicate<TFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatch
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchReadOnlySpan)
              , (ReadOnlySpan<T>)ComplexTypeCollectionFieldAlwaysAddFilteredMatchReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class MatchNullableReadOnlySpanAlwaysAddFilteredStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<T?[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
    where T : TFilterBase
{
    public T?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public T?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatchNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableMatchReadOnlySpan)
              , (ReadOnlySpan<T?>)ComplexTypeCollectionFieldAlwaysAddFilteredNullableMatchReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass | SupportsValueFormatString)]
public class ObjectReadOnlySpanAlwaysAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<object[]?>
  , ISupportsOrderedCollectionPredicate<object>, ISupportsValueFormatString
{
    public object[]? ComplexTypeCollectionFieldAlwaysAddFilteredObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public object[]? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObject
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectReadOnlySpan)
              , (ReadOnlySpan<object>)ComplexTypeCollectionFieldAlwaysAddFilteredObjectReadOnlySpan
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsNullableClass | SupportsValueFormatString)]
public class NullableObjectReadOnlySpanAlwaysAddFilteredStringBearer
    : IStringBearer, IMoldSupportedValue<object?[]?>, ISupportsOrderedCollectionPredicate<object?>, ISupportsValueFormatString
{
    public object?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public object?[]? Value { get; set; }

    public OrderedCollectionPredicate<object?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObjectNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableObjectReadOnlySpan)
              , (ReadOnlySpan<object?>)ComplexTypeCollectionFieldAlwaysAddFilteredNullableObjectReadOnlySpan
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsStruct)]
public class BoolArrayAlwaysAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<bool[]?>, ISupportsOrderedCollectionPredicate<bool>
{
    public bool[]? ComplexTypeCollectionFieldAlwaysAddFilteredBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public bool[]? Value { get; set; }

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredBoolArray, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class NullableBoolArrayAlwaysAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<bool?[]?>, ISupportsOrderedCollectionPredicate<bool?>
{
    public bool?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public bool?[]? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolArray, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SpanFormattableArrayAlwaysAddFilteredStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt?[]?>
  , ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public TFmt?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableArray
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class NullableSpanFormattableArrayAlwaysAddFilteredStringBearer<TFmtStruct>
    : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableArray
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class CloakedBearerArrayAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IStringBearer, IMoldSupportedValue<TCloaked[]?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase> where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked[]? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> ValueRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerArray, ElementPredicate
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class NullableCloakedBearerArrayAlwaysAddFilteredStringBearer<TCloakedStruct>
    : IStringBearer, IMoldSupportedValue<TCloakedStruct?[]?>, ISupportsValueRevealer<TCloakedStruct>
      , ISupportsOrderedCollectionPredicate<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public TCloakedStruct?[]? Value { get; set; }


    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerArray
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass |
                  AcceptsStringBearer)]
public class StringBearerArrayAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : IStringBearer, IMoldSupportedValue<TBearer?[]?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public TBearer?[]? Value { get; set; }

    public OrderedCollectionPredicate<TBearerBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerArray, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerArrayAlwaysAddFilteredStringBearer<TBearerStruct>
    : IStringBearer, IMoldSupportedValue<TBearerStruct?[]?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public TBearerStruct?[]? Value { get; set; }

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerArray
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class StringArrayAlwaysAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<string?[]?>, ISupportsOrderedCollectionPredicate<string?>
  , ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringArray
    {
        get => Value;
        set => Value = value;
    }

    public string?[]? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringArray
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class CharSequenceArrayAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<TCharSeq?[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceArray
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq?[]? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeq
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceArray
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class StringBuilderArrayAlwaysAddFilteredStringBearer
    : IStringBearer, IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderArray
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder?[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderArray
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class MatchArrayAlwaysAddFilteredStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<T[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
    where T : TFilterBase
{
    public T[]? ComplexTypeCollectionFieldAlwaysAddFilteredMatchArray
    {
        get => Value;
        set => Value = value;
    }

    public T[]? Value { get; set; }

    public OrderedCollectionPredicate<TFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .AlwaysAddFilteredMatch
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredMatchArray
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class ObjectArrayAlwaysAddFilteredStringBearer
    : IStringBearer, IMoldSupportedValue<object?[]?>, ISupportsOrderedCollectionPredicate<object>
{
    public object?[]? ComplexTypeCollectionFieldAlwaysAddFilteredObjectArray
    {
        get => Value;
        set => Value = value;
    }

    public object?[]? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObject
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredObjectArray, ElementPredicate);
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsStruct)]
public class BoolListAlwaysAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<bool>>
  , ISupportsOrderedCollectionPredicate<bool>
{
    public IReadOnlyList<bool>? ComplexTypeCollectionFieldAlwaysAddFilteredBoolList
    {
        get => Value;
        set => Value = value!;
    }

    public IReadOnlyList<bool> Value { get; set; } = null!;

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredBoolList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class NullableBoolListAlwaysAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<bool?>?>
  , ISupportsOrderedCollectionPredicate<bool?>
{
    public IReadOnlyList<bool?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<bool?>? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class SpanFormattableListAlwaysAddFilteredStringBearer<TFmt>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TFmt?>?>, ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString
    where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt?>? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TFmt?>? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class NullableSpanFormattableListAlwaysAddFilteredStringBearer<TFmtStruct>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TFmtStruct?>?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TFmtStruct?>? Value { get; set; }

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableList
              , ElementPredicate)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class CloakedBearerListAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TCloaked>?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase> where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public IReadOnlyList<TCloaked>? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> ValueRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerList, ElementPredicate
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable
                | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer)]
public class NullableCloakedBearerListAlwaysAddFilteredStringBearer<TCloakedStruct> : IStringBearer
  , IMoldSupportedValue<IReadOnlyList<TCloakedStruct?>?>, ISupportsValueRevealer<TCloakedStruct>
  , ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TCloakedStruct?>? Value { get; set; }


    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerList
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerListAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TBearer?>?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : IStringBearer, TBearerBase
{
    public IReadOnlyList<TBearer?>? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TBearer?>? Value { get; set; }

    public OrderedCollectionPredicate<TBearerBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class NullableStringBearerListAlwaysAddFilteredStringBearer<TBearerStruct>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TBearerStruct?>?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TBearerStruct?>? Value { get; set; }

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class StringListAlwaysAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<string?>?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public IReadOnlyList<string?>? ComplexTypeCollectionFieldAlwaysAddFilteredStringList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<string?>? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class CharSequenceListAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TCharSeq?>?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>
      , ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IReadOnlyList<TCharSeq?>? ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TCharSeq?>? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeq
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class StringBuilderListAlwaysAddFilteredStringBearer
    : IStringBearer, IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder?[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class MatchListAlwaysAddFilteredStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<T>?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
    where T : TFilterBase
{
    public IReadOnlyList<T>? ComplexTypeCollectionFieldAlwaysAddFilteredMatchList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<T>? Value { get; set; }

    public OrderedCollectionPredicate<TFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatch
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredMatchList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | FilterPredicate | AcceptsNullableClass | SupportsValueFormatString)]
public class ObjectListAlwaysAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<object?>?>
  , ISupportsOrderedCollectionPredicate<object>, ISupportsValueFormatString
{
    public IReadOnlyList<object?>? ComplexTypeCollectionFieldAlwaysAddFilteredObjectList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<object?>? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObject
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredObjectList
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }
}
