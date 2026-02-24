// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField.FixtureScaffolding;

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolEnumerableWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool, IEnumerable<bool>>
  
{
    public IEnumerable<bool>? ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolEnumerableWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool?, IEnumerable<bool?>>
  
{
    public IEnumerable<bool?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableEnumerableWhenPopulatedWithFilterStringBearer<TFmt, TFmtBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmt, TFmtBase, IEnumerable<TFmt>>
    where TFmt : ISpanFormattable, TFmtBase
{
    public IEnumerable<TFmt?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | FilterPredicate
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumerableWhenPopulatedWithFilterStringBearer<TFmtStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmtStruct?, IEnumerable<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | FilterPredicate
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerEnumerableWhenPopulatedWithFilterStringBearer<TCloaked, TFilterBase, TRevealBase> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloaked, TFilterBase, TRevealBase, IEnumerable<TCloaked>?>
    where TCloaked : TRevealBase, TFilterBase
    where TRevealBase : notnull
{
    public IEnumerable<TCloaked>? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate<IEnumerable<TCloaked>?, TCloaked, TFilterBase, TRevealBase>
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerEnumerable, ElementPredicate
              , ValueRevealer, ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | FilterPredicate
                | AcceptsAnyNullableStruct | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerEnumerableWhenPopulatedWithFilterStringBearer<TCloakedStruct> :
    RevealerFilteredCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, IEnumerable<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerateNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerEnumerable
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | FilterPredicate
                | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerEnumerableWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearer, TBearerBase, IEnumerable<TBearer>>
    where TBearer : IStringBearer?, TBearerBase?
{
    public IEnumerable<TBearer>? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate<IEnumerable<TBearer>?, TBearer, TBearerBase>
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerEnumerableWhenPopulatedWithFilterStringBearer<TBearerStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearerStruct?, IEnumerable<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerateNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | FilterPredicate | AcceptsString
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringEnumerableWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<string?, IEnumerable<string?>>
  
{
    public IEnumerable<string?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | FilterPredicate | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceEnumerableWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TCharSeq, TCharSeqFilterBase, IEnumerable<TCharSeq>>
    where TCharSeq : ICharSequence?, TCharSeqFilterBase?
    where TCharSeqFilterBase : notnull
{
    public IEnumerable<TCharSeq?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStringBuilder
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderEnumerableWhenPopulatedWithFilterStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder?, IEnumerable<StringBuilder?>>
{
    public IEnumerable<StringBuilder?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString)]
public class FieldMatchEnumerableWhenPopulatedWithFilterStringBearer<TAny, TAnyBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TAny, TAnyBase, IEnumerable<TAny>>
    where TAny : TAnyBase
{
    public IEnumerable<TAny>? ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableObject
                | SupportsValueFormatString)]
public class FieldObjectEnumerableWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<object?, IEnumerable<object?>>
  
{
    public IEnumerable<object?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags);
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolEnumeratorWhenPopulatedWithFilterStringBearer : FormattedFilteredEnumeratorFieldMoldScaffold<bool, IEnumerator<bool>>
  
{
    public IEnumerator<bool>? ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolEnumeratorWhenPopulatedWithFilterStringBearer : FormattedFilteredEnumeratorFieldMoldScaffold<bool?, IEnumerator<bool?>>
  
{
    public IEnumerator<bool?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableEnumeratorWhenPopulatedWithFilterStringBearer<TFmt, TFmtBase>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TFmt, TFmtBase, IEnumerator<TFmt>>
    where TFmt : ISpanFormattable, TFmtBase
{
    public IEnumerator<TFmt?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | FilterPredicate
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumeratorWhenPopulatedWithFilterStringBearer<TFmtStruct>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TFmtStruct?, IEnumerator<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | FilterPredicate
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerEnumeratorWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TRevealBase> : 
    RevealerFilteredEnumeratorFieldMoldScaffold<TCloaked, TCloakedFilterBase, TRevealBase, IEnumerator<TCloaked>?>
    where TCloaked : TRevealBase?, TCloakedFilterBase?
    where TRevealBase : notnull
{
    public IEnumerator<TCloaked>? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredIterate<IEnumerator<TCloaked>?, TCloaked, TCloakedFilterBase, TRevealBase>
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerEnumerator, ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | FilterPredicate
                | AcceptsAnyNullableStruct | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerEnumeratorWhenPopulatedWithFilterStringBearer<TCloakedStruct> :
    RevealerFilteredEnumeratorFieldMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, IEnumerator<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredIterateNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerEnumerator
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | FilterPredicate
                | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerEnumeratorWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TBearer, TBearerBase, IEnumerator<TBearer>>
    where TBearer : IStringBearer, TBearerBase
{
    public IEnumerator<TBearer>? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredIterate<IEnumerator<TBearer>?, TBearer, TBearerBase>
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerEnumeratorWhenPopulatedWithFilterStringBearer<TBearerStruct>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TBearerStruct?, IEnumerator<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredIterateNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | FilterPredicate | AcceptsString
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringEnumeratorWhenPopulatedWithFilterStringBearer : FormattedFilteredEnumeratorFieldMoldScaffold<string?, IEnumerator<string?>>
  
{
    public IEnumerator<string?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | FilterPredicate | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceEnumeratorWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TCharSeq, TCharSeqFilterBase, IEnumerator<TCharSeq>>
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IEnumerator<TCharSeq?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStringBuilder
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderEnumeratorWhenPopulatedWithFilterStringBearer
    : FormattedFilteredEnumeratorFieldMoldScaffold<StringBuilder?, IEnumerator<StringBuilder?>>
{
    public IEnumerator<StringBuilder?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString)]
public class FieldMatchEnumeratorWhenPopulatedWithFilterStringBearer<TAny, TAnyBase>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TAny, TAnyBase, IEnumerator<TAny>>
    where TAny : TAnyBase
{
    public IEnumerator<TAny>? ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableClass
                | SupportsValueFormatString)]
public class FieldObjectEnumeratorWhenPopulatedWithFilterStringBearer : FormattedFilteredEnumeratorFieldMoldScaffold<object?, IEnumerator<object?>>
  
{
    public IEnumerator<object?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags);
}
