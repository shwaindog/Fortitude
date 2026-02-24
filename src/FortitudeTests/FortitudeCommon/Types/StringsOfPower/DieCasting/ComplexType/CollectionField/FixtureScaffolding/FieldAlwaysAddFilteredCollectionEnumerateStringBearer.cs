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

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsStruct |
                  SupportsValueFormatString)]
public class FieldBoolEnumerableAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool, IEnumerable<bool>>
  
{
    public IEnumerable<bool>? ComplexTypeCollectionFieldAlwaysAddFilteredBoolEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddFilteredBoolEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolEnumerableAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool?, IEnumerable<bool?>>
  
{
    public IEnumerable<bool?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredEnumerateNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableEnumerableAlwaysAddFilteredStringBearer<TFmt, TFmtBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmt, TFmtBase, IEnumerable<TFmt>>
    where TFmt : ISpanFormattable?, TFmtBase?
{
    public IEnumerable<TFmt>? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .AlwaysAddFilteredEnumerate<IEnumerable<TFmt>?, TFmt, TFmtBase>
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumerableAlwaysAddFilteredStringBearer<TFmtStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmtStruct?, IEnumerable<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredEnumerateNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyExceptNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerEnumerableAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TRevealBase> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloaked, TCloakedFilterBase, TRevealBase, IEnumerable<TCloaked>?>
    where TCloaked : TRevealBase?, TCloakedFilterBase?
    where TRevealBase : notnull
{
    public IEnumerable<TCloaked>? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate<IEnumerable<TCloaked>?, TCloaked, TCloakedFilterBase, TRevealBase>
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerEnumerable, ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerEnumerableAlwaysAddFilteredStringBearer<TCloakedStruct> :
    RevealerFilteredCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, IEnumerable<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerateNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerEnumerable
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerEnumerableAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearer, TBearerBase, IEnumerable<TBearer>> where TBearer : IStringBearer?, TBearerBase?
{
    public IEnumerable<TBearer>? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate<IEnumerable<TBearer>?, TBearer, TBearerBase>
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerEnumerableAlwaysAddFilteredStringBearer<TBearerStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearerStruct?, IEnumerable<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerateNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsString 
                | AcceptsClass | AcceptsNullableClass  | SupportsValueFormatString)]
public class FieldStringEnumerableAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<string?, IEnumerable<string?>>
  
{
    public IEnumerable<string?>? ComplexTypeCollectionFieldAlwaysAddFilteredStringEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsCharSequence 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceEnumerableAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TCharSeq, TCharSeqFilterBase, IEnumerable<TCharSeq>>
    where TCharSeq : ICharSequence?, TCharSeqFilterBase?
{
    public IEnumerable<TCharSeq>? ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeqEnumerate<IEnumerable<TCharSeq>?, TCharSeq, TCharSeqFilterBase>
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsStringBuilder 
    | AcceptsClass | AcceptsNullableClass  | SupportsValueFormatString)]
public class FieldStringBuilderEnumerableAlwaysAddFilteredStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder?, IEnumerable<StringBuilder?>>
{
    public IEnumerable<StringBuilder?>? ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyGeneric
                | CallsViaMatch | SupportsValueFormatString)]
public class FieldMatchEnumerableAlwaysAddFilteredStringBearer<TAny, TAnyFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TAny, TAnyFilterBase, IEnumerable<TAny>>
    where TAny : TAnyFilterBase
{
    public IEnumerable<TAny>? ComplexTypeCollectionFieldAlwaysAddFilteredMatchEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatchEnumerate<IEnumerable<TAny>?, TAny, TAnyFilterBase>
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddFilteredMatchEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsNullableObject
                | SupportsValueFormatString)]
public class FieldObjectEnumerableAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<object?, IEnumerable<object?>>
  
{
    public IEnumerable<object?>? ComplexTypeCollectionFieldAlwaysAddFilteredObjectEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObjectEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddFilteredObjectEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags);
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolEnumeratorAlwaysAddFilteredStringBearer : FormattedFilteredEnumeratorFieldMoldScaffold<bool, IEnumerator<bool>>
  
{
    public IEnumerator<bool>? ComplexTypeCollectionFieldAlwaysAddFilteredBoolEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredIterate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddFilteredBoolEnumerator, ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolEnumeratorAlwaysAddFilteredStringBearer : FormattedFilteredEnumeratorFieldMoldScaffold<bool?, IEnumerator<bool?>>
  
{
    public IEnumerator<bool?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredIterateNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolEnumerator, ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableEnumeratorAlwaysAddFilteredStringBearer<TFmt, TFmtBase>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TFmt, TFmtBase, IEnumerator<TFmt>>
    where TFmt : ISpanFormattable?, TFmtBase?
{
    public IEnumerator<TFmt>? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .AlwaysAddFilteredIterate<IEnumerator<TFmt>?, TFmt, TFmtBase>
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumeratorAlwaysAddFilteredStringBearer<TFmtStruct>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TFmtStruct?, IEnumerator<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredIterateNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsAnyExceptNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerEnumeratorAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TRevealBase> : 
    RevealerFilteredEnumeratorFieldMoldScaffold<TCloaked, TCloakedFilterBase, TRevealBase, IEnumerator<TCloaked>?>
    where TCloaked : TRevealBase, TCloakedFilterBase
    where TRevealBase : notnull
{
    public IEnumerator<TCloaked>? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredIterate<IEnumerator<TCloaked>?, TCloaked, TCloakedFilterBase, TRevealBase>
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerEnumerator, ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsAnyNullableStruct
                | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerEnumeratorAlwaysAddFilteredStringBearer<TCloakedStruct> :
    RevealerFilteredEnumeratorFieldMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, IEnumerator<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredIterateNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerEnumerator
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AlwaysWrites | AcceptsEnumerator | FilterPredicate | AcceptsTypeAllButNullableStruct
                | AcceptsNullableClass | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerEnumeratorAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TBearer, TBearerBase, IEnumerator<TBearer>>
    where TBearer : IStringBearer?, TBearerBase?
{
    public IEnumerator<TBearer>? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredIterate<IEnumerator<TBearer>?, TBearer, TBearerBase>
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerEnumeratorAlwaysAddFilteredStringBearer<TBearerStruct>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TBearerStruct?, IEnumerator<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredIterateNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsString 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringEnumeratorAlwaysAddFilteredStringBearer : FormattedFilteredEnumeratorFieldMoldScaffold<string?, IEnumerator<string?>>
  
{
    public IEnumerator<string?>? ComplexTypeCollectionFieldAlwaysAddFilteredStringEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredIterate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsCharSequence 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceEnumeratorAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TCharSeq, TCharSeqFilterBase, IEnumerator<TCharSeq>>
    where TCharSeq : ICharSequence?, TCharSeqFilterBase?
{
    public IEnumerator<TCharSeq>? ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeqIterate<IEnumerator<TCharSeq>?, TCharSeq, TCharSeqFilterBase>
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsStringBuilder 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderEnumeratorAlwaysAddFilteredStringBearer
    : FormattedFilteredEnumeratorFieldMoldScaffold<StringBuilder?, IEnumerator<StringBuilder?>>
{
    public IEnumerator<StringBuilder?>? ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredIterate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsAnyGeneric 
                 | SupportsValueFormatString)]
public class FieldMatchEnumeratorAlwaysAddFilteredStringBearer<TAny, TAnyFilterBase>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TAny, TAnyFilterBase, IEnumerator<TAny>>
    where TAny : TAnyFilterBase
{
    public IEnumerator<TAny>? ComplexTypeCollectionFieldAlwaysAddFilteredMatchEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatchIterate<IEnumerator<TAny>?, TAny, TAnyFilterBase>
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddFilteredMatchEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsNullableClass |
                  SupportsValueFormatString)]
public class FieldObjectEnumeratorAlwaysAddFilteredStringBearer : FormattedFilteredEnumeratorFieldMoldScaffold<object?, IEnumerator<object?>>
  
{
    public IEnumerator<object?>? ComplexTypeCollectionFieldAlwaysAddFilteredObjectEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObjectIterate
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddFilteredObjectEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags);
}
