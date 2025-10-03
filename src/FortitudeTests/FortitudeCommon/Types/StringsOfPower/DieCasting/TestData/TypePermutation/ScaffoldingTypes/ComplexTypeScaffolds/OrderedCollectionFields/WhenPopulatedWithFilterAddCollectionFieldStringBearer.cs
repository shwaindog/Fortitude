using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.TypeGeneratePartFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.OrderedCollectionFields;


[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsStruct)]
public class BoolSpanWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<bool[]?>, ISupportsOrderedCollectionPredicate<bool>
{
    public bool[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public bool[]? Value { get; set; }

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolSpan.AsSpan()
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class NullableBoolSpanWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<bool?[]?>, ISupportsOrderedCollectionPredicate<bool?>
{
    public bool?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public bool?[]? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolSpan.AsSpan()
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableSpanWhenPopulatedWithFilterStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt[]?>
  , ISupportsOrderedCollectionPredicate<TFmt>, ISupportsSingleFormatString where TFmt : ISpanFormattable
{
    public TFmt[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmt[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableSpan.AsSpan()
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableNullableSpanWhenPopulatedWithFilterStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt?[]?>
  , ISupportsOrderedCollectionPredicate<TFmt>, ISupportsSingleFormatString where TFmt : class, ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmt?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableSpan.AsSpan()
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class NullableSpanFormattableSpanWhenPopulatedWithFilterStringBearer<TFmtStruct>
    : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsSingleFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableSpan.AsSpan()
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class CloakedBearerSpanWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase> : IStringBearer,
    IMoldSupportedValue<TCloaked[]?>, ISupportsSingleRevealer<TCloakedRevealBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked[]? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> PalantirRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerSpan.AsSpan()
              , ElementPredicate, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsNullableClass | AcceptsSpanFormattable
                | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | OnePalantirRevealer)]
public class CloakedBearerNullableSpanWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase> : IStringBearer
  , IMoldSupportedValue<TCloaked?[]?>, ISupportsSingleRevealer<TCloakedRevealBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : class, TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked?[]? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> PalantirRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableSpan.AsSpan()
              , ElementPredicate, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class NullableCloakedBearerSpanWhenPopulatedWithFilterStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<TCloakedStruct?[]?>
  , ISupportsSingleRevealer<TCloakedStruct>, ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloakedStruct?[]? Value { get; set; }

    public PalantírReveal<TCloakedStruct> PalantirRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerSpan.AsSpan()
              , ElementPredicate, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class StringBearerSpanWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : IStringBearer, IMoldSupportedValue<TBearer[]?>, ISupportsOrderedCollectionPredicate<TBearerBase>
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerSpan
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
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerSpan.AsSpan(), ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerNullableSpanWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : IStringBearer, IMoldSupportedValue<TBearer?[]?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerNullableSpan
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
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerNullableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerNullableSpan.AsSpan()
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerSpanWhenPopulatedWithFilterStringBearer<TBearerStruct>
    : IStringBearer, IMoldSupportedValue<TBearerStruct?[]?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerSpan
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
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerSpan.AsSpan()
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsChars | OneFormatString)]
public class StringSpanWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<string[]?>, ISupportsOrderedCollectionPredicate<string?>
  , ISupportsSingleFormatString
{
    public string[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public string[]? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringSpan.AsSpan()
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsChars | OneFormatString)]
public class StringNullableSpanWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<string?[]?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsSingleFormatString
{
    public string?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public string?[]? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenPopulatedWithFilterNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringSpan.AsSpan()
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsChars | OneFormatString)]
public class CharSequenceSpanWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<TCharSeq[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsSingleFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq[]? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceSpan.AsSpan()
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsChars | OneFormatString)]
public class CharSequenceNullableSpanWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<TCharSeq?[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsSingleFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq?[]? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeqNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCharSequenceSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCharSequenceSpan.AsSpan()
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsChars | OneFormatString)]
public class StringBuilderSpanWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder[]?>
  , ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsSingleFormatString
{
    public StringBuilder[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderSpan.AsSpan()
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsChars | OneFormatString)]
public class StringBuilderNullableSpanWhenPopulatedWithFilterStringBearer
    : IStringBearer, IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsSingleFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder?[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderSpan.AsSpan()
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsChars | OneFormatString)]
public class MatchSpanWhenPopulatedWithFilterStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<T[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsSingleFormatString
    where T : TFilterBase
{
    public T[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public T[]? Value { get; set; }

    public OrderedCollectionPredicate<TFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchSpan.AsSpan()
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsAny | OneFormatString)]
public class MatchNullableSpanWhenPopulatedWithFilterStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<T?[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsSingleFormatString
    where T : TFilterBase
{
    public T?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableMatchSpan
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
           .WhenPopulatedWithFilterMatchNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableMatchSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableMatchSpan.AsSpan()
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsStruct | AcceptsClass | OneFormatString)]
public class ObjectSpanWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<object[]?>, ISupportsOrderedCollectionPredicate<object>
  , ISupportsSingleFormatString
{
    public object[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public object[]? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectSpan.AsSpan()
              , ElementPredicate, FormatString);

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsNullableClass | OneFormatString)]
public class NullableObjectSpanWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<object?[]?>
  , ISupportsOrderedCollectionPredicate<object?>, ISupportsSingleFormatString
{
    public object?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public object?[]? Value { get; set; }

    public OrderedCollectionPredicate<object?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObjectNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectSpan.AsSpan()
              , ElementPredicate, FormatString);

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsStruct)]
public class BoolReadOnlySpanWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<bool[]?>, ISupportsOrderedCollectionPredicate<bool>
{
    public bool[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public bool[]? Value { get; set; }

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolReadOnlySpan)
              , (ReadOnlySpan<bool>)ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolReadOnlySpan
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class NullableBoolReadOnlySpanSpanWhenPopulatedWithFilterStringBearer
    : IStringBearer, IMoldSupportedValue<bool?[]?>, ISupportsOrderedCollectionPredicate<bool?>
{
    public bool?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public bool?[]? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolReadOnlySpan)
              , (ReadOnlySpan<bool?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolReadOnlySpan
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableReadOnlySpanSpanWhenPopulatedWithFilterStringBearer<TFmt>
    : IStringBearer, IMoldSupportedValue<TFmt[]?>, ISupportsOrderedCollectionPredicate<TFmt>, ISupportsSingleFormatString
    where TFmt : struct, ISpanFormattable
{
    public TFmt[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmt[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmt>)ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableReadOnlySpan
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableNullableReadOnlySpanWhenPopulatedWithFilterStringBearer<TFmt>
    : IStringBearer, IMoldSupportedValue<TFmt?[]?>, ISupportsOrderedCollectionPredicate<TFmt>, ISupportsSingleFormatString
    where TFmt : class, ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmt?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableReadOnlySpan)
              , (ReadOnlySpan<TFmt?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableReadOnlySpan
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class NullableSpanFormattableReadOnlySpanWhenPopulatedWithFilterStringBearer<TFmtStruct>
    : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsSingleFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmtStruct?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableReadOnlySpan
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class CloakedBearerReadOnlySpanWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IStringBearer, IMoldSupportedValue<TCloaked[]?>, ISupportsSingleRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked[]? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> PalantirRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloaked>)ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerReadOnlySpan
              , ElementPredicate, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class CustomBearerNullableReadOnlySpanWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IStringBearer, IMoldSupportedValue<TCloaked?[]?>, ISupportsSingleRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : class, TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked?[]? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> PalantirRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableReadOnlySpan)
              , (ReadOnlySpan<TCloaked?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableReadOnlySpan
              , ElementPredicate, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class NullableCustomBearerReadOnlySpanWhenPopulatedWithFilterStringBearer<TCloakedStruct>
    : IStringBearer, IMoldSupportedValue<TCloakedStruct?[]?>, ISupportsSingleRevealer<TCloakedStruct>
      , ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TCloakedStruct?[]? Value { get; set; }

    public PalantírReveal<TCloakedStruct> PalantirRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloakedStruct?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerReadOnlySpan
              , ElementPredicate, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class StringBearerReadOnlySpanWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : IStringBearer, IMoldSupportedValue<TBearer[]?>, ISupportsOrderedCollectionPredicate<TBearerBase>
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerReadOnlySpan
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
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearer>)ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerReadOnlySpan
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerNullableReadOnlySpanWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : IStringBearer, IMoldSupportedValue<TBearer?[]?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerNullableReadOnlySpan
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
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerNullableReadOnlySpan)
              , (ReadOnlySpan<TBearer?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerNullableReadOnlySpan
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerReadOnlySpanWhenPopulatedWithFilterStringBearer<TBearerStruct>
    : IStringBearer, IMoldSupportedValue<TBearerStruct?[]?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerReadOnlySpan
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
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerReadOnlySpan),
                (ReadOnlySpan<TBearerStruct?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerReadOnlySpan
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsChars | OneFormatString)]
public class StringReadOnlySpanWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<string[]?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsSingleFormatString
{
    public string[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string[]? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringReadOnlySpan)
              , (ReadOnlySpan<string>)ComplexTypeCollectionFieldWhenPopulatedWithFilterStringReadOnlySpan.AsSpan()
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsChars | OneFormatString)]
public class StringNullableReadOnlySpanWhenPopulatedWithFilterStringBearer
    : IStringBearer, IMoldSupportedValue<string?[]?>, ISupportsOrderedCollectionPredicate<string?>, ISupportsSingleFormatString
{
    public string?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string?[]? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringReadOnlySpan),
                (ReadOnlySpan<string?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringReadOnlySpan
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsChars | OneFormatString)]
public class CharSequenceReadOnlySpanWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<TCharSeq[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsSingleFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq[]? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceReadOnlySpan)
              , (ReadOnlySpan<TCharSeq>)ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceReadOnlySpan
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsChars | OneFormatString)]
public class CharSequenceNullableReadOnlySpanWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<TCharSeq?[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsSingleFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq?[]? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeqNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCharSequenceReadOnlySpan),
                (ReadOnlySpan<TCharSeq?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCharSequenceReadOnlySpan
              , ElementPredicate)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsChars | OneFormatString)]
public class StringBuilderReadOnlySpanWhenPopulatedWithFilterStringBearer
    : IStringBearer, IMoldSupportedValue<StringBuilder[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsSingleFormatString
{
    public StringBuilder[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderReadOnlySpan)
              , (ReadOnlySpan<StringBuilder>)ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderReadOnlySpan
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsChars | OneFormatString)]
public class StringBuilderNullableReadOnlySpanWhenPopulatedWithFilterStringBearer
    : IStringBearer, IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsSingleFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder?[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderReadOnlySpan)
              , (ReadOnlySpan<StringBuilder?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderReadOnlySpan
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsChars | OneFormatString)]
public class MatchReadOnlySpanWhenPopulatedWithFilterStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<T[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsSingleFormatString
    where T : TFilterBase
{
    public T[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public T[]? Value { get; set; }

    public OrderedCollectionPredicate<TFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchReadOnlySpan)
              , (ReadOnlySpan<T>)ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchReadOnlySpan
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsAny | OneFormatString)]
public class MatchNullableReadOnlySpanWhenPopulatedWithFilterStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<T?[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsSingleFormatString
    where T : TFilterBase
{
    public T?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public T?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterMatchNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableMatchReadOnlySpan)
              , (ReadOnlySpan<T?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableMatchReadOnlySpan
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsStruct | AcceptsClass | OneFormatString)]
public class ObjectReadOnlySpanWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<object[]?>
  , ISupportsOrderedCollectionPredicate<object>, ISupportsSingleFormatString
{
    public object[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public object[]? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectReadOnlySpan)
              , (ReadOnlySpan<object>)ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectReadOnlySpan
              , ElementPredicate, FormatString);

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsNullableClass | OneFormatString)]
public class NullableObjectReadOnlySpanWhenPopulatedWithFilterStringBearer
    : IStringBearer, IMoldSupportedValue<object?[]?>, ISupportsOrderedCollectionPredicate<object?>, ISupportsSingleFormatString
{
    public object?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public object?[]? Value { get; set; }

    public OrderedCollectionPredicate<object?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObjectNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectReadOnlySpan)
              , (ReadOnlySpan<object?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectReadOnlySpan
              , ElementPredicate, FormatString);

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsStruct)]
public class BoolArrayWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<bool[]?>, ISupportsOrderedCollectionPredicate<bool>
{
    public bool[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public bool[]? Value { get; set; }

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolArray, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class NullableBoolArrayWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<bool?[]?>, ISupportsOrderedCollectionPredicate<bool?>
{
    public bool?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public bool?[]? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolArray, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableArrayWhenPopulatedWithFilterStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt?[]?>
  , ISupportsOrderedCollectionPredicate<TFmt>, ISupportsSingleFormatString where TFmt : ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public TFmt?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableArray
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class NullableSpanFormattableArrayWhenPopulatedWithFilterStringBearer<TFmtStruct>
    : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsSingleFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableArray
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class CloakedBearerArrayWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IStringBearer, IMoldSupportedValue<TCloaked[]?>, ISupportsSingleRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase> where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public TCloaked[]? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> PalantirRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerArray, ElementPredicate
              , PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class NullableCloakedBearerArrayWhenPopulatedWithFilterStringBearer<TCloakedStruct>
    : IStringBearer, IMoldSupportedValue<TCloakedStruct?[]?>, ISupportsSingleRevealer<TCloakedStruct>
      , ISupportsOrderedCollectionPredicate<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public TCloakedStruct?[]? Value { get; set; }


    public PalantírReveal<TCloakedStruct> PalantirRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerArray
              , ElementPredicate, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass |
                  AcceptsStringBearer)]
public class StringBearerArrayWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : IStringBearer, IMoldSupportedValue<TBearer?[]?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerArray
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
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerArray, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerArrayWhenPopulatedWithFilterStringBearer<TBearerStruct>
    : IStringBearer, IMoldSupportedValue<TBearerStruct?[]?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerArray
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
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerArray
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsChars | OneFormatString)]
public class StringArrayWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<string?[]?>, ISupportsOrderedCollectionPredicate<string?>
  , ISupportsSingleFormatString
{
    public string?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringArray
    {
        get => Value;
        set => Value = value;
    }

    public string?[]? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringArray
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsChars | OneFormatString)]
public class CharSequenceArrayWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<TCharSeq?[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsSingleFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceArray
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq?[]? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceArray
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsChars | OneFormatString)]
public class StringBuilderArrayWhenPopulatedWithFilterStringBearer
    : IStringBearer, IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsSingleFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderArray
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
           .WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderArray
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsAny | OneFormatString)]
public class MatchArrayWhenPopulatedWithFilterStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<T[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsSingleFormatString
    where T : TFilterBase
{
    public T[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchArray
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
           .WhenPopulatedWithFilterMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchArray
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsAny | OneFormatString)]
public class ObjectArrayWhenPopulatedWithFilterStringBearer
    : IStringBearer, IMoldSupportedValue<object?[]?>, ISupportsOrderedCollectionPredicate<object>
{
    public object?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectArray
    {
        get => Value;
        set => Value = value;
    }

    public object?[]? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectArray, ElementPredicate);
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsStruct)]
public class BoolListWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<bool>>
  , ISupportsOrderedCollectionPredicate<bool>
{
    public IReadOnlyList<bool>? ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolList
    {
        get => Value;
        set => Value = value!;
    }

    public IReadOnlyList<bool> Value { get; set; } = null!;

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class NullableBoolListWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<bool?>?>
  , ISupportsOrderedCollectionPredicate<bool?>
{
    public IReadOnlyList<bool?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<bool?>? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableListWhenPopulatedWithFilterStringBearer<TFmt>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TFmt?>?>, ISupportsOrderedCollectionPredicate<TFmt>, ISupportsSingleFormatString
    where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TFmt?>? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableList
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class NullableSpanFormattableListWhenPopulatedWithFilterStringBearer<TFmtStruct>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TFmtStruct?>?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsSingleFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TFmtStruct?>? Value { get; set; }

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableList
              , ElementPredicate)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class CloakedBearerListWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TCloaked>?>, ISupportsSingleRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase> where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public IReadOnlyList<TCloaked>? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> PalantirRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList, ElementPredicate
              , PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable
                | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | OnePalantirRevealer)]
public class NullableCloakedBearerListWhenPopulatedWithFilterStringBearer<TCloakedStruct> : IStringBearer
  , IMoldSupportedValue<IReadOnlyList<TCloakedStruct?>?>, ISupportsSingleRevealer<TCloakedStruct>
  , ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TCloakedStruct?>? Value { get; set; }


    public PalantírReveal<TCloakedStruct> PalantirRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList
              , ElementPredicate, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerListWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TBearer?>?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : IStringBearer, TBearerBase
{
    public IReadOnlyList<TBearer?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerList
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
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsChars | OneFormatString)]
public class NullableStringBearerListWhenPopulatedWithFilterStringBearer<TBearerStruct>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TBearerStruct?>?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerList
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
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsChars | OneFormatString)]
public class StringListWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<string?>?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsSingleFormatString
{
    public IReadOnlyList<string?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<string?>? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringList
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsChars | OneFormatString)]
public class CharSequenceListWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TCharSeq?>?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>
      , ISupportsSingleFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IReadOnlyList<TCharSeq?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TCharSeq?>? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceList
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsChars | OneFormatString)]
public class StringBuilderListWhenPopulatedWithFilterStringBearer
    : IStringBearer, IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsSingleFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder?[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderList
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsAny | OneFormatString)]
public class MatchListWhenPopulatedWithFilterStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<T>?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsSingleFormatString
    where T : TFilterBase
{
    public IReadOnlyList<T>? ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<T>? Value { get; set; }

    public OrderedCollectionPredicate<TFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchList
              , ElementPredicate, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | CollectionFilterPredicate | AcceptsNullableClass | OneFormatString)]
public class ObjectListWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<object?>?>
  , ISupportsOrderedCollectionPredicate<object>, ISupportsSingleFormatString
{
    public IReadOnlyList<object?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<object?>? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectList
              , ElementPredicate, FormatString);

    public string? FormatString { get; set; }
}
