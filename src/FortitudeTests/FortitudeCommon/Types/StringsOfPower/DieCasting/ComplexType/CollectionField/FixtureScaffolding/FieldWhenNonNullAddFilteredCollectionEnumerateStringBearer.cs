using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.ScaffoldingStringBuilderInvokeFlags;

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

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolEnumerable, ElementPredicate, ValueFormatString)
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

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolEnumerable, ElementPredicate
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate 
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableEnumerableWhenNonNullAddFilteredStringBearer<TFmt, TFmtBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmt, TFmtBase, IEnumerable<TFmt>>
    where TFmt : ISpanFormattable, TFmtBase
{
    public IEnumerable<TFmt?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableEnumerable);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableEnumerable
              , ElementPredicate, ValueFormatString)
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

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableEnumerable
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct |
                  SupportsValueRevealer)]
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
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerEnumerable, ElementPredicate
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyNullableStruct 
                | SupportsValueRevealer)]
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
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerEnumerable
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsTypeAllButNullableStruct 
                | AcceptsStringBearer)]
public class FieldStringBearerEnumerableWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>
    : FilteredCollectionFieldMoldScaffold<TBearer, TBearerBase, IEnumerable<TBearer>>
    where TBearer : IStringBearer, TBearerBase
{
    public IEnumerable<TBearer?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerEnumerable);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerEnumerable, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableStruct 
                | AcceptsStringBearer)]
public class FieldNullableStringBearerEnumerableWhenNonNullAddFilteredStringBearer<TBearerStruct>
    : FilteredCollectionFieldMoldScaffold<TBearerStruct?, IEnumerable<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerEnumerable);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerEnumerable, ElementPredicate)
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

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringEnumerable
              , ElementPredicate, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsCharSequence 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceEnumerableWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TCharSeq, TCharSeqFilterBase, IEnumerable<TCharSeq>>
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IEnumerable<TCharSeq?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceEnumerable
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceEnumerable);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceEnumerable
              , ElementPredicate, ValueFormatString)
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

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderEnumerable
              , ElementPredicate, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyGeneric 
                | SupportsValueFormatString)]
public class FieldMatchEnumerableWhenNonNullAddFilteredStringBearer<TAny, TAnyBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TAny, IEnumerable<TAny>>
    where TAny : TAnyBase
{
    public IEnumerable<TAny>? ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchEnumerable);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchEnumerable
              , ElementPredicate, ValueFormatString)
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

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectEnumerable
              , ElementPredicate, ValueFormatString);

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

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolEnumerator, ElementPredicate, ValueFormatString)
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

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolEnumerator, ElementPredicate, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate 
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableEnumeratorWhenNonNullAddFilteredStringBearer<TFmt, TFmtBase>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TFmt, TFmtBase, IEnumerator<TFmt>>
    where TFmt : ISpanFormattable, TFmtBase
{
    public IEnumerator<TFmt?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableEnumerator);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableEnumerator
              , ElementPredicate, ValueFormatString)
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

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableEnumerator
              , ElementPredicate, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct |
                  SupportsValueRevealer)]
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
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerEnumerator, ElementPredicate
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyNullableStruct 
                | SupportsValueRevealer)]
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
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerEnumerator
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsTypeAllButNullableStruct 
                | AcceptsStringBearer)]
public class FieldStringBearerEnumeratorWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>
    : FilteredEnumeratorFieldMoldScaffold<TBearer, TBearerBase, IEnumerator<TBearer>>
    where TBearer : IStringBearer, TBearerBase
{
    public IEnumerator<TBearer?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerEnumerator);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerEnumerator, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableStruct 
                | AcceptsStringBearer)]
public class FieldNullableStringBearerEnumeratorWhenNonNullAddFilteredStringBearer<TBearerStruct>
    : FilteredEnumeratorFieldMoldScaffold<TBearerStruct?, IEnumerator<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerEnumerator);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerEnumerator, ElementPredicate)
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

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringEnumerator
              , ElementPredicate, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsCharSequence 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceEnumeratorWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TCharSeq, TCharSeqFilterBase, IEnumerator<TCharSeq>>
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IEnumerator<TCharSeq?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceEnumerator);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceEnumerator
              , ElementPredicate, ValueFormatString)
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

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderEnumerator
              , ElementPredicate, ValueFormatString)
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

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchEnumerator
              , ElementPredicate, ValueFormatString)
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

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectEnumerator
              , ElementPredicate, ValueFormatString);

}