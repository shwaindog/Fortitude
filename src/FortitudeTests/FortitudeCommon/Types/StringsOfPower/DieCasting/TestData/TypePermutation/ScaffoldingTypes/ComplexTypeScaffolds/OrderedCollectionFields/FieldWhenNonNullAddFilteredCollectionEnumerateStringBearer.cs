using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    OrderedCollectionFields;

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStruct 
                | SupportsValueFormatString)]
public class FieldBoolEnumerableWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool, IEnumerable<bool>>
  
{
    public IEnumerable<bool>? ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolEnumerableWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool?, IEnumerable<bool?>>
  
{
    public IEnumerable<bool?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList, ElementPredicate
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate 
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableEnumerableWhenNonNullAddFilteredStringBearer<TFmt, TFmtBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmt, TFmtBase, IEnumerable<TFmt>>
    where TFmt : ISpanFormattable, TFmtBase
{
    public IEnumerable<TFmt?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate 
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumerableWhenNonNullAddFilteredStringBearer<TFmtStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmtStruct?, IEnumerable<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct |
                  SupportsValueRevealer)]
public class FieldCloakedBearerEnumerableWhenNonNullAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TRevealBase> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloaked, TCloakedFilterBase, TRevealBase, IEnumerable<TCloaked>>
       where TCloaked : TRevealBase, TCloakedFilterBase
       where TRevealBase : notnull
{
    public IEnumerable<TCloaked>? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList, ElementPredicate
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyNullableStruct 
                | SupportsValueRevealer)]
public class FieldNullableCloakedBearerEnumerableWhenNonNullAddFilteredStringBearer<TCloakedStruct> : 
  RevealerFilteredCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, IEnumerable<TCloakedStruct?>>
   where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsTypeAllButNullableStruct 
                | AcceptsStringBearer)]
public class FieldStringBearerEnumerableWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>
    : FilteredCollectionFieldMoldScaffold<TBearer, TBearerBase, IEnumerable<TBearer>>
    where TBearer : IStringBearer, TBearerBase
{
    public IEnumerable<TBearer?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableStruct 
                | AcceptsStringBearer)]
public class FieldNullableStringBearerEnumerableWhenNonNullAddFilteredStringBearer<TBearerStruct>
    : FilteredCollectionFieldMoldScaffold<TBearerStruct?, IEnumerable<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsString 
                | SupportsValueFormatString)]
public class FieldStringEnumerableWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<string?, IEnumerable<string?>>
  
{
    public IEnumerable<string?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList
              , ElementPredicate, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsCharSequence 
                | SupportsValueFormatString)]
public class FieldCharSequenceEnumerableWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TCharSeq, TCharSeqFilterBase, IEnumerable<TCharSeq>>
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IEnumerable<TCharSeq?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList
              , ElementPredicate, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStringBuilder 
                | SupportsValueFormatString)]
public class FieldStringBuilderEnumerableWhenNonNullAddFilteredStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder?, IEnumerable<StringBuilder?>>
{
    public IEnumerable<StringBuilder?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList
              , ElementPredicate, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyGeneric 
                | SupportsValueFormatString)]
public class FieldMatchEnumerableWhenNonNullAddFilteredStringBearer<TAny, TAnyBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TAny, IEnumerable<TAny>>
    where TAny : TAnyBase
{
    public IEnumerable<TAny>? ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList
              , ElementPredicate, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableObject 
                | SupportsValueFormatString)]
public class FieldObjectEnumerableWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<object?, IEnumerable<object?>>
  
{
    public IEnumerable<object?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList
              , ElementPredicate, ValueFormatString);

}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStruct 
                | SupportsValueFormatString)]
public class FieldBoolEnumeratorWhenNonNullAddFilteredStringBearer : FormattedFilteredEnumeratorFieldMoldScaffold<bool, IEnumerator<bool>>
  
{
    public IEnumerator<bool>? ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList, ElementPredicate, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolEnumeratorWhenNonNullAddFilteredStringBearer : FormattedFilteredEnumeratorFieldMoldScaffold<bool?, IEnumerator<bool?>>
  
{
    public IEnumerator<bool?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList, ElementPredicate, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate 
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableEnumeratorWhenNonNullAddFilteredStringBearer<TFmt, TFmtBase>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TFmt, TFmtBase, IEnumerator<TFmt>>
    where TFmt : ISpanFormattable, TFmtBase
{
    public IEnumerator<TFmt?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList
              , ElementPredicate, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate 
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumeratorWhenNonNullAddFilteredStringBearer<TFmtStruct>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TFmtStruct?, IEnumerator<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList
              , ElementPredicate, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct |
                  SupportsValueRevealer)]
public class FieldCloakedBearerEnumeratorWhenNonNullAddFilteredStringBearer<TCloaked, TFilterBase, TRevealBase> : 
    RevealerFilteredEnumeratorFieldMoldScaffold<TCloaked, TFilterBase, TRevealBase, IEnumerator<TCloaked>>
       where TCloaked : TRevealBase, TFilterBase
       where TRevealBase : notnull
{
    public IEnumerator<TCloaked>? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList, ElementPredicate
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyNullableStruct 
                | SupportsValueRevealer)]
public class FieldNullableCloakedBearerEnumeratorWhenNonNullAddFilteredStringBearer<TCloakedStruct> : 
  RevealerFilteredEnumeratorFieldMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, IEnumerator<TCloakedStruct?>>
   where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsTypeAllButNullableStruct 
                | AcceptsStringBearer)]
public class FieldStringBearerEnumeratorWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>
    : FilteredEnumeratorFieldMoldScaffold<TBearer, TBearerBase, IEnumerator<TBearer>>
    where TBearer : IStringBearer, TBearerBase
{
    public IEnumerator<TBearer?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableStruct 
                | AcceptsStringBearer)]
public class FieldNullableStringBearerEnumeratorWhenNonNullAddFilteredStringBearer<TBearerStruct>
    : FilteredEnumeratorFieldMoldScaffold<TBearerStruct?, IEnumerator<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsString 
                | SupportsValueFormatString)]
public class FieldStringEnumeratorWhenNonNullAddFilteredStringBearer : FormattedFilteredEnumeratorFieldMoldScaffold<string?, IEnumerator<string?>>
  
{
    public IEnumerator<string?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList
              , ElementPredicate, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsCharSequence 
                | SupportsValueFormatString)]
public class FieldCharSequenceEnumeratorWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TCharSeq, TCharSeqFilterBase, IEnumerator<TCharSeq>>
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IEnumerator<TCharSeq?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList
              , ElementPredicate, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStringBuilder 
                | SupportsValueFormatString)]
public class FieldStringBuilderEnumeratorWhenNonNullAddFilteredStringBearer
    : FormattedFilteredEnumeratorFieldMoldScaffold<StringBuilder?, IEnumerator<StringBuilder?>>
{
    public IEnumerator<StringBuilder?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList
              , ElementPredicate, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyGeneric 
                | SupportsValueFormatString)]
public class FieldMatchEnumeratorWhenNonNullAddFilteredStringBearer<TAny, TAnyBase>
    : FormattedFilteredEnumeratorFieldMoldScaffold<TAny, TAnyBase, IEnumerator<TAny>>
    where TAny : TAnyBase
{
    public IEnumerator<TAny>? ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList
              , ElementPredicate, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableObject 
                | SupportsValueFormatString)]
public class FieldObjectEnumeratorWhenNonNullAddFilteredStringBearer : FormattedFilteredEnumeratorFieldMoldScaffold<object?, IEnumerator<object?>>
  
{
    public IEnumerator<object?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList
              , ElementPredicate, ValueFormatString);

}