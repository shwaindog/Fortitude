﻿using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    OrderedCollectionFields;


[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsStruct)]
public class FieldBoolSpanWhenNonNullAddFilteredStringBearer : IMoldSupportedValue<bool[]?>, ISupportsOrderedCollectionPredicate<bool>
{
    public bool[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolSpan);
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
public class FieldNullableBoolSpanWhenNonNullAddFilteredStringBearer : IMoldSupportedValue<bool?[]?>, ISupportsOrderedCollectionPredicate<bool?>
{
    public bool?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolSpan);
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
public class FieldSpanFormattableSpanWhenNonNullAddFilteredStringBearer<TFmt> : IMoldSupportedValue<TFmt[]?>
  , ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public TFmt[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableSpan);
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
public class FieldSpanFormattableNullableClassSpanWhenNonNullAddFilteredStringBearer<TFmt> : IMoldSupportedValue<TFmt?[]?>
  , ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString where TFmt : class, ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableNullableSpan);
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
    : IMoldSupportedValue<TFmtStruct?[]?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableSpan);
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
public class FieldCloakedBearerSpanWhenNonNullAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase> : 
    IMoldSupportedValue<TCloaked[]?>, ISupportsValueRevealer<TCloakedRevealBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerSpan);
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
public class FieldCloakedBearerNullableClassSpanWhenNonNullAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase> : 
  IMoldSupportedValue<TCloaked?[]?>, ISupportsValueRevealer<TCloakedRevealBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : class, TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerNullableSpan);
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
public class FieldNullableCloakedBearerSpanWhenNonNullAddFilteredStringBearer<TCloakedStruct> : IMoldSupportedValue<TCloakedStruct?[]?>
  , ISupportsValueRevealer<TCloakedStruct>, ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerSpan);
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
    : IMoldSupportedValue<TBearer[]?>, ISupportsOrderedCollectionPredicate<TBearerBase>
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerSpan);
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
    : IMoldSupportedValue<TBearer?[]?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerNullableSpan);
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
    : IMoldSupportedValue<TBearerStruct?[]?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerSpan);
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
public class FieldStringSpanWhenNonNullAddFilteredStringBearer : IMoldSupportedValue<string[]?>, ISupportsOrderedCollectionPredicate<string?>
  , ISupportsValueFormatString
{
    public string[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringSpan);
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
public class FieldStringNullableSpanWhenNonNullAddFilteredStringBearer : IMoldSupportedValue<string?[]?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringSpan);
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
    : IMoldSupportedValue<TCharSeq[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceSpan);
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
    : IMoldSupportedValue<TCharSeq?[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCharSequenceSpan);
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
public class FieldStringBuilderSpanWhenNonNullAddFilteredStringBearer : IMoldSupportedValue<StringBuilder[]?>
  , ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderSpan);
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
    : IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBuilderSpan);
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
    : IMoldSupportedValue<T[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
    where T : TFilterBase
{
    public T[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchSpan);
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
    : IMoldSupportedValue<T?[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
    where T : TFilterBase
{
    public T?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableMatchSpan);
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
public class FieldObjectSpanWhenNonNullAddFilteredStringBearer : IMoldSupportedValue<object[]?>, ISupportsOrderedCollectionPredicate<object>
  , ISupportsValueFormatString
{
    public object[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectSpan);
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
public class FieldNullableObjectSpanWhenNonNullAddFilteredStringBearer : IMoldSupportedValue<object?[]?>
  , ISupportsOrderedCollectionPredicate<object?>, ISupportsValueFormatString
{
    public object?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableObjectSpan);
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
public class FieldBoolReadOnlySpanWhenNonNullAddFilteredStringBearer : IMoldSupportedValue<bool[]?>, ISupportsOrderedCollectionPredicate<bool>
{
    public bool[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolReadOnlySpan);
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
    : IMoldSupportedValue<bool?[]?>, ISupportsOrderedCollectionPredicate<bool?>
{
    public bool?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolReadOnlySpan);
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
    : IMoldSupportedValue<TFmt[]?>, ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString
    where TFmt : struct, ISpanFormattable
{
    public TFmt[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableReadOnlySpan);
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
    : IMoldSupportedValue<TFmt?[]?>, ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString
    where TFmt : class, ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableNullableReadOnlySpan);
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
    : IMoldSupportedValue<TFmtStruct?[]?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableReadOnlySpan);
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
    : IMoldSupportedValue<TCloaked[]?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerReadOnlySpan);
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
    : IMoldSupportedValue<TCloaked?[]?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : class, TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerNullableReadOnlySpan);
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
    : IMoldSupportedValue<TCloakedStruct?[]?>, ISupportsValueRevealer<TCloakedStruct>
      , ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerReadOnlySpan);
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
    : IMoldSupportedValue<TBearer[]?>, ISupportsOrderedCollectionPredicate<TBearerBase>
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerReadOnlySpan);
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
    : IMoldSupportedValue<TBearer?[]?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerNullableReadOnlySpan);
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
    : IMoldSupportedValue<TBearerStruct?[]?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerReadOnlySpan);
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
public class FieldStringReadOnlySpanWhenNonNullAddFilteredStringBearer : IMoldSupportedValue<string[]?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public string[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringReadOnlySpan);
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
    : IMoldSupportedValue<string?[]?>, ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringReadOnlySpan);
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
    : IMoldSupportedValue<TCharSeq[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceReadOnlySpan);
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
    : IMoldSupportedValue<TCharSeq?[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCharSequenceReadOnlySpan);
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
    : IMoldSupportedValue<StringBuilder[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderReadOnlySpan);
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
    : IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBuilderReadOnlySpan);
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
    : IMoldSupportedValue<T[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
    where T : TFilterBase
{
    public T[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchReadOnlySpan);
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
    : IMoldSupportedValue<T?[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
    where T : TFilterBase
{
    public T?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableMatchReadOnlySpan);
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
public class FieldObjectReadOnlySpanWhenNonNullAddFilteredStringBearer : IMoldSupportedValue<object[]?>
  , ISupportsOrderedCollectionPredicate<object>, ISupportsValueFormatString
{
    public object[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectReadOnlySpan);
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
    : IMoldSupportedValue<object?[]?>, ISupportsOrderedCollectionPredicate<object?>, ISupportsValueFormatString
{
    public object?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableObjectReadOnlySpan);
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
public class FieldBoolArrayWhenNonNullAddFilteredStringBearer : IMoldSupportedValue<bool[]?>, ISupportsOrderedCollectionPredicate<bool>
{
    public bool[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolArray);
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
public class FieldNullableBoolArrayWhenNonNullAddFilteredStringBearer : IMoldSupportedValue<bool?[]?>, ISupportsOrderedCollectionPredicate<bool?>
{
    public bool?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolArray);
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
public class FieldSpanFormattableArrayWhenNonNullAddFilteredStringBearer<TFmt> : IMoldSupportedValue<TFmt?[]?>
  , ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableArray);
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
    : IMoldSupportedValue<TFmtStruct?[]?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableArray);
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
    : IMoldSupportedValue<TCloaked[]?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase> where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerArray);
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
    : IMoldSupportedValue<TCloakedStruct?[]?>, ISupportsValueRevealer<TCloakedStruct>
      , ISupportsOrderedCollectionPredicate<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerArray);
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
    : IMoldSupportedValue<TBearer?[]?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerArray);
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
    : IMoldSupportedValue<TBearerStruct?[]?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerArray);
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
public class FieldStringArrayWhenNonNullAddFilteredStringBearer : IMoldSupportedValue<string?[]?>, ISupportsOrderedCollectionPredicate<string?>
  , ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringArray);
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
    : IMoldSupportedValue<TCharSeq?[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceArray);
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
    : IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderArray);
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
    : IMoldSupportedValue<T[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
    where T : TFilterBase
{
    public T[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchArray);
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
    : IMoldSupportedValue<object?[]?>, ISupportsOrderedCollectionPredicate<object>
{
    public object?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectArray);
    public object?[]? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObject
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectArray, ElementPredicate);
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStruct)]
public class FieldBoolListWhenNonNullAddFilteredStringBearer : IMoldSupportedValue<IReadOnlyList<bool>>
  , ISupportsOrderedCollectionPredicate<bool>
{
    public IReadOnlyList<bool>? ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList
    {
        get => Value;
        set => Value = value!;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList);
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
public class FieldNullableBoolListWhenNonNullAddFilteredStringBearer : IMoldSupportedValue<IReadOnlyList<bool?>?>
  , ISupportsOrderedCollectionPredicate<bool?>
{
    public IReadOnlyList<bool?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList);
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
    : IMoldSupportedValue<IReadOnlyList<TFmt?>?>, ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString
    where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList);
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
    : IMoldSupportedValue<IReadOnlyList<TFmtStruct?>?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList);
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
    : IMoldSupportedValue<IReadOnlyList<TCloaked>?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase> where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public IReadOnlyList<TCloaked>? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList);
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
public class FieldNullableCloakedBearerListWhenNonNullAddFilteredStringBearer<TCloakedStruct> : 
  IMoldSupportedValue<IReadOnlyList<TCloakedStruct?>?>, ISupportsValueRevealer<TCloakedStruct>
  , ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList);
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
    : IMoldSupportedValue<IReadOnlyList<TBearer?>?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : IStringBearer, TBearerBase
{
    public IReadOnlyList<TBearer?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList);
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
    : IMoldSupportedValue<IReadOnlyList<TBearerStruct?>?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList);
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
public class FieldStringListWhenNonNullAddFilteredStringBearer : IMoldSupportedValue<IReadOnlyList<string?>?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public IReadOnlyList<string?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList);
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
    : IMoldSupportedValue<List<TCharSeq?>?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase>
      , ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public List<TCharSeq?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList);
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
    : IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList);
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
    : IMoldSupportedValue<IReadOnlyList<T>?>, ISupportsOrderedCollectionPredicate<TFilterBase?>, ISupportsValueFormatString
    where T : TFilterBase
{
    public IReadOnlyList<T>? ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList);
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
public class FieldObjectListWhenNonNullAddFilteredStringBearer : IMoldSupportedValue<IReadOnlyList<object?>?>
  , ISupportsOrderedCollectionPredicate<object>, ISupportsValueFormatString
{
    public IReadOnlyList<object?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList);
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
