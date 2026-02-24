using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField.FixtureScaffolding;

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStruct 
                | SupportsValueFormatString)]
public class FieldBoolEnumerableWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool, IEnumerable<bool>>
  
{
    public IEnumerable<bool>? ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolEnumerable);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolEnumerableWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool?, IEnumerable<bool?>>
  
{
    public IEnumerable<bool?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolEnumerable);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerateNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate 
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableEnumerableWhenNonNullAddFilteredStringBearer<TFmt, TFmtBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmt, TFmtBase, IEnumerable<TFmt>>
    where TFmt : ISpanFormattable?, TFmtBase?
{
    public IEnumerable<TFmt>? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableEnumerable);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenNonNullAddFilteredEnumerate<IEnumerable<TFmt>?, TFmt, TFmtBase>
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate 
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumerableWhenNonNullAddFilteredStringBearer<TFmtStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmtStruct?, IEnumerable<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableEnumerable);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerateNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerEnumerableWhenNonNullAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TRevealBase> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloaked, TCloakedFilterBase, TRevealBase, IEnumerable<TCloaked>?>
       where TCloaked : TRevealBase?, TCloakedFilterBase?
       where TRevealBase : notnull
{
    public IEnumerable<TCloaked>? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerEnumerable);
    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFilteredEnumerate<IEnumerable<TCloaked>?, TCloaked, TCloakedFilterBase, TRevealBase>
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerEnumerable, ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyNullableStruct 
                | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerEnumerableWhenNonNullAddFilteredStringBearer<TCloakedStruct> : 
  RevealerFilteredCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, IEnumerable<TCloakedStruct?>?>
   where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerEnumerable);
    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFilteredEnumerateNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerEnumerable
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsTypeAllButNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerEnumerableWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearer, TBearerBase, IEnumerable<TBearer>>
    where TBearer : IStringBearer?, TBearerBase?
{
    public IEnumerable<TBearer>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerEnumerable);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFilteredEnumerate<IEnumerable<TBearer>?, TBearer, TBearerBase>
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerEnumerableWhenNonNullAddFilteredStringBearer<TBearerStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearerStruct?, IEnumerable<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerEnumerable);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFilteredEnumerateNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsString 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringEnumerableWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<string?, IEnumerable<string?>>
  
{
    public IEnumerable<string?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringEnumerable);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredStringEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsCharSequence 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceEnumerableWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TCharSeq, TCharSeqFilterBase, IEnumerable<TCharSeq>>
    where TCharSeq : ICharSequence?, TCharSeqFilterBase?
{
    public IEnumerable<TCharSeq>? ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceEnumerable);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeqEnumerate<IEnumerable<TCharSeq>?, TCharSeq, TCharSeqFilterBase>
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStringBuilder 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderEnumerableWhenNonNullAddFilteredStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder?, IEnumerable<StringBuilder?>>
{
    public IEnumerable<StringBuilder?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderEnumerable);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredStringBuilderEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyGeneric 
                | SupportsValueFormatString)]
public class FieldMatchEnumerableWhenNonNullAddFilteredStringBearer<TAny, TAnyBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TAny, TAnyBase, IEnumerable<TAny>>
    where TAny : TAnyBase?
{
    public IEnumerable<TAny>? ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchEnumerable);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredMatchEnumerate<IEnumerable<TAny>?, TAny, TAnyBase>
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableObject 
                | SupportsValueFormatString)]
public class FieldObjectEnumerableWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<object?, IEnumerable<object?>>
  
{
    public IEnumerable<object?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectEnumerable);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectEnumerable
              , ElementPredicate
              , ValueFormatString, FormattingFlags);

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStruct 
                | SupportsValueFormatString)]
public class FieldBoolEnumeratorWhenNonNullAddFilteredStringBearer : FormattedFilteredEnumeratorFieldMoldScaffold<bool, IEnumerator<bool>>
  
{
    public IEnumerator<bool>? ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolEnumerator);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredIterate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolEnumeratorWhenNonNullAddFilteredStringBearer : FormattedFilteredEnumeratorFieldMoldScaffold<bool?, IEnumerator<bool?>>
  
{
    public IEnumerator<bool?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolEnumerator);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredIterateNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate 
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableEnumeratorWhenNonNullAddFilteredStringBearer<TFmt, TFmtBase>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TFmt, TFmtBase, IEnumerator<TFmt>>
    where TFmt : ISpanFormattable?, TFmtBase?
{
    public IEnumerator<TFmt>? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableEnumerator);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenNonNullAddFilteredIterate<IEnumerator<TFmt>?, TFmt, TFmtBase>
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate 
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumeratorWhenNonNullAddFilteredStringBearer<TFmtStruct>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TFmtStruct?, IEnumerator<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableEnumerator);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredIterateNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerEnumeratorWhenNonNullAddFilteredStringBearer<TCloaked, TFilterBase, TRevealBase> : 
    RevealerFilteredEnumeratorFieldMoldScaffold<TCloaked, TFilterBase, TRevealBase, IEnumerator<TCloaked>?>
       where TCloaked : TRevealBase, TFilterBase
       where TRevealBase : notnull
{
    public IEnumerator<TCloaked>? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerEnumerator);
    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFilteredIterate<IEnumerator<TCloaked>?, TCloaked, TFilterBase, TRevealBase>
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerEnumerator, ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyNullableStruct 
                | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerEnumeratorWhenNonNullAddFilteredStringBearer<TCloakedStruct> : 
  RevealerFilteredEnumeratorFieldMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, IEnumerator<TCloakedStruct?>?>
   where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerEnumerator);
    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFilteredIterateNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerEnumerator
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsTypeAllButNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerEnumeratorWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TBearer, TBearerBase, IEnumerator<TBearer>>
    where TBearer : IStringBearer?, TBearerBase?
{
    public IEnumerator<TBearer>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerEnumerator);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFilteredIterate<IEnumerator<TBearer>?, TBearer, TBearerBase>
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerEnumeratorWhenNonNullAddFilteredStringBearer<TBearerStruct>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TBearerStruct?, IEnumerator<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerEnumerator);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFilteredIterateNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsString 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringEnumeratorWhenNonNullAddFilteredStringBearer : FormattedFilteredEnumeratorFieldMoldScaffold<string?, IEnumerator<string?>>
  
{
    public IEnumerator<string?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringEnumerator);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredStringIterate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsCharSequence 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceEnumeratorWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TCharSeq, TCharSeqFilterBase, IEnumerator<TCharSeq>>
    where TCharSeq : ICharSequence?, TCharSeqFilterBase?
{
    public IEnumerator<TCharSeq>? ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceEnumerator);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeqIterate<IEnumerator<TCharSeq>?, TCharSeq, TCharSeqFilterBase>
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStringBuilder 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderEnumeratorWhenNonNullAddFilteredStringBearer
    : FormattedFilteredEnumeratorFieldMoldScaffold<StringBuilder?, IEnumerator<StringBuilder?>>
{
    public IEnumerator<StringBuilder?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderEnumerator);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredStringBuilderIterate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyGeneric 
                | SupportsValueFormatString)]
public class FieldMatchEnumeratorWhenNonNullAddFilteredStringBearer<TAny, TAnyBase>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TAny, TAnyBase, IEnumerator<TAny>>
    where TAny : TAnyBase
{
    public IEnumerator<TAny>? ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchEnumerator);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredMatchIterate<IEnumerator<TAny>?, TAny, TAnyBase>
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableObject 
                | SupportsValueFormatString)]
public class FieldObjectEnumeratorWhenNonNullAddFilteredStringBearer : FormattedFilteredEnumeratorFieldMoldScaffold<object?, IEnumerator<object?>>
  
{
    public IEnumerator<object?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectEnumerator);

    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObjectIterate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectEnumerator
              , ElementPredicate
              , ValueFormatString, FormattingFlags);

}