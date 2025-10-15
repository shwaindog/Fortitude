// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.OrderedCollectionFields;

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct)]
public class FieldBoolEnumerableWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<IEnumerable<bool>>
  , ISupportsOrderedCollectionPredicate<bool>
{
    public IEnumerable<bool>? ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolList
    {
        get => Value;
        set => Value = value!;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolList);
    public IEnumerable<bool> Value { get; set; } = null!;

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class FieldNullableBoolEnumerableWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<IEnumerable<bool?>?>
  , ISupportsOrderedCollectionPredicate<bool?>
{
    public IEnumerable<bool?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolList);
    public IEnumerable<bool?>? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableEnumerableWhenPopulatedWithFilterStringBearer<TFmt, TFmtBase>
    : IMoldSupportedValue<IEnumerable<TFmt?>?>, ISupportsOrderedCollectionPredicate<TFmtBase>, ISupportsValueFormatString
    where TFmt : ISpanFormattable, TFmtBase
{
    public IEnumerable<TFmt?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableList);
    public IEnumerable<TFmt?>? Value { get; set; }

    public OrderedCollectionPredicate<TFmtBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmtBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumerableWhenPopulatedWithFilterStringBearer<TFmtStruct>
    : IMoldSupportedValue<IEnumerable<TFmtStruct?>?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableList);
    public IEnumerable<TFmtStruct?>? Value { get; set; }

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableList
              , ElementPredicate)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerEnumerableWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IMoldSupportedValue<IEnumerable<TCloaked>?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase> where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public IEnumerable<TCloaked>? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList);
    public IEnumerable<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> ValueRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList, ElementPredicate
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable
                | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer)]
public class FieldNullableCloakedBearerEnumerableWhenPopulatedWithFilterStringBearer<TCloakedStruct> : 
  IMoldSupportedValue<IEnumerable<TCloakedStruct?>?>, ISupportsValueRevealer<TCloakedStruct>
  , ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList);
    public IEnumerable<TCloakedStruct?>? Value { get; set; }


    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass 
                | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerEnumerableWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : IMoldSupportedValue<IEnumerable<TBearer?>?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : IStringBearer, TBearerBase
{
    public IEnumerable<TBearer?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerList);
    public IEnumerable<TBearer?>? Value { get; set; }

    public OrderedCollectionPredicate<TBearerBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate)]
public class FieldNullableStringBearerEnumerableWhenPopulatedWithFilterStringBearer<TBearerStruct>
    : IMoldSupportedValue<IEnumerable<TBearerStruct?>?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerList);
    public IEnumerable<TBearerStruct?>? Value { get; set; }

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsString | SupportsValueFormatString)]
public class FieldStringEnumerableWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<IEnumerable<string?>?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public IEnumerable<string?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringList);
    public IEnumerable<string?>? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceEnumerableWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : IMoldSupportedValue<IEnumerable<TCharSeq?>?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>
      , ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IEnumerable<TCharSeq?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceList);
    public IEnumerable<TCharSeq?>? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderEnumerableWhenPopulatedWithFilterStringBearer
    : IMoldSupportedValue<IEnumerable<StringBuilder?>?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public IEnumerable<StringBuilder?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderList);
    public IEnumerable<StringBuilder?>? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchEnumerableWhenPopulatedWithFilterStringBearer<TAny, TAnyBase>
    : IMoldSupportedValue<IEnumerable<TAny>?>, ISupportsOrderedCollectionPredicate<TAnyBase>, ISupportsValueFormatString
    where TAny : TAnyBase
{
    public IEnumerable<TAny>? ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchList);
    public IEnumerable<TAny>? Value { get; set; }

    public OrderedCollectionPredicate<TAnyBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TAnyBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldObjectEnumerableWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<IEnumerable<object?>?>
  , ISupportsOrderedCollectionPredicate<object>, ISupportsValueFormatString
{
    public IEnumerable<object?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectList);
    public IEnumerable<object?>? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectList
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct)]
public class FieldBoolEnumeratorWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<IEnumerator<bool>>
  , ISupportsOrderedCollectionPredicate<bool>
{
    public IEnumerator<bool>? ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolList
    {
        get => Value;
        set => Value = value!;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolList);
    public IEnumerator<bool> Value { get; set; } = null!;

    public OrderedCollectionPredicate<bool> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsNullableStruct)]
public class FieldNullableBoolEnumeratorWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<IEnumerator<bool?>?>
  , ISupportsOrderedCollectionPredicate<bool?>
{
    public IEnumerator<bool?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolList);
    public IEnumerator<bool?>? Value { get; set; }

    public OrderedCollectionPredicate<bool?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<bool?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldSpanFormattableEnumeratorWhenPopulatedWithFilterStringBearer<TFmt, TFmtBase>
    : IMoldSupportedValue<IEnumerator<TFmt?>?>, ISupportsOrderedCollectionPredicate<TFmtBase>, ISupportsValueFormatString
    where TFmt : ISpanFormattable, TFmtBase
{
    public IEnumerator<TFmt?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableList);
    public IEnumerator<TFmt?>? Value { get; set; }

    public OrderedCollectionPredicate<TFmtBase> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<TFmtBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumeratorWhenPopulatedWithFilterStringBearer<TFmtStruct>
    : IMoldSupportedValue<IEnumerator<TFmtStruct?>?>, ISupportsOrderedCollectionPredicate<TFmtStruct?>, ISupportsValueFormatString
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableList);
    public IEnumerator<TFmtStruct?>? Value { get; set; }

    public OrderedCollectionPredicate<TFmtStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TFmtStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableList
              , ElementPredicate)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  SupportsValueRevealer)]
public class FieldCloakedBearerEnumeratorWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : IMoldSupportedValue<IEnumerator<TCloaked>?>, ISupportsValueRevealer<TCloakedRevealBase>
      , ISupportsOrderedCollectionPredicate<TCloakedFilterBase> where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public IEnumerator<TCloaked>? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList);
    public IEnumerator<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedRevealBase> ValueRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedFilterBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList, ElementPredicate
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct | AcceptsSpanFormattable
                | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | SupportsValueRevealer)]
public class FieldNullableCloakedBearerEnumeratorWhenPopulatedWithFilterStringBearer<TCloakedStruct> : 
  IMoldSupportedValue<IEnumerator<TCloakedStruct?>?>, ISupportsValueRevealer<TCloakedStruct>
  , ISupportsOrderedCollectionPredicate<TCloakedStruct?> where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList);
    public IEnumerator<TCloakedStruct?>? Value { get; set; }


    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public OrderedCollectionPredicate<TCloakedStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCloakedStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct | AcceptsClass 
                | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerEnumeratorWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : IMoldSupportedValue<IEnumerator<TBearer?>?>, ISupportsOrderedCollectionPredicate<TBearerBase?>
    where TBearer : IStringBearer, TBearerBase
{
    public IEnumerator<TBearer?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerList);
    public IEnumerator<TBearer?>? Value { get; set; }

    public OrderedCollectionPredicate<TBearerBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate)]
public class FieldNullableStringBearerEnumeratorWhenPopulatedWithFilterStringBearer<TBearerStruct>
    : IMoldSupportedValue<IEnumerator<TBearerStruct?>?>, ISupportsOrderedCollectionPredicate<TBearerStruct?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerList);
    public IEnumerator<TBearerStruct?>? Value { get; set; }

    public OrderedCollectionPredicate<TBearerStruct?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TBearerStruct?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsString | SupportsValueFormatString)]
public class FieldStringEnumeratorWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<IEnumerator<string?>?>
  , ISupportsOrderedCollectionPredicate<string?>, ISupportsValueFormatString
{
    public IEnumerator<string?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringList);
    public IEnumerator<string?>? Value { get; set; }

    public OrderedCollectionPredicate<string?> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<string?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceEnumeratorWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : IMoldSupportedValue<IEnumerator<TCharSeq?>?>, ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>
      , ISupportsValueFormatString
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IEnumerator<TCharSeq?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceList);
    public IEnumerator<TCharSeq?>? Value { get; set; }

    public OrderedCollectionPredicate<TCharSeqFilterBase?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TCharSeqFilterBase?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderEnumeratorWhenPopulatedWithFilterStringBearer
    : IMoldSupportedValue<IEnumerator<StringBuilder?>?>, ISupportsOrderedCollectionPredicate<StringBuilder?>, ISupportsValueFormatString
{
    public IEnumerator<StringBuilder?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderList);
    public IEnumerator<StringBuilder?>? Value { get; set; }

    public OrderedCollectionPredicate<StringBuilder?> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<StringBuilder?>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchEnumeratorWhenPopulatedWithFilterStringBearer<TAny, TAnyBase>
    : IMoldSupportedValue<IEnumerator<TAny>?>, ISupportsOrderedCollectionPredicate<TAnyBase>, ISupportsValueFormatString
    where TAny : TAnyBase
{
    public IEnumerator<TAny>? ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchList);
    public IEnumerator<TAny>? Value { get; set; }

    public OrderedCollectionPredicate<TAnyBase> ElementPredicate { get; set; }
        = ISupportsOrderedCollectionPredicate<TAnyBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchList
              , ElementPredicate, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldObjectEnumeratorWhenPopulatedWithFilterStringBearer : IMoldSupportedValue<IEnumerator<object?>?>
  , ISupportsOrderedCollectionPredicate<object>, ISupportsValueFormatString
{
    public IEnumerator<object?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectList
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectList);
    public IEnumerator<object?>? Value { get; set; }

    public OrderedCollectionPredicate<object> ElementPredicate { get; set; } = ISupportsOrderedCollectionPredicate<object>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectList
              , ElementPredicate, ValueFormatString);

    public string? ValueFormatString { get; set; }
}