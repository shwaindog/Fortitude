using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    OrderedCollectionFields;

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStruct)]
public class FieldBoolEnumerableWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<IEnumerable<bool>>
  , ISupportsOrderedCollectionPredicate<bool>
{
    public IEnumerable<bool>? ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList
    {
        get => Value;
        set => Value = value!;
    }

    public IEnumerable<bool> Value { get; set; } = null!;

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class FieldNullableBoolEnumerableWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<IEnumerable<bool?>?>
  , ISupportsOrderedCollectionPredicate<bool?>
{
    public IEnumerable<bool?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<bool?>? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableEnumerableWhenNonNullAddFilteredStringBearer<TFmt, TFmtBase>
    : IStringBearer, IMoldSupportedValue<IEnumerable<TFmt?>?>, ISupportsOrderedCollectionPredicate<TFmtBase>, ISupportsValueFormatString
    where TFmt : ISpanFormattable, TFmtBase
{
    public IEnumerable<TFmt?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TFmt?>? Value { get; set; }

    public OrderedCollectionPredicate<TFmtBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmtBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumerableWhenNonNullAddFilteredStringBearer<TFmtStruct>
    : IStringBearer, IMoldSupportedValue<IEnumerable<TFmtStruct?>?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TFmtStruct?>? Value { get; set; }

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList
              , ElementPredicate)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerEnumerableWhenNonNullAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IStringBearer, IMoldSupportedValue<IEnumerable<TCloaked>?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase> where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public IEnumerable<TCloaked>? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> ValueRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList, ElementPredicate
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable
                | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer)]
public class FieldNullableCloakedBearerEnumerableWhenNonNullAddFilteredStringBearer<TCloakedStruct> : IStringBearer
  , IMoldSupportedValue<IEnumerable<TCloakedStruct?>?>, ISupportsValueRevealer<TCloakedStruct>
  , ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TCloakedStruct?>? Value { get; set; }


    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsClass 
                | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerEnumerableWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>
    : IStringBearer, IMoldSupportedValue<IEnumerable<TBearer?>?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : IStringBearer, TBearerBase
{
    public IEnumerable<TBearer?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TBearer?>? Value { get; set; }

    public OrderedCollectionPredicate<TBearerBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsChars)]
public class FieldNullableStringBearerEnumerableWhenNonNullAddFilteredStringBearer<TBearerStruct>
    : IStringBearer, IMoldSupportedValue<IEnumerable<TBearerStruct?>?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TBearerStruct?>? Value { get; set; }

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringEnumerableWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<IEnumerable<string?>?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public IEnumerable<string?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<string?>? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceEnumerableWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<IEnumerable<TCharSeq?>?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>
      , ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IEnumerable<TCharSeq?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TCharSeq?>? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderEnumerableWhenNonNullAddFilteredStringBearer
    : IStringBearer, IMoldSupportedValue<IEnumerable<StringBuilder?>?>, ISupportsOrderedCollectionPredicate<StringBuilder>, ISupportsValueFormatString
{
    public IEnumerable<StringBuilder?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<StringBuilder?>? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchEnumerableWhenNonNullAddFilteredStringBearer<TAny, TAnyBase>
    : IStringBearer, IMoldSupportedValue<IEnumerable<TAny>?>, ISupportsOrderedCollectionPredicate<TAnyBase>, ISupportsValueFormatString
    where TAny : TAnyBase
{
    public IEnumerable<TAny>? ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TAny>? Value { get; set; }

    public OrderedCollectionPredicate<TAnyBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TAnyBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldObjectEnumerableWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<IEnumerable<object?>?>
  , ISupportsOrderedCollectionPredicate<object>, ISupportsValueFormatString
{
    public IEnumerable<object?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<object?>? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStruct)]
public class FieldBoolEnumeratorWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<IEnumerator<bool>>
  , ISupportsOrderedCollectionPredicate<bool>
{
    public IEnumerator<bool>? ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList
    {
        get => Value;
        set => Value = value!;
    }

    public IEnumerator<bool> Value { get; set; } = null!;

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class FieldNullableBoolEnumeratorWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<IEnumerator<bool?>?>
  , ISupportsOrderedCollectionPredicate<bool?>
{
    public IEnumerator<bool?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<bool?>? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableEnumeratorWhenNonNullAddFilteredStringBearer<TFmt, TFmtBase>
    : IStringBearer, IMoldSupportedValue<IEnumerator<TFmt?>?>, ISupportsOrderedCollectionPredicate<TFmtBase>, ISupportsValueFormatString
    where TFmt : ISpanFormattable, TFmtBase
{
    public IEnumerator<TFmt?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TFmt?>? Value { get; set; }

    public OrderedCollectionPredicate<TFmtBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmtBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumeratorWhenNonNullAddFilteredStringBearer<TFmtStruct>
    : IStringBearer, IMoldSupportedValue<IEnumerator<TFmtStruct?>?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TFmtStruct?>? Value { get; set; }

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList
              , ElementPredicate)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerEnumeratorWhenNonNullAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IStringBearer, IMoldSupportedValue<IEnumerator<TCloaked>?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase> where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public IEnumerator<TCloaked>? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> ValueRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList, ElementPredicate
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable
                | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer)]
public class FieldNullableCloakedBearerEnumeratorWhenNonNullAddFilteredStringBearer<TCloakedStruct> : IStringBearer
  , IMoldSupportedValue<IEnumerator<TCloakedStruct?>?>, ISupportsValueRevealer<TCloakedStruct>
  , ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TCloakedStruct?>? Value { get; set; }


    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStruct | AcceptsClass 
                | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerEnumeratorWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>
    : IStringBearer, IMoldSupportedValue<IEnumerator<TBearer?>?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : IStringBearer, TBearerBase
{
    public IEnumerator<TBearer?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TBearer?>? Value { get; set; }

    public OrderedCollectionPredicate<TBearerBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsChars)]
public class FieldNullableStringBearerEnumeratorWhenNonNullAddFilteredStringBearer<TBearerStruct>
    : IStringBearer, IMoldSupportedValue<IEnumerator<TBearerStruct?>?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TBearerStruct?>? Value { get; set; }

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringEnumeratorWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<IEnumerator<string?>?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public IEnumerator<string?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<string?>? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldCharSequenceEnumeratorWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : IStringBearer, IMoldSupportedValue<IEnumerator<TCharSeq?>?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>
      , ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IEnumerator<TCharSeq?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TCharSeq?>? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsChars | SupportsValueFormatString)]
public class FieldStringBuilderEnumeratorWhenNonNullAddFilteredStringBearer
    : IStringBearer, IMoldSupportedValue<IEnumerator<StringBuilder?>?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public IEnumerator<StringBuilder?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<StringBuilder?>? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAny | SupportsValueFormatString)]
public class FieldMatchEnumeratorWhenNonNullAddFilteredStringBearer<TAny, TAnyBase>
    : IStringBearer, IMoldSupportedValue<IEnumerator<TAny>?>, ISupportsOrderedCollectionPredicate<TAnyBase>, ISupportsValueFormatString
    where TAny : TAnyBase
{
    public IEnumerator<TAny>? ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TAny>? Value { get; set; }

    public OrderedCollectionPredicate<TAnyBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TAnyBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldObjectEnumeratorWhenNonNullAddFilteredStringBearer : IStringBearer, IMoldSupportedValue<IEnumerator<object?>?>
  , ISupportsOrderedCollectionPredicate<object>, ISupportsValueFormatString
{
    public IEnumerator<object?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<object?>? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }
}