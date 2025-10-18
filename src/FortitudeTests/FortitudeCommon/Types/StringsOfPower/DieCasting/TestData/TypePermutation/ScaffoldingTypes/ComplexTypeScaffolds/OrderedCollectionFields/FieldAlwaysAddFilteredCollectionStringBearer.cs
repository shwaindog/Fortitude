using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    OrderedCollectionFields;

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsStruct)]
public class FieldBoolSpanAlwaysAddFilteredStringBearer : IMoldSupportedValue<bool[]?>, ISupportsOrderedCollectionPredicate<bool>
{
    public bool[]? ComplexTypeCollectionFieldAlwaysAddFilteredBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolSpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class FieldNullableBoolSpanAlwaysAddFilteredStringBearer : IMoldSupportedValue<bool?[]?>, ISupportsOrderedCollectionPredicate<bool?>
{
    public bool?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolSpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableSpanAlwaysAddFilteredStringBearer<TFmt> : IMoldSupportedValue<TFmt[]?>
  , ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public TFmt[]? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableSpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassSpanAlwaysAddFilteredStringBearer<TFmt> : IMoldSupportedValue<TFmt?[]?>
  , ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString where TFmt : class, ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableNullableSpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableSpanAlwaysAddFilteredStringBearer<TFmtStruct>
    : IMoldSupportedValue<TFmtStruct?[]?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableSpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerSpanAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase> : IStringBearer,
    IMoldSupportedValue<TCloaked[]?>, ISupportsValueRevealer<TCloakedRevealBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerSpan);
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
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerSpan.AsSpan()
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsNullableClass | AcceptsSpanFormattable
                | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer)]
public class FieldCloakedBearerNullableClassSpanAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase> : IMoldSupportedValue<TCloaked?[]?>, ISupportsValueRevealer<TCloakedRevealBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : class, TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked?[]? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableSpan);
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
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableSpan.AsSpan()
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerSpanAlwaysAddFilteredStringBearer<TCloakedStruct> : IMoldSupportedValue<TCloakedStruct?[]?>
  , ISupportsValueRevealer<TCloakedStruct>, ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerSpan);
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
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerSpan.AsSpan()
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class FieldStringBearerSpanAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : IMoldSupportedValue<TBearer[]?>, ISupportsOrderedCollectionPredicate<TBearerBase>
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerSpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerNullableClassSpanAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : IMoldSupportedValue<TBearer?[]?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerNullableSpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerSpanAlwaysAddFilteredStringBearer<TBearerStruct>
    : IMoldSupportedValue<TBearerStruct?[]?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerSpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsString | SupportsValueFormatString)]
public class FieldStringSpanAlwaysAddFilteredStringBearer : IMoldSupportedValue<string[]?>, ISupportsOrderedCollectionPredicate<string?>
  , ISupportsValueFormatString
{
    public string[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringSpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsString | SupportsValueFormatString)]
public class FieldStringNullableSpanAlwaysAddFilteredStringBearer : IMoldSupportedValue<string?[]?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringSpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceSpanAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IMoldSupportedValue<TCharSeq[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq[]? ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceSpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceNullableSpanAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IMoldSupportedValue<TCharSeq?[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCharSequenceSpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderSpanAlwaysAddFilteredStringBearer : IMoldSupportedValue<StringBuilder[]?>
  , ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderSpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderNullableSpanAlwaysAddFilteredStringBearer
    : IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBuilderSpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchSpanAlwaysAddFilteredStringBearer<TAny, TAnyFilterBase>
    : IMoldSupportedValue<TAny[]?>, ISupportsOrderedCollectionPredicate<TAnyFilterBase>, ISupportsValueFormatString
    where TAny : TAnyFilterBase
{
    public TAny[]? ComplexTypeCollectionFieldAlwaysAddFilteredMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchSpan);
    public TAny[]? Value { get; set; }

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatch
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredMatchSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchNullableSpanAlwaysAddFilteredStringBearer<TAny, TAnyFilterBase>
    : IMoldSupportedValue<TAny?[]?>, ISupportsOrderedCollectionPredicate<TAnyFilterBase>, ISupportsValueFormatString
    where TAny : TAnyFilterBase
{
    public TAny?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableMatchSpan);
    public TAny?[]? Value { get; set; }

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass | SupportsValueFormatString)]
public class FieldObjectSpanAlwaysAddFilteredStringBearer : IMoldSupportedValue<object[]?>, ISupportsOrderedCollectionPredicate<object>
  , ISupportsValueFormatString
{
    public object[]? ComplexTypeCollectionFieldAlwaysAddFilteredObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectSpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldNullableObjectSpanAlwaysAddFilteredStringBearer : IMoldSupportedValue<object?[]?>
  , ISupportsOrderedCollectionPredicate<object?>, ISupportsValueFormatString
{
    public object?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableObjectSpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsStruct)]
public class FieldBoolReadOnlySpanAlwaysAddFilteredStringBearer : IMoldSupportedValue<bool[]?>, ISupportsOrderedCollectionPredicate<bool>
{
    public bool[]? ComplexTypeCollectionFieldAlwaysAddFilteredBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolReadOnlySpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class FieldNullableBoolReadOnlySpanAlwaysAddFilteredStringBearer
    : IMoldSupportedValue<bool?[]?>, ISupportsOrderedCollectionPredicate<bool?>
{
    public bool?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolReadOnlySpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableReadOnlySpanAlwaysAddFilteredStringBearer<TFmt>
    : IMoldSupportedValue<TFmt[]?>, ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString
    where TFmt : struct, ISpanFormattable
{
    public TFmt[]? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableReadOnlySpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassReadOnlySpanAlwaysAddFilteredStringBearer<TFmt>
    : IMoldSupportedValue<TFmt?[]?>, ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString
    where TFmt : class, ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableNullableReadOnlySpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableReadOnlySpanAlwaysAddFilteredStringBearer<TFmtStruct>
    : IMoldSupportedValue<TFmtStruct?[]?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableReadOnlySpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerReadOnlySpanAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IMoldSupportedValue<TCloaked[]?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerReadOnlySpan);
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
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloaked>)ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerReadOnlySpan
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerNullableClassReadOnlySpanAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IMoldSupportedValue<TCloaked?[]?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : class, TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked?[]? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableReadOnlySpan);
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
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableReadOnlySpan)
              , (ReadOnlySpan<TCloaked?>)ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableReadOnlySpan
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerReadOnlySpanAlwaysAddFilteredStringBearer<TCloakedStruct>
    : IMoldSupportedValue<TCloakedStruct?[]?>, ISupportsValueRevealer<TCloakedStruct>
      , ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerReadOnlySpan);
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
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloakedStruct?>)ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerReadOnlySpan
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class FieldStringBearerReadOnlySpanAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : IMoldSupportedValue<TBearer[]?>, ISupportsOrderedCollectionPredicate<TBearerBase>
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerReadOnlySpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerNullableClassReadOnlySpanAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : IMoldSupportedValue<TBearer?[]?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerNullableReadOnlySpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerReadOnlySpanAlwaysAddFilteredStringBearer<TBearerStruct>
    : IMoldSupportedValue<TBearerStruct?[]?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerReadOnlySpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsString | SupportsValueFormatString)]
public class FieldStringReadOnlySpanAlwaysAddFilteredStringBearer : IMoldSupportedValue<string[]?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public string[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringReadOnlySpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsString | SupportsValueFormatString)]
public class FieldStringNullableReadOnlySpanAlwaysAddFilteredStringBearer
    : IMoldSupportedValue<string?[]?>, ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringReadOnlySpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceReadOnlySpanAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IMoldSupportedValue<TCharSeq[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq[]? ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceReadOnlySpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceNullableReadOnlySpanAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IMoldSupportedValue<TCharSeq?[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCharSequenceReadOnlySpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderReadOnlySpanAlwaysAddFilteredStringBearer
    : IMoldSupportedValue<StringBuilder[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderReadOnlySpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderNullableReadOnlySpanAlwaysAddFilteredStringBearer
    : IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBuilderReadOnlySpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchReadOnlySpanAlwaysAddFilteredStringBearer<TAny, TAnyFilterBase>
    : IMoldSupportedValue<TAny[]?>, ISupportsOrderedCollectionPredicate<TAnyFilterBase>, ISupportsValueFormatString
    where TAny : TAnyFilterBase
{
    public TAny[]? ComplexTypeCollectionFieldAlwaysAddFilteredMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchReadOnlySpan);
    public TAny[]? Value { get; set; }

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatch
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchReadOnlySpan)
              , (ReadOnlySpan<TAny>)ComplexTypeCollectionFieldAlwaysAddFilteredMatchReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchNullableReadOnlySpanAlwaysAddFilteredStringBearer<TAny, TAnyFilterBase>
    : IMoldSupportedValue<TAny?[]?>, ISupportsOrderedCollectionPredicate<TAnyFilterBase>, ISupportsValueFormatString
    where TAny : TAnyFilterBase
{
    public TAny?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableMatchReadOnlySpan);
    public TAny?[]? Value { get; set; }

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatchNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableMatchReadOnlySpan)
              , (ReadOnlySpan<TAny?>)ComplexTypeCollectionFieldAlwaysAddFilteredNullableMatchReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass | SupportsValueFormatString)]
public class FieldObjectReadOnlySpanAlwaysAddFilteredStringBearer : IMoldSupportedValue<object[]?>
  , ISupportsOrderedCollectionPredicate<object>, ISupportsValueFormatString
{
    public object[]? ComplexTypeCollectionFieldAlwaysAddFilteredObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectReadOnlySpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldNullableObjectReadOnlySpanAlwaysAddFilteredStringBearer
    : IMoldSupportedValue<object?[]?>, ISupportsOrderedCollectionPredicate<object?>, ISupportsValueFormatString
{
    public object?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableObjectReadOnlySpan);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsStruct)]
public class FieldBoolArrayAlwaysAddFilteredStringBearer : IMoldSupportedValue<bool[]?>, ISupportsOrderedCollectionPredicate<bool>
{
    public bool[]? ComplexTypeCollectionFieldAlwaysAddFilteredBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolArray);
    public bool[]? Value { get; set; }

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredBoolArray, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class FieldNullableBoolArrayAlwaysAddFilteredStringBearer : IMoldSupportedValue<bool?[]?>, ISupportsOrderedCollectionPredicate<bool?>
{
    public bool?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolArray);
    public bool?[]? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolArray, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableArrayAlwaysAddFilteredStringBearer<TFmt> : IMoldSupportedValue<TFmt?[]?>
  , ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableArray);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableArrayAlwaysAddFilteredStringBearer<TFmtStruct>
    : IMoldSupportedValue<TFmtStruct?[]?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableArray);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerArrayAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IMoldSupportedValue<TCloaked[]?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase> where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerArray);
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
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerArray, ElementPredicate
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerArrayAlwaysAddFilteredStringBearer<TCloakedStruct>
    : IMoldSupportedValue<TCloakedStruct?[]?>, ISupportsValueRevealer<TCloakedStruct>
      , ISupportsOrderedCollectionPredicate<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerArray);
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
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerArray
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass |
                  AcceptsStringBearer)]
public class FieldStringBearerArrayAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : IMoldSupportedValue<TBearer?[]?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerArray);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerArrayAlwaysAddFilteredStringBearer<TBearerStruct>
    : IMoldSupportedValue<TBearerStruct?[]?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerArray);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsString | SupportsValueFormatString)]
public class FieldStringArrayAlwaysAddFilteredStringBearer : IMoldSupportedValue<string?[]?>, ISupportsOrderedCollectionPredicate<string?>
  , ISupportsValueFormatString
{
    public string?[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringArray);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceArrayAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IMoldSupportedValue<TCharSeq?[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceArray);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderArrayAlwaysAddFilteredStringBearer
    : IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public StringBuilder?[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderArray);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchArrayAlwaysAddFilteredStringBearer<TAny, TAnyFilterBase>
    : IMoldSupportedValue<TAny[]?>, ISupportsOrderedCollectionPredicate<TAnyFilterBase>, ISupportsValueFormatString
    where TAny : TAnyFilterBase
{
    public TAny[]? ComplexTypeCollectionFieldAlwaysAddFilteredMatchArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchArray);
    public TAny[]? Value { get; set; }

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsAnyGeneric)]
public class FieldObjectArrayAlwaysAddFilteredStringBearer
    : IMoldSupportedValue<object?[]?>, ISupportsOrderedCollectionPredicate<object>
{
    public object?[]? ComplexTypeCollectionFieldAlwaysAddFilteredObjectArray
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectArray);
    public object?[]? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObject
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredObjectArray, ElementPredicate);
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsStruct)]
public class FieldBoolListAlwaysAddFilteredStringBearer : IMoldSupportedValue<IReadOnlyList<bool>>
  , ISupportsOrderedCollectionPredicate<bool>
{
    public IReadOnlyList<bool>? ComplexTypeCollectionFieldAlwaysAddFilteredBoolList
    {
        get => Value;
        set => Value = value!;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolList);
    public IReadOnlyList<bool> Value { get; set; } = null!;

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredBoolList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class FieldNullableBoolListAlwaysAddFilteredStringBearer : IMoldSupportedValue<IReadOnlyList<bool?>?>
  , ISupportsOrderedCollectionPredicate<bool?>
{
    public IReadOnlyList<bool?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolList);
    public IReadOnlyList<bool?>? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableListAlwaysAddFilteredStringBearer<TFmt>
    : IMoldSupportedValue<IReadOnlyList<TFmt?>?>, ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString
    where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt?>? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableList);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableListAlwaysAddFilteredStringBearer<TFmtStruct>
    : IMoldSupportedValue<IReadOnlyList<TFmtStruct?>?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableList);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerListAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IMoldSupportedValue<IReadOnlyList<TCloaked>?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase> where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public IReadOnlyList<TCloaked>? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerList);
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
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerList, ElementPredicate
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable
                | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer)]
public class FieldNullableCloakedBearerListAlwaysAddFilteredStringBearer<TCloakedStruct> : IMoldSupportedValue<IReadOnlyList<TCloakedStruct?>?>, ISupportsValueRevealer<TCloakedStruct>
  , ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerList);
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
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerList
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsStruct | AcceptsClass 
                | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerListAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : IMoldSupportedValue<IReadOnlyList<TBearer?>?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : IStringBearer, TBearerBase
{
    public IReadOnlyList<TBearer?>? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerList);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | FilterPredicate)]
public class FieldNullableStringBearerListAlwaysAddFilteredStringBearer<TBearerStruct>
    : IMoldSupportedValue<IReadOnlyList<TBearerStruct?>?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerList);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsString | SupportsValueFormatString)]
public class FieldStringListAlwaysAddFilteredStringBearer : IMoldSupportedValue<IReadOnlyList<string?>?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public IReadOnlyList<string?>? ComplexTypeCollectionFieldAlwaysAddFilteredStringList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringList);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceListAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IMoldSupportedValue<IReadOnlyList<TCharSeq?>?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>
      , ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IReadOnlyList<TCharSeq?>? ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceList);
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderListAlwaysAddFilteredStringBearer
    : IMoldSupportedValue<IReadOnlyList<StringBuilder?>?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public IReadOnlyList<StringBuilder?>? ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderList);
    public IReadOnlyList<StringBuilder?>? Value { get; set; }

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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchListAlwaysAddFilteredStringBearer<TAny, TAnyFilterBase>
    : IMoldSupportedValue<IReadOnlyList<TAny>?>, ISupportsOrderedCollectionPredicate<TAnyFilterBase>, ISupportsValueFormatString
    where TAny : TAnyFilterBase
{
    public IReadOnlyList<TAny>? ComplexTypeCollectionFieldAlwaysAddFilteredMatchList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchList);
    public IReadOnlyList<TAny>? Value { get; set; }

    public OrderedCollectionPredicate<TAnyFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TAnyFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatch
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredMatchList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldObjectListAlwaysAddFilteredStringBearer : IMoldSupportedValue<IReadOnlyList<object?>?>
  , ISupportsOrderedCollectionPredicate<object>, ISupportsValueFormatString
{
    public IReadOnlyList<object?>? ComplexTypeCollectionFieldAlwaysAddFilteredObjectList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectList);
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
