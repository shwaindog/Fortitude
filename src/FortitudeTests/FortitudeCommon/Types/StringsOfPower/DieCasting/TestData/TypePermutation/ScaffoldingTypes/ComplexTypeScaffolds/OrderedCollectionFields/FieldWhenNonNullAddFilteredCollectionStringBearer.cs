using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    OrderedCollectionFields;


[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsStruct)]
public class FieldBoolSpanWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<bool[]?>, ISupportsOrderedCollectionPredicate<bool>
{
    public bool[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public bool[]? Value { get; set; }

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolSpan.AsSpan()
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class FieldNullableBoolSpanWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<bool?[]?>, ISupportsOrderedCollectionPredicate<bool?>
{
    public bool?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public bool?[]? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolSpan.AsSpan()
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableSpanWhenNonNullAddFilteredStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt[]?>
  , ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public TFmt[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmt[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassSpanWhenNonNullAddFilteredStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt?[]?>
  , ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString where TFmt : class, ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmt?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableNullableSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableNullableSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableSpanWhenNonNullAddFilteredStringBearer<TFmtStruct>
    : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerSpanWhenNonNullAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase> : IStringBearer,
    IMoldSupportedValue<TCloaked[]?>, ISupportsValueRevealer<TCloakedRevealBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerSpan
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
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerSpan.AsSpan()
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsNullableClass | AcceptsSpanFormattable
                | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer)]
public class FieldCloakedBearerNullableClassSpanWhenNonNullAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase> : IStringBearer
  , IMoldSupportedValue<TCloaked?[]?>, ISupportsValueRevealer<TCloakedRevealBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : class, TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerNullableSpan
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
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerNullableSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerNullableSpan.AsSpan()
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerSpanWhenNonNullAddFilteredStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<TCloakedStruct?[]?>
  , ISupportsValueRevealer<TCloakedStruct>, ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerSpan
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
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerSpan.AsSpan()
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class FieldStringBearerSpanWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>
    : IStringBearer, IMoldSupportedValue<TBearer[]?>, ISupportsOrderedCollectionPredicate<TBearerBase>
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerSpan
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
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerSpan.AsSpan(), ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerNullableClassSpanWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>
    : IStringBearer, IMoldSupportedValue<TBearer?[]?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerNullableSpan
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
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerNullableSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerNullableSpan.AsSpan()
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerSpanWhenNonNullAddFilteredStringBearer<TBearerStruct>
    : IStringBearer, IMoldSupportedValue<TBearerStruct?[]?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerSpan
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
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerSpan.AsSpan()
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringSpanWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<string[]?>, ISupportsOrderedCollectionPredicate<string?>
  , ISupportsValueFormatString
{
    public string[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public string[]? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringNullableSpanWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<string?[]?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public string?[]? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenNonNullAddFilteredNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceSpanWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<TCharSeq[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq[]? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeq
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceNullableSpanWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<TCharSeq?[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq?[]? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeqNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCharSequenceSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCharSequenceSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderSpanWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder[]?>
  , ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderNullableSpanWhenNonNullAddFilteredStringBearer
    : IStringBearer, IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder?[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBuilderSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBuilderSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldMatchSpanWhenNonNullAddFilteredStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<T[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
    where T : TFilterBase
{
    public T[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public T[]? Value { get; set; }

    public OrderedCollectionPredicate<TFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredMatch
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchNullableSpanWhenNonNullAddFilteredStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<T?[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
    where T : TFilterBase
{
    public T?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableMatchSpan
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
           .WhenNonNullAddFilteredMatchNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableMatchSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableMatchSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsClass | SupportsValueFormatString)]
public class FieldObjectSpanWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<object[]?>, ISupportsOrderedCollectionPredicate<object>
  , ISupportsValueFormatString
{
    public object[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public object[]? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObject
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectSpan.AsSpan()
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldNullableObjectSpanWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<object?[]?>
  , ISupportsOrderedCollectionPredicate<object?>, ISupportsValueFormatString
{
    public object?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public object?[]? Value { get; set; }

    public OrderedCollectionPredicate<object?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObjectNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableObjectSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableObjectSpan.AsSpan()
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsStruct)]
public class FieldBoolReadOnlySpanWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<bool[]?>, ISupportsOrderedCollectionPredicate<bool>
{
    public bool[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public bool[]? Value { get; set; }

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolReadOnlySpan)
              , (ReadOnlySpan<bool>)ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolReadOnlySpan
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class FieldNullableBoolReadOnlySpanWhenNonNullAddFilteredStringBearer
    : IStringBearer, IMoldSupportedValue<bool?[]?>, ISupportsOrderedCollectionPredicate<bool?>
{
    public bool?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public bool?[]? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolReadOnlySpan)
              , (ReadOnlySpan<bool?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolReadOnlySpan
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableReadOnlySpanWhenNonNullAddFilteredStringBearer<TFmt>
    : IStringBearer, IMoldSupportedValue<TFmt[]?>, ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString
    where TFmt : struct, ISpanFormattable
{
    public TFmt[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmt[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmt>)ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassReadOnlySpanWhenNonNullAddFilteredStringBearer<TFmt>
    : IStringBearer, IMoldSupportedValue<TFmt?[]?>, ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString
    where TFmt : class, ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmt?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableNullableReadOnlySpan)
              , (ReadOnlySpan<TFmt?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableNullableReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableReadOnlySpanWhenNonNullAddFilteredStringBearer<TFmtStruct>
    : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmtStruct?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerReadOnlySpanWhenNonNullAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IStringBearer, IMoldSupportedValue<TCloaked[]?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerReadOnlySpan
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
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloaked>)ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerReadOnlySpan
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerNullableClassReadOnlySpanWhenNonNullAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IStringBearer, IMoldSupportedValue<TCloaked?[]?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : class, TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerNullableReadOnlySpan
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
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerNullableReadOnlySpan)
              , (ReadOnlySpan<TCloaked?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerNullableReadOnlySpan
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerReadOnlySpanWhenNonNullAddFilteredStringBearer<TCloakedStruct>
    : IStringBearer, IMoldSupportedValue<TCloakedStruct?[]?>, ISupportsValueRevealer<TCloakedStruct>
      , ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerReadOnlySpan
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
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloakedStruct?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerReadOnlySpan
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class FieldStringBearerReadOnlySpanWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>
    : IStringBearer, IMoldSupportedValue<TBearer[]?>, ISupportsOrderedCollectionPredicate<TBearerBase>
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerReadOnlySpan
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
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearer>)ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerReadOnlySpan
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerNullableClassReadOnlySpanWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>
    : IStringBearer, IMoldSupportedValue<TBearer?[]?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerNullableReadOnlySpan
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
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerNullableReadOnlySpan)
              , (ReadOnlySpan<TBearer?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerNullableReadOnlySpan
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerReadOnlySpanWhenNonNullAddFilteredStringBearer<TBearerStruct>
    : IStringBearer, IMoldSupportedValue<TBearerStruct?[]?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerReadOnlySpan
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
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerReadOnlySpan),
                (ReadOnlySpan<TBearerStruct?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerReadOnlySpan
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringReadOnlySpanWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<string[]?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public string[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string[]? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringReadOnlySpan)
              , (ReadOnlySpan<string>)ComplexTypeCollectionFieldWhenNonNullAddFilteredStringReadOnlySpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringNullableReadOnlySpanWhenNonNullAddFilteredStringBearer
    : IStringBearer, IMoldSupportedValue<string?[]?>, ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string?[]? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringReadOnlySpan),
                (ReadOnlySpan<string?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceReadOnlySpanWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<TCharSeq[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq[]? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeq
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceReadOnlySpan)
              , (ReadOnlySpan<TCharSeq>)ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceNullableReadOnlySpanWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<TCharSeq?[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq?[]? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeqNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCharSequenceReadOnlySpan),
                (ReadOnlySpan<TCharSeq?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCharSequenceReadOnlySpan
              , ElementPredicate)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderReadOnlySpanWhenNonNullAddFilteredStringBearer
    : IStringBearer, IMoldSupportedValue<StringBuilder[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderReadOnlySpan)
              , (ReadOnlySpan<StringBuilder>)ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderNullableReadOnlySpanWhenNonNullAddFilteredStringBearer
    : IStringBearer, IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder?[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBuilderReadOnlySpan)
              , (ReadOnlySpan<StringBuilder?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBuilderReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldMatchReadOnlySpanWhenNonNullAddFilteredStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<T[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
    where T : TFilterBase
{
    public T[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public T[]? Value { get; set; }

    public OrderedCollectionPredicate<TFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredMatch
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchReadOnlySpan)
              , (ReadOnlySpan<T>)ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchNullableReadOnlySpanWhenNonNullAddFilteredStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<T?[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
    where T : TFilterBase
{
    public T?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public T?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredMatchNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableMatchReadOnlySpan)
              , (ReadOnlySpan<T?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableMatchReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsClass | SupportsValueFormatString)]
public class FieldObjectReadOnlySpanWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<object[]?>
  , ISupportsOrderedCollectionPredicate<object>, ISupportsValueFormatString
{
    public object[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public object[]? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObject
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectReadOnlySpan)
              , (ReadOnlySpan<object>)ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectReadOnlySpan
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldNullableObjectReadOnlySpanWhenNonNullAddFilteredStringBearer
    : IStringBearer, IMoldSupportedValue<object?[]?>, ISupportsOrderedCollectionPredicate<object?>, ISupportsValueFormatString
{
    public object?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public object?[]? Value { get; set; }

    public OrderedCollectionPredicate<object?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObjectNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableObjectReadOnlySpan)
              , (ReadOnlySpan<object?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableObjectReadOnlySpan
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsStruct)]
public class FieldBoolArrayWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<bool[]?>, ISupportsOrderedCollectionPredicate<bool>
{
    public bool[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public bool[]? Value { get; set; }

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolArray, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class FieldNullableBoolArrayWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<bool?[]?>, ISupportsOrderedCollectionPredicate<bool?>
{
    public bool?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public bool?[]? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolArray, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableArrayWhenNonNullAddFilteredStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt?[]?>
  , ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public TFmt?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableArray
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableArrayWhenNonNullAddFilteredStringBearer<TFmtStruct>
    : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public TFmtStruct?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableArray
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerArrayWhenNonNullAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IStringBearer, IMoldSupportedValue<TCloaked[]?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase> where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerArray
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
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerArray, ElementPredicate
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerArrayWhenNonNullAddFilteredStringBearer<TCloakedStruct>
    : IStringBearer, IMoldSupportedValue<TCloakedStruct?[]?>, ISupportsValueRevealer<TCloakedStruct>
      , ISupportsOrderedCollectionPredicate<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerArray
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
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerArray
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass |
                  AcceptsStringBearer)]
public class FieldStringBearerArrayWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>
    : IStringBearer, IMoldSupportedValue<TBearer?[]?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerArray
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
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerArray, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerArrayWhenNonNullAddFilteredStringBearer<TBearerStruct>
    : IStringBearer, IMoldSupportedValue<TBearerStruct?[]?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerArray
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
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerArray
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringArrayWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<string?[]?>, ISupportsOrderedCollectionPredicate<string?>
  , ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringArray
    {
        get => Value;
        set => Value = value;
    }

    public string?[]? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringArray
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceArrayWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<TCharSeq?[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceArray
    {
        get => Value;
        set => Value = value;
    }

    public TCharSeq?[]? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeq
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceArray
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderArrayWhenNonNullAddFilteredStringBearer
    : IStringBearer, IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderArray
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
           .WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderArray
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchArrayWhenNonNullAddFilteredStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<T[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
    where T : TFilterBase
{
    public T[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchArray
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
           .WhenNonNullAddFilteredMatch
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchArray
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsAny)]
public class FieldObjectArrayWhenNonNullAddFilteredStringBearer
    : IStringBearer, IMoldSupportedValue<object?[]?>, ISupportsOrderedCollectionPredicate<object>
{
    public object?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectArray
    {
        get => Value;
        set => Value = value;
    }

    public object?[]? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObject
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectArray, ElementPredicate);
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStruct)]
public class FieldBoolListWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<bool>>
  , ISupportsOrderedCollectionPredicate<bool>
{
    public IReadOnlyList<bool>? ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList
    {
        get => Value;
        set => Value = value!;
    }

    public IReadOnlyList<bool> Value { get; set; } = null!;

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class FieldNullableBoolListWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<bool?>?>
  , ISupportsOrderedCollectionPredicate<bool?>
{
    public IReadOnlyList<bool?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<bool?>? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableListWhenNonNullAddFilteredStringBearer<TFmt>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TFmt?>?>, ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString
    where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TFmt?>? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableListWhenNonNullAddFilteredStringBearer<TFmtStruct>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TFmtStruct?>?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<TFmtStruct?>? Value { get; set; }

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList
              , ElementPredicate)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerListWhenNonNullAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TCloaked>?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase> where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public IReadOnlyList<TCloaked>? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList
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
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList, ElementPredicate
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable
                | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer)]
public class FieldNullableCloakedBearerListWhenNonNullAddFilteredStringBearer<TCloakedStruct> : IStringBearer
  , IMoldSupportedValue<IReadOnlyList<TCloakedStruct?>?>, ISupportsValueRevealer<TCloakedStruct>
  , ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList
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
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerListWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TBearer?>?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : IStringBearer, TBearerBase
{
    public IReadOnlyList<TBearer?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList
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
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsChars)]
public class FieldNullableStringBearerListWhenNonNullAddFilteredStringBearer<TBearerStruct>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TBearerStruct?>?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList
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
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringListWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<string?>?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public IReadOnlyList<string?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<string?>? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceListWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<List<TCharSeq?>?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase>
      , ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public List<TCharSeq?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public List<TCharSeq?>? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeq
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderListWhenNonNullAddFilteredStringBearer
    : IStringBearer, IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public StringBuilder?[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchListWhenNonNullAddFilteredStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<T>?>, ISupportsOrderedCollectionPredicate<TFilterBase?>, ISupportsValueFormatString
    where T : TFilterBase
{
    public IReadOnlyList<T>? ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<T>? Value { get; set; }

    public OrderedCollectionPredicate<TFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredMatch
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldObjectListWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<object?>?>
  , ISupportsOrderedCollectionPredicate<object>, ISupportsValueFormatString
{
    public IReadOnlyList<object?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<object?>? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObject
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }
}
