using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField.FixtureScaffolding;

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolSpanWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool, bool[]>
  
{
    public bool[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolSpanWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool?, bool?[]>
  
{
    public bool?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableSpanWhenPopulatedWithFilterStringBearer<TFmt, TFmtBase> : 
    FormattedFilteredCollectionFieldMoldScaffold<TFmt, TFmtBase?, TFmt[]>
   where TFmt : ISpanFormattable?, TFmtBase?
{
    public TFmt[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassSpanWhenPopulatedWithFilterStringBearer<TFmt, TFmtBase> : 
    FormattedFilteredCollectionFieldMoldScaffold<TFmt?, TFmtBase?, TFmt?[]>
   where TFmt : class, ISpanFormattable, TFmtBase
{
    public TFmt?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableSpanWhenPopulatedWithFilterStringBearer<TFmtStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmtStruct?, TFmtStruct?[]>
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate 
        | AcceptsTypeAllButNullableStruct | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerSpanWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloaked, TCloakedFilterBase, TCloakedRevealBase, TCloaked[]?>
    where TCloaked : TCloakedRevealBase?, TCloakedFilterBase?
    where TCloakedRevealBase : notnull
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerSpan);
    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerSpan.AsSpan()
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsAnyNullableClass | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerNullableClassSpanWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase> : 
  RevealerFilteredCollectionFieldMoldScaffold<TCloaked, TCloakedFilterBase, TCloakedRevealBase, TCloaked[]>
    where TCloaked : class?, TCloakedRevealBase?, TCloakedFilterBase?
    where TCloakedRevealBase : notnull
{
    public TCloaked?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableSpan.AsSpan()
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsAnyNullableStruct | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerSpanWhenPopulatedWithFilterStringBearer<TCloakedStruct> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerSpan.AsSpan()
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerSpanWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearer, TBearerBase?, TBearer[]>
    where TBearer : IStringBearer?, TBearerBase?
{
    public TBearer[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsNullableClass | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerNullableClassSpanWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearer, TBearerBase, TBearer[]>
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerNullableSpan
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerNullableSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerNullableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerNullableSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsNullableStruct | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerSpanWhenPopulatedWithFilterStringBearer<TBearerStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsString | AcceptsClass | SupportsValueFormatString)]
public class FieldStringSpanWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<string, string[]>
  
{
    public string[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsString | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringNullableSpanWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<string?, string?[]>
  
{
    public string?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenPopulatedWithFilterNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate 
                 | AcceptsCharSequence | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceSpanWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TCharSeq, TCharSeqFilterBase?, TCharSeq[]>
    where TCharSeq : ICharSequence?, TCharSeqFilterBase?
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsStringBuilder | AcceptsClass | SupportsValueFormatString)]
public class FieldStringBuilderSpanWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder, StringBuilder[]>
  
{
    public StringBuilder[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsStringBuilder | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderNullableSpanWhenPopulatedWithFilterStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder?, StringBuilder?[]>
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchSpanWhenPopulatedWithFilterStringBearer<TAny, TAnyFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TAny, TAnyFilterBase?, TAny[]>
    where TAny : TAnyFilterBase?
{
    public TAny[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsNonNullableObject | SupportsValueFormatString)]
public class FieldObjectSpanWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<object, object[]>
  
{
    public object[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags);
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsNullableObject | SupportsValueFormatString)]
public class FieldNullableObjectSpanWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<object?, object?[]>
  
{
    public object?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObjectNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectSpan)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags);
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolReadOnlySpanWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool, bool[]>
  
{
    public bool[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolReadOnlySpan)
              , (ReadOnlySpan<bool>)ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolReadOnlySpanWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool?, bool?[]>
  
{
    public bool?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolReadOnlySpan)
              , (ReadOnlySpan<bool?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableReadOnlySpanWhenPopulatedWithFilterStringBearer<TFmt, TFmtBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmt, TFmtBase?, TFmt[]>
    where TFmt : ISpanFormattable?, TFmtBase?
{
    public TFmt[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmt>)ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassReadOnlySpanWhenPopulatedWithFilterStringBearer<TFmt, TFmtBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmt?, TFmtBase?, TFmt?[]>
    where TFmt : class, ISpanFormattable, TFmtBase
{
    public TFmt?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableReadOnlySpan)
              , (ReadOnlySpan<TFmt?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableNullableReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableReadOnlySpanWhenPopulatedWithFilterStringBearer<TFmtStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmtStruct?, TFmtStruct?[]>
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmtStruct?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsTypeAllButNullableStruct | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerReadOnlySpanWhenPopulatedWithFilterStringBearer<TCloaked, TFilterBase, TRevealBase>
    : RevealerFilteredCollectionFieldMoldScaffold<TCloaked, TFilterBase, TRevealBase, TCloaked[]?>
    where TCloaked : TRevealBase?, TFilterBase?
    where TRevealBase : notnull
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloaked>)ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerReadOnlySpan
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsAnyNullableClass | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerNullableClassReadOnlySpanWhenPopulatedWithFilterStringBearer<TCloaked, TFilterBase, TRevealBase> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloaked, TFilterBase, TRevealBase, TCloaked[]>
    where TCloaked : class, TRevealBase, TFilterBase
    where TRevealBase : notnull
{
    public TCloaked?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableReadOnlySpan
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableReadOnlySpan)
              , (ReadOnlySpan<TCloaked?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerNullableReadOnlySpan
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsAnyNullableStruct | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerReadOnlySpanWhenPopulatedWithFilterStringBearer<TCloakedStruct> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]?>
       where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloakedStruct?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerReadOnlySpan
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerReadOnlySpanWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearer, TBearerBase?, TBearer[]>
    where TBearer : IStringBearer?, TBearerBase?
{
    public TBearer[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearer>)ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsNullableClass | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerNullableClassReadOnlySpanWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearer, TBearerBase, TBearer[]>
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerNullableReadOnlySpan
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerNullableReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerNullableReadOnlySpan)
              , (ReadOnlySpan<TBearer?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerNullableReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsNullableStruct | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerReadOnlySpanWhenPopulatedWithFilterStringBearer<TBearerStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerReadOnlySpan),
                (ReadOnlySpan<TBearerStruct?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsString | AcceptsClass | SupportsValueFormatString)]
public class FieldStringReadOnlySpanWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<string, string[]>
  
{
    public string[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringReadOnlySpan)
              , (ReadOnlySpan<string>)ComplexTypeCollectionFieldWhenPopulatedWithFilterStringReadOnlySpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsString | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringNullableReadOnlySpanWhenPopulatedWithFilterStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<string?, string?[]>
{
    public string?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringReadOnlySpan),
                (ReadOnlySpan<string?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsCharSequence | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceReadOnlySpanWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TCharSeq, TCharSeqFilterBase?, TCharSeq[]>
    where TCharSeq : ICharSequence?, TCharSeqFilterBase?
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceReadOnlySpan)
              , (ReadOnlySpan<TCharSeq>)ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsStringBuilder | AcceptsClass | SupportsValueFormatString)]
public class FieldStringBuilderReadOnlySpanWhenPopulatedWithFilterStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder, StringBuilder[]>
{
    public StringBuilder[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderReadOnlySpan)
              , (ReadOnlySpan<StringBuilder>)ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsStringBuilder | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderNullableReadOnlySpanWhenPopulatedWithFilterStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder?, StringBuilder?[]>
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderReadOnlySpan)
              , (ReadOnlySpan<StringBuilder?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBuilderReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchReadOnlySpanWhenPopulatedWithFilterStringBearer<TAny, TAnyFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TAny, TAnyFilterBase?, TAny[]>
    where TAny : TAnyFilterBase?
{
    public TAny[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchReadOnlySpan)
              , (ReadOnlySpan<TAny>)ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsNonNullableObject | SupportsValueFormatString)]
public class FieldObjectReadOnlySpanWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<object, object[]>
{
    public object[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectReadOnlySpan)
              , (ReadOnlySpan<object>)ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags);
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsNullableObject | SupportsValueFormatString)]
public class FieldNullableObjectReadOnlySpanWhenPopulatedWithFilterStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<object?, object?[]>
{
    public object?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObjectNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectReadOnlySpan)
              , (ReadOnlySpan<object?>)ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableObjectReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags);
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolArrayWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool, bool[]>
  
{
    public bool[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolArrayWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool?, bool?[]>
  
{
    public bool?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableArrayWhenPopulatedWithFilterStringBearer<TFmt, TFmtBase> : 
    FormattedFilteredCollectionFieldMoldScaffold<TFmt, TFmtBase, TFmt[]>
   where TFmt : ISpanFormattable, TFmtBase
{
    public TFmt?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableArray
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableArrayWhenPopulatedWithFilterStringBearer<TFmtStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmtStruct?, TFmtStruct?[]>
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerArrayWhenPopulatedWithFilterStringBearer<TCloaked, TFilterBase, TRevealBase> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloaked, TFilterBase, TRevealBase, TCloaked[]?>
       where TCloaked : TRevealBase?, TFilterBase?
       where TRevealBase : notnull
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerArray
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsAnyNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerArrayWhenPopulatedWithFilterStringBearer<TCloakedStruct> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerArray
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerArrayWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearer, TBearerBase, TBearer[]>
    where TBearer : IStringBearer?, TBearerBase?
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerArray
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerArrayWhenPopulatedWithFilterStringBearer<TBearerStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsString 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringArrayWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<string?, string?[]>
  
{
    public string?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsCharSequence 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceArrayWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TCharSeq, TCharSeqFilterBase, TCharSeq[]>
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceArray
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStringBuilder 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderArrayWhenPopulatedWithFilterStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder?, StringBuilder?[]>
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsAnyGeneric 
                | SupportsValueFormatString)]
public class FieldMatchArrayWhenPopulatedWithFilterStringBearer<TAny, TAnyFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TAny, TAnyFilterBase, TAny[]>
    where TAny : TAnyFilterBase
{
    public TAny[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenPopulatedWithFilterMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableObject
                     | SupportsValueFormatString)]
public class FieldObjectArrayWhenPopulatedWithFilterStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<object?, object?[]>
{
    public object?[]? ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectArray)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags);
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolListWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool, IReadOnlyList<bool>>
  
{
    public IReadOnlyList<bool>? ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolList
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterBoolList
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolListWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool?, IReadOnlyList<bool?>>
  
{
    public IReadOnlyList<bool?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableBoolList
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableListWhenPopulatedWithFilterStringBearer<TFmt, TFmtBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmt, TFmtBase, IReadOnlyList<TFmt>>
    where TFmt : ISpanFormattable, TFmtBase
{
    public IReadOnlyList<TFmt?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableList
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterSpanFormattableList
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableListWhenPopulatedWithFilterStringBearer<TFmtStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmtStruct?, IReadOnlyList<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableSpanFormattableList
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerListWhenPopulatedWithFilterStringBearer<TCloaked, TFilterBase, TRevealBase> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloaked?, TFilterBase?, TRevealBase, IReadOnlyList<TCloaked?>?>
       where TCloaked : TRevealBase, TFilterBase
       where TRevealBase : notnull
{
    public IReadOnlyList<TCloaked?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCloakedBearerList, ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsAnyNullableStruct 
                | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerListWhenPopulatedWithFilterStringBearer<TCloakedStruct> : 
  RevealerFilteredCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, IReadOnlyList<TCloakedStruct?>?>
   where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableCloakedBearerList
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate 
                | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerListWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearer, TBearerBase, IReadOnlyList<TBearer>>
    where TBearer : IStringBearer?, TBearerBase?
{
    public IReadOnlyList<TBearer>? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerList
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBearerList
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableStruct
                     | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerListWhenPopulatedWithFilterStringBearer<TBearerStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearerStruct?, IReadOnlyList<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterNullableStringBearerList
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsString 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringListWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<string?, IReadOnlyList<string?>>
  
{
    public IReadOnlyList<string?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringList
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsCharSequence 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceListWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TCharSeq, TCharSeqFilterBase, IReadOnlyList<TCharSeq>>
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IReadOnlyList<TCharSeq?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceList
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterCharSequenceList
              , ElementPredicate!
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsStringBuilder 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderListWhenPopulatedWithFilterStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder?, IReadOnlyList<StringBuilder?>>
{
    public IReadOnlyList<StringBuilder?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterStringBuilderList
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsAnyGeneric 
                | SupportsValueFormatString)]
public class FieldMatchListWhenPopulatedWithFilterStringBearer<TAny, TAnyFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TAny, TAnyFilterBase, IReadOnlyList<TAny>>
    where TAny : TAnyFilterBase
{
    public IReadOnlyList<TAny>? ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterMatchList
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | FilterPredicate | AcceptsNullableObject 
                | SupportsValueFormatString)]
public class FieldObjectListWhenPopulatedWithFilterStringBearer : FormattedFilteredCollectionFieldMoldScaffold<object?, IReadOnlyList<object?>>
  
{
    public IReadOnlyList<object?>? ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectList)
              , ComplexTypeCollectionFieldWhenPopulatedWithFilterObjectList
              , ElementPredicate
              , ValueFormatString, FormattingFlags);
}
