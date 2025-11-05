using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.OrderedCollectionFields;


[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolSpanWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<bool[]?>, ISupportsOrderedCollectionPredicate<bool>
  , ISupportsValueFormatString
{
    public bool[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolSpan);
    public bool[]? Value { get; set; }

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolSpanWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<bool?[]?>, ISupportsOrderedCollectionPredicate<bool?>
  , ISupportsValueFormatString
{
    public bool?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolSpan);
    public bool?[]? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableSpanWhenPopulatedWithFilterStringBearer<TFmt> : IMoldSupportedValue<TFmt[]?>
  , ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public TFmt[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableSpan);
    public TFmt[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassSpanWhenPopulatedWithFilterStringBearer<TFmt> : IMoldSupportedValue<TFmt?[]?>
  , ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString where TFmt : class, ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableSpan);
    public TFmt?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableSpanWhenPopulatedWithFilterStringBearer<TFmtStruct>
    : IMoldSupportedValue<TFmtStruct?[]?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableSpan);
    public TFmtStruct?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerSpanWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase> : 
    IMoldSupportedValue<TCloaked[]?>, ISupportsValueRevealer<TCloakedRevealBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerSpan);
    public TCloaked[]? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedRevealBase>)value;
    }

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerSpan.AsSpan()
              , ElementPredicate, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableClass | AcceptsSpanFormattable
                | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer)]
public class FieldCloakedBearerNullableClassSpanWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase> : 
  IMoldSupportedValue<TCloaked?[]?>, ISupportsValueRevealer<TCloakedRevealBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : class, TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableSpan);
    public TCloaked?[]? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedRevealBase>)value;
    }

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterRevealNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableSpan.AsSpan()
              , ElementPredicate, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerSpanWhenPopulatedWithFilterStringBearer<TCloakedStruct> : IMoldSupportedValue<TCloakedStruct?[]?>
  , ISupportsValueRevealer<TCloakedStruct>, ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerSpan);
    public TCloakedStruct?[]? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerSpan.AsSpan()
              , ElementPredicate, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class FieldStringBearerSpanWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : IMoldSupportedValue<TBearer[]?>, ISupportsOrderedCollectionPredicate<TBearerBase>
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerSpan);
    public TBearer[]? Value { get; set; }

    public OrderedCollectionPredicate<TBearerBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerSpan.AsSpan(), ElementPredicate)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerNullableClassSpanWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : IMoldSupportedValue<TBearer?[]?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerNullableSpan);
    public TBearer?[]? Value { get; set; }

    public OrderedCollectionPredicate<TBearerBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterRevealNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerNullableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerNullableSpan.AsSpan()
              , ElementPredicate)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerSpanWhenPopulatedWithFilterStringBearer<TBearerStruct>
    : IMoldSupportedValue<TBearerStruct?[]?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerSpan);
    public TBearerStruct?[]? Value { get; set; }

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerSpan.AsSpan()
              , ElementPredicate)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsString | SupportsValueFormatString)]
public class FieldStringSpanWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<string[]?>, ISupportsOrderedCollectionPredicate<string?>
  , ISupportsValueFormatString
{
    public string[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringSpan);
    public string[]? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsString | SupportsValueFormatString)]
public class FieldStringNullableSpanWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<string?[]?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringSpan);
    public string?[]? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenPopulatedWithFilterNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceSpanWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : IMoldSupportedValue<TCharSeq[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceSpan);
    public TCharSeq[]? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceNullableSpanWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : IMoldSupportedValue<TCharSeq?[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCharSequenceSpan);
    public TCharSeq?[]? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeqNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCharSequenceSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCharSequenceSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderSpanWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<StringBuilder[]?>
  , ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderSpan);
    public StringBuilder[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderNullableSpanWhenPopulatedWithFilterStringBearer
    : IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderSpan);
    public StringBuilder?[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchSpanWhenPopulatedWithFilterStringBearer<TAny, TAnyFilterBase>
    : IMoldSupportedValue<TAny[]?>, ISupportsOrderedCollectionPredicate<TAnyFilterBase>, ISupportsValueFormatString
    where TAny : TAnyFilterBase
{
    public TAny[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchSpan);
    public TAny[]? Value { get; set; }

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchNullableSpanWhenPopulatedWithFilterStringBearer<TAny, TAnyFilterBase>
    : IMoldSupportedValue<TAny?[]?>, ISupportsOrderedCollectionPredicate<TAnyFilterBase>, ISupportsValueFormatString
    where TAny : TAnyFilterBase
{
    public TAny?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableMatchSpan);
    public TAny?[]? Value { get; set; }

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenPopulatedWithFilterMatchNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableMatchSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableMatchSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass | SupportsValueFormatString)]
public class FieldObjectSpanWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<object[]?>, ISupportsOrderedCollectionPredicate<object>
  , ISupportsValueFormatString
{
    public object[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectSpan);
    public object[]? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectSpan.AsSpan()
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldNullableObjectSpanWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<object?[]?>
  , ISupportsOrderedCollectionPredicate<object?>, ISupportsValueFormatString
{
    public object?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectSpan);
    public object?[]? Value { get; set; }

    public OrderedCollectionPredicate<object?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObjectNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectSpan.AsSpan()
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolReadOnlySpanWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<bool[]?>, ISupportsOrderedCollectionPredicate<bool>
  , ISupportsValueFormatString
{
    public bool[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolReadOnlySpan);
    public bool[]? Value { get; set; }

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolReadOnlySpan)
              , (ReadOnlySpan<bool>)ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolReadOnlySpanWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<bool?[]?>
  , ISupportsOrderedCollectionPredicate<bool?>, ISupportsValueFormatString
{
    public bool?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolReadOnlySpan);
    public bool?[]? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolReadOnlySpan)
              , (ReadOnlySpan<bool?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableReadOnlySpanWhenPopulatedWithFilterStringBearer<TFmt>
    : IMoldSupportedValue<TFmt[]?>, ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString
    where TFmt : struct, ISpanFormattable
{
    public TFmt[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableReadOnlySpan);
    public TFmt[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmt>)ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassReadOnlySpanWhenPopulatedWithFilterStringBearer<TFmt>
    : IMoldSupportedValue<TFmt?[]?>, ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString
    where TFmt : class, ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableReadOnlySpan);
    public TFmt?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableReadOnlySpan)
              , (ReadOnlySpan<TFmt?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableReadOnlySpanWhenPopulatedWithFilterStringBearer<TFmtStruct>
    : IMoldSupportedValue<TFmtStruct?[]?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableReadOnlySpan);
    public TFmtStruct?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmtStruct?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerReadOnlySpanWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IMoldSupportedValue<TCloaked[]?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerReadOnlySpan);
    public TCloaked[]? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedRevealBase>)value;
    }

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloaked>)ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerReadOnlySpan
              , ElementPredicate, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerNullableClassReadOnlySpanWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IMoldSupportedValue<TCloaked?[]?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : class, TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableReadOnlySpan);
    public TCloaked?[]? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedRevealBase>)value;
    }

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableReadOnlySpan)
              , (ReadOnlySpan<TCloaked?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableReadOnlySpan
              , ElementPredicate, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerReadOnlySpanWhenPopulatedWithFilterStringBearer<TCloakedStruct>
    : IMoldSupportedValue<TCloakedStruct?[]?>, ISupportsValueRevealer<TCloakedStruct>
      , ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerReadOnlySpan);
    public TCloakedStruct?[]? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloakedStruct?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerReadOnlySpan
              , ElementPredicate, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class FieldStringBearerReadOnlySpanWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : IMoldSupportedValue<TBearer[]?>, ISupportsOrderedCollectionPredicate<TBearerBase>
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerReadOnlySpan);
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

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerNullableClassReadOnlySpanWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : IMoldSupportedValue<TBearer?[]?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerNullableReadOnlySpan);
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

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerReadOnlySpanWhenPopulatedWithFilterStringBearer<TBearerStruct>
    : IMoldSupportedValue<TBearerStruct?[]?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerReadOnlySpan);
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

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsString | SupportsValueFormatString)]
public class FieldStringReadOnlySpanWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<string[]?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public string[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringReadOnlySpan);
    public string[]? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringReadOnlySpan)
              , (ReadOnlySpan<string>)ComplexTypeCollectionFieldWhenPopulatedWithFilterStringReadOnlySpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsString | SupportsValueFormatString)]
public class FieldStringNullableReadOnlySpanWhenPopulatedWithFilterStringBearer
    : IMoldSupportedValue<string?[]?>, ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringReadOnlySpan);
    public string?[]? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringReadOnlySpan),
                (ReadOnlySpan<string?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceReadOnlySpanWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : IMoldSupportedValue<TCharSeq[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceReadOnlySpan);
    public TCharSeq[]? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceReadOnlySpan)
              , (ReadOnlySpan<TCharSeq>)ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceNullableReadOnlySpanWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : IMoldSupportedValue<TCharSeq?[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCharSequenceReadOnlySpan);
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

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderReadOnlySpanWhenPopulatedWithFilterStringBearer
    : IMoldSupportedValue<StringBuilder[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderReadOnlySpan);
    public StringBuilder[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderReadOnlySpan)
              , (ReadOnlySpan<StringBuilder>)ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderNullableReadOnlySpanWhenPopulatedWithFilterStringBearer
    : IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderReadOnlySpan);
    public StringBuilder?[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderReadOnlySpan)
              , (ReadOnlySpan<StringBuilder?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchReadOnlySpanWhenPopulatedWithFilterStringBearer<TAny, TAnyFilterBase>
    : IMoldSupportedValue<TAny[]?>, ISupportsOrderedCollectionPredicate<TAnyFilterBase>, ISupportsValueFormatString
    where TAny : TAnyFilterBase
{
    public TAny[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchReadOnlySpan);
    public TAny[]? Value { get; set; }

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchReadOnlySpan)
              , (ReadOnlySpan<TAny>)ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchNullableReadOnlySpanWhenPopulatedWithFilterStringBearer<TAny, TAnyFilterBase>
    : IMoldSupportedValue<TAny?[]?>, ISupportsOrderedCollectionPredicate<TAnyFilterBase>, ISupportsValueFormatString
    where TAny : TAnyFilterBase
{
    public TAny?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableMatchReadOnlySpan);
    public TAny?[]? Value { get; set; }

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterMatchNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableMatchReadOnlySpan)
              , (ReadOnlySpan<TAny?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableMatchReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass | SupportsValueFormatString)]
public class FieldObjectReadOnlySpanWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<object[]?>
  , ISupportsOrderedCollectionPredicate<object>, ISupportsValueFormatString
{
    public object[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectReadOnlySpan);
    public object[]? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectReadOnlySpan)
              , (ReadOnlySpan<object>)ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectReadOnlySpan
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldNullableObjectReadOnlySpanWhenPopulatedWithFilterStringBearer
    : IMoldSupportedValue<object?[]?>, ISupportsOrderedCollectionPredicate<object?>, ISupportsValueFormatString
{
    public object?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectReadOnlySpan);
    public object?[]? Value { get; set; }

    public OrderedCollectionPredicate<object?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObjectNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectReadOnlySpan)
              , (ReadOnlySpan<object?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectReadOnlySpan
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolArrayWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<bool[]?>, ISupportsOrderedCollectionPredicate<bool>
  , ISupportsValueFormatString
{
    public bool[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolArray);
    public bool[]? Value { get; set; }

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolArray, ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolArrayWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<bool?[]?>, ISupportsOrderedCollectionPredicate<bool?>
  , ISupportsValueFormatString
{
    public bool?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolArray);
    public bool?[]? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolArray, ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableArrayWhenPopulatedWithFilterStringBearer<TFmt> : IMoldSupportedValue<TFmt?[]?>
  , ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableArray);
    public TFmt?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableArray
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableArrayWhenPopulatedWithFilterStringBearer<TFmtStruct>
    : IMoldSupportedValue<TFmtStruct?[]?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableArray);
    public TFmtStruct?[]? Value { get; set; }

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableArray
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerArrayWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IMoldSupportedValue<TCloaked[]?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase> where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerArray);
    public TCloaked[]? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedRevealBase>)value;
    }

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerArray, ElementPredicate
              , ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerArrayWhenPopulatedWithFilterStringBearer<TCloakedStruct>
    : IMoldSupportedValue<TCloakedStruct?[]?>, ISupportsValueRevealer<TCloakedStruct>
      , ISupportsOrderedCollectionPredicate<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerArray);
    public TCloakedStruct?[]? Value { get; set; }


    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerArray
              , ElementPredicate, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass |
                  AcceptsStringBearer)]
public class FieldStringBearerArrayWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : IMoldSupportedValue<TBearer?[]?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerArray);
    public TBearer?[]? Value { get; set; }

    public OrderedCollectionPredicate<TBearerBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerArray, ElementPredicate)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerArrayWhenPopulatedWithFilterStringBearer<TBearerStruct>
    : IMoldSupportedValue<TBearerStruct?[]?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerArray);
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

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsString | SupportsValueFormatString)]
public class FieldStringArrayWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<string?[]?>, ISupportsOrderedCollectionPredicate<string?>
  , ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringArray);
    public string?[]? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringArray
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceArrayWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : IMoldSupportedValue<TCharSeq?[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceArray);
    public TCharSeq?[]? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceArray
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderArrayWhenPopulatedWithFilterStringBearer
    : IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderArray);
    public StringBuilder?[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderArray
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchArrayWhenPopulatedWithFilterStringBearer<TAny, TAnyFilterBase>
    : IMoldSupportedValue<TAny[]?>, ISupportsOrderedCollectionPredicate<TAnyFilterBase>, ISupportsValueFormatString
    where TAny : TAnyFilterBase
{
    public TAny[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchArray);
    public TAny[]? Value { get; set; }

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenPopulatedWithFilterMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchArray
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsAnyGeneric)]
public class FieldObjectArrayWhenPopulatedWithFilterStringBearer
    : IMoldSupportedValue<object?[]?>, ISupportsOrderedCollectionPredicate<object>
{
    public object?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectArray);
    public object?[]? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectArray, ElementPredicate);

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolListWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<IReadOnlyList<bool>>
  , ISupportsOrderedCollectionPredicate<bool>, ISupportsValueFormatString
{
    public IReadOnlyList<bool>? ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolList
    {
        get => Value;
        set => Value = value!;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolList);
    public IReadOnlyList<bool> Value { get; set; } = null!;

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolList, ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolListWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<IReadOnlyList<bool?>?>
  , ISupportsOrderedCollectionPredicate<bool?>, ISupportsValueFormatString
{
    public IReadOnlyList<bool?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolList);
    public IReadOnlyList<bool?>? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolList, ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableListWhenPopulatedWithFilterStringBearer<TFmt>
    : IMoldSupportedValue<IReadOnlyList<TFmt?>?>, ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString
    where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableList);
    public IReadOnlyList<TFmt?>? Value { get; set; }

    public OrderedCollectionPredicate<TFmt> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmt>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableListWhenPopulatedWithFilterStringBearer<TFmtStruct>
    : IMoldSupportedValue<IReadOnlyList<TFmtStruct?>?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableList);
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

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerListWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IMoldSupportedValue<IReadOnlyList<TCloaked>?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase> where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public IReadOnlyList<TCloaked>? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList);
    public IReadOnlyList<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedRevealBase>)value;
    }

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList, ElementPredicate
              , ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable
                | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer)]
public class FieldNullableCloakedBearerListWhenPopulatedWithFilterStringBearer<TCloakedStruct> : 
  IMoldSupportedValue<IReadOnlyList<TCloakedStruct?>?>, ISupportsValueRevealer<TCloakedStruct>
  , ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList);
    public IReadOnlyList<TCloakedStruct?>? Value { get; set; }


    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList
              , ElementPredicate, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerListWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : IMoldSupportedValue<IReadOnlyList<TBearer?>?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : IStringBearer, TBearerBase
{
    public IReadOnlyList<TBearer?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerList);
    public IReadOnlyList<TBearer?>? Value { get; set; }

    public OrderedCollectionPredicate<TBearerBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerList, ElementPredicate)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate)]
public class FieldNullableStringBearerListWhenPopulatedWithFilterStringBearer<TBearerStruct>
    : IMoldSupportedValue<IReadOnlyList<TBearerStruct?>?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerList);
    public IReadOnlyList<TBearerStruct?>? Value { get; set; }

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerList, ElementPredicate)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsString | SupportsValueFormatString)]
public class FieldStringListWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<IReadOnlyList<string?>?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public IReadOnlyList<string?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringList);
    public IReadOnlyList<string?>? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceListWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : IMoldSupportedValue<IReadOnlyList<TCharSeq?>?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>
      , ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IReadOnlyList<TCharSeq?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceList);
    public IReadOnlyList<TCharSeq?>? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderListWhenPopulatedWithFilterStringBearer
    : IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderList);
    public StringBuilder?[]? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchListWhenPopulatedWithFilterStringBearer<TAny, TAnyFilterBase>
    : IMoldSupportedValue<IReadOnlyList<TAny>?>, ISupportsOrderedCollectionPredicate<TAnyFilterBase>, ISupportsValueFormatString
    where TAny : TAnyFilterBase
{
    public IReadOnlyList<TAny>? ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchList);
    public IReadOnlyList<TAny>? Value { get; set; }

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldObjectListWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<IReadOnlyList<object?>?>
  , ISupportsOrderedCollectionPredicate<object>, ISupportsValueFormatString
{
    public IReadOnlyList<object?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectList);
    public IReadOnlyList<object?>? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectList
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}
