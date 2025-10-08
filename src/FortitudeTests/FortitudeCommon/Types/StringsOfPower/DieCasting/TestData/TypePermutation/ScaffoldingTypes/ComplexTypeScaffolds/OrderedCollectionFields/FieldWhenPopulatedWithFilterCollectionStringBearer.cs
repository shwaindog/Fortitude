using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.OrderedCollectionFields;


[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct)]
public class FieldBoolSpanWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<bool[]?>, ISupportsOrderedCollectionPredicate<bool>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class FieldNullableBoolSpanWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<bool?[]?>, ISupportsOrderedCollectionPredicate<bool?>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableSpanWhenPopulatedWithFilterStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt[]?>
  , ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString where TFmt : ISpanFormattable
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassSpanWhenPopulatedWithFilterStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt?[]?>
  , ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString where TFmt : class, ISpanFormattable
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableSpanWhenPopulatedWithFilterStringBearer<TFmtStruct>
    : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerSpanWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase> : IStringBearer,
    IMoldSupportedValue<TCloaked[]?>, ISupportsValueRevealer<TCloakedRevealBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerSpan
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
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerSpan.AsSpan()
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableClass | AcceptsSpanFormattable
                | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer)]
public class FieldCloakedBearerNullableClassSpanWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase> : IStringBearer
  , IMoldSupportedValue<TCloaked?[]?>, ISupportsValueRevealer<TCloakedRevealBase>, ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : class, TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableSpan
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
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableSpan.AsSpan()
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerSpanWhenPopulatedWithFilterStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<TCloakedStruct?[]?>
  , ISupportsValueRevealer<TCloakedStruct>, ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerSpan
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
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerSpan.AsSpan()
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class FieldStringBearerSpanWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerNullableClassSpanWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerSpanWhenPopulatedWithFilterStringBearer<TBearerStruct>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringSpanWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<string[]?>, ISupportsOrderedCollectionPredicate<string?>
  , ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringNullableSpanWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<string?[]?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceSpanWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<TCharSeq[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceNullableSpanWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<TCharSeq?[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderSpanWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<StringBuilder[]?>
  , ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderNullableSpanWhenPopulatedWithFilterStringBearer
    : IStringBearer, IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldMatchSpanWhenPopulatedWithFilterStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<T[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchNullableSpanWhenPopulatedWithFilterStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<T?[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass | SupportsValueFormatString)]
public class FieldObjectSpanWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<object[]?>, ISupportsOrderedCollectionPredicate<object>
  , ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldNullableObjectSpanWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<object?[]?>
  , ISupportsOrderedCollectionPredicate<object?>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct)]
public class FieldBoolReadOnlySpanWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<bool[]?>, ISupportsOrderedCollectionPredicate<bool>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class FieldNullableBoolReadOnlySpanWhenPopulatedWithFilterStringBearer
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableReadOnlySpanWhenPopulatedWithFilterStringBearer<TFmt>
    : IStringBearer, IMoldSupportedValue<TFmt[]?>, ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassReadOnlySpanWhenPopulatedWithFilterStringBearer<TFmt>
    : IStringBearer, IMoldSupportedValue<TFmt?[]?>, ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableReadOnlySpanWhenPopulatedWithFilterStringBearer<TFmtStruct>
    : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerReadOnlySpanWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IStringBearer, IMoldSupportedValue<TCloaked[]?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerReadOnlySpan
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
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloaked>)ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerReadOnlySpan
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerNullableClassReadOnlySpanWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IStringBearer, IMoldSupportedValue<TCloaked?[]?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase>
    where TCloaked : class, TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableReadOnlySpan
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
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableReadOnlySpan)
              , (ReadOnlySpan<TCloaked?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableReadOnlySpan
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerReadOnlySpanWhenPopulatedWithFilterStringBearer<TCloakedStruct>
    : IStringBearer, IMoldSupportedValue<TCloakedStruct?[]?>, ISupportsValueRevealer<TCloakedStruct>
      , ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerReadOnlySpan
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
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloakedStruct?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerReadOnlySpan
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsStringBearer)]
public class FieldStringBearerReadOnlySpanWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerNullableClassReadOnlySpanWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerReadOnlySpanWhenPopulatedWithFilterStringBearer<TBearerStruct>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringReadOnlySpanWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<string[]?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringNullableReadOnlySpanWhenPopulatedWithFilterStringBearer
    : IStringBearer, IMoldSupportedValue<string?[]?>, ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceReadOnlySpanWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<TCharSeq[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceNullableReadOnlySpanWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<TCharSeq?[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
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

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderReadOnlySpanWhenPopulatedWithFilterStringBearer
    : IStringBearer, IMoldSupportedValue<StringBuilder[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderNullableReadOnlySpanWhenPopulatedWithFilterStringBearer
    : IStringBearer, IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldMatchReadOnlySpanWhenPopulatedWithFilterStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<T[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchNullableReadOnlySpanWhenPopulatedWithFilterStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<T?[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass | SupportsValueFormatString)]
public class FieldObjectReadOnlySpanWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<object[]?>
  , ISupportsOrderedCollectionPredicate<object>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldNullableObjectReadOnlySpanWhenPopulatedWithFilterStringBearer
    : IStringBearer, IMoldSupportedValue<object?[]?>, ISupportsOrderedCollectionPredicate<object?>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct)]
public class FieldBoolArrayWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<bool[]?>, ISupportsOrderedCollectionPredicate<bool>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class FieldNullableBoolArrayWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<bool?[]?>, ISupportsOrderedCollectionPredicate<bool?>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableArrayWhenPopulatedWithFilterStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<TFmt?[]?>
  , ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString where TFmt : ISpanFormattable
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableArrayWhenPopulatedWithFilterStringBearer<TFmtStruct>
    : IStringBearer, IMoldSupportedValue<TFmtStruct?[]?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerArrayWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IStringBearer, IMoldSupportedValue<TCloaked[]?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase> where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerArray
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
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerArray, ElementPredicate
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerArrayWhenPopulatedWithFilterStringBearer<TCloakedStruct>
    : IStringBearer, IMoldSupportedValue<TCloakedStruct?[]?>, ISupportsValueRevealer<TCloakedStruct>
      , ISupportsOrderedCollectionPredicate<TCloakedStruct?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerArray
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
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerArray
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass |
                  AcceptsStringBearer)]
public class FieldStringBearerArrayWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerArrayWhenPopulatedWithFilterStringBearer<TBearerStruct>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringArrayWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<string?[]?>, ISupportsOrderedCollectionPredicate<string?>
  , ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceArrayWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<TCharSeq?[]?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderArrayWhenPopulatedWithFilterStringBearer
    : IStringBearer, IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchArrayWhenPopulatedWithFilterStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<T[]?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class FieldObjectArrayWhenPopulatedWithFilterStringBearer
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct)]
public class FieldBoolListWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<bool>>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class FieldNullableBoolListWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<bool?>?>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableListWhenPopulatedWithFilterStringBearer<TFmt>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TFmt?>?>, ISupportsOrderedCollectionPredicate<TFmt>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableListWhenPopulatedWithFilterStringBearer<TFmtStruct>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TFmtStruct?>?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
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

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerListWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TCloaked>?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase> where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public IReadOnlyList<TCloaked>? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList
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
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList, ElementPredicate
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable
                | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer)]
public class FieldNullableCloakedBearerListWhenPopulatedWithFilterStringBearer<TCloakedStruct> : IStringBearer
  , IMoldSupportedValue<IReadOnlyList<TCloakedStruct?>?>, ISupportsValueRevealer<TCloakedStruct>
  , ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList
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
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerListWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldNullableStringBearerListWhenPopulatedWithFilterStringBearer<TBearerStruct>
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

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringListWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<string?>?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceListWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<TCharSeq?>?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>
      , ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderListWhenPopulatedWithFilterStringBearer
    : IStringBearer, IMoldSupportedValue<StringBuilder?[]?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchListWhenPopulatedWithFilterStringBearer<T, TFilterBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<T>?>, ISupportsOrderedCollectionPredicate<TFilterBase>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldObjectListWhenPopulatedWithFilterStringBearer : IStringBearer, IMoldSupportedValue<IReadOnlyList<object?>?>
  , ISupportsOrderedCollectionPredicate<object>, ISupportsValueFormatString
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
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }
}
