using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField.FixtureScaffolding;

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolSpanAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool, bool[]>
{
    public bool[]? ComplexTypeCollectionFieldAlwaysAddFilteredBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolSpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredBoolSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolSpanAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool?, bool?[]>
  
{
    public bool?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolSpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate 
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableSpanAlwaysAddFilteredStringBearer<TFmt, TFmtBase> : FormattedFilteredCollectionFieldMoldScaffold<TFmt, TFmt[]>
   where TFmt : ISpanFormattable?, TFmtBase?
{
    public TFmt[]? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableSpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate 
                | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassSpanAlwaysAddFilteredStringBearer<TFmt, TFmtBase> : 
    FormattedFilteredCollectionFieldMoldScaffold<TFmt?, TFmtBase?, TFmt?[]>
   where TFmt : class, ISpanFormattable, TFmtBase
{
    public TFmt?[]? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableNullableSpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableNullableSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableNullableSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate 
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableSpanAlwaysAddFilteredStringBearer<TFmtStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmtStruct?, TFmtStruct?[]>
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableSpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate 
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerSpanAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TRevealBase> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloaked, TCloakedFilterBase, TRevealBase, TCloaked[]?>
    where TCloaked : TRevealBase?, TCloakedFilterBase?
    where TRevealBase : notnull
{
    public TCloaked[]? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerSpan);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerSpan.AsSpan()
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate 
                | AcceptsAnyNullableClass | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerNullableClassSpanAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TRevealBase> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloaked?, TCloakedFilterBase?, TRevealBase, TCloaked?[]?>
    where TCloaked : class, TRevealBase, TCloakedFilterBase
    where TRevealBase : notnull
{
    public TCloaked?[]? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableSpan);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableSpan.AsSpan()
              , ElementPredicate, ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate 
                | AcceptsAnyNullableStruct | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerSpanAlwaysAddFilteredStringBearer<TCloakedStruct> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerSpan);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerSpan.AsSpan()
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate 
                | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerSpanAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearer, TBearer[]>
    where TBearer : IStringBearer?, TBearerBase?
{
    public TBearer[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerSpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsNullableClass
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerNullableClassSpanAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearer?, TBearerBase?, TBearer?[]>
    where TBearer : class?, IStringBearer?, TBearerBase?
{
    public TBearer?[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerNullableSpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerNullableSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerNullableSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerSpanAlwaysAddFilteredStringBearer<TBearerStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerSpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsString 
                | AcceptsClass | SupportsValueFormatString)]
public class FieldStringSpanAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<string, string[]>
  
{
    public string[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringSpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsString 
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringNullableSpanAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<string?, string?[]>
  
{
    public string?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringSpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .AlwaysAddFilteredNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsCharSequence 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceSpanAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TCharSeq, TCharSeq[]>
    where TCharSeq : ICharSequence?, TCharSeqFilterBase?
{
    public TCharSeq[]? ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceSpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeq
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsStringBuilder 
                | AcceptsClass | SupportsValueFormatString)]
public class FieldStringBuilderSpanAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder, StringBuilder[]>
  
{
    public StringBuilder[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderSpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsStringBuilder 
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderNullableSpanAlwaysAddFilteredStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder?, StringBuilder?[]>
{
    public StringBuilder?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBuilderSpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBuilderSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBuilderSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsAnyGeneric 
                | SupportsValueFormatString)]
public class FieldMatchSpanAlwaysAddFilteredStringBearer<TAny, TAnyFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TAny, TAny[]>
    where TAny : TAnyFilterBase?
{
    public TAny[]? ComplexTypeCollectionFieldAlwaysAddFilteredMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchSpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatch
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredMatchSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate 
                | AcceptsNonNullableObject | SupportsValueFormatString)]
public class FieldObjectSpanAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<object, object[]>
  
{
    public object[]? ComplexTypeCollectionFieldAlwaysAddFilteredObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectSpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObject
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredObjectSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags);

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | FilterPredicate | AcceptsNullableObject 
                | SupportsValueFormatString)]
public class FieldNullableObjectSpanAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<object?, object?[]>
  
{
    public object?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableObjectSpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObjectNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableObjectSpan)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableObjectSpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags);

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolReadOnlySpanAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool, bool[]>
  
{
    public bool[]? ComplexTypeCollectionFieldAlwaysAddFilteredBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolReadOnlySpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolReadOnlySpan)
              , (ReadOnlySpan<bool>)ComplexTypeCollectionFieldAlwaysAddFilteredBoolReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate 
                | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolReadOnlySpanAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool?, bool?[]> 
  
{
    public bool?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolReadOnlySpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolReadOnlySpan)
              , (ReadOnlySpan<bool?>)ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate 
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableReadOnlySpanAlwaysAddFilteredStringBearer<TFmt, TFmtBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmt, TFmtBase?, TFmt[]>
    where TFmt : ISpanFormattable?, TFmtBase?
{
    public TFmt[]? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableReadOnlySpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmt>)ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate 
                | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassReadOnlySpanAlwaysAddFilteredStringBearer<TFmt, TFmtBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmt?, TFmt?[]>
    where TFmt : class, ISpanFormattable, TFmtBase
{
    public TFmt?[]? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableNullableReadOnlySpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableNullableReadOnlySpan)
              , (ReadOnlySpan<TFmt?>)ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableNullableReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate 
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableReadOnlySpanAlwaysAddFilteredStringBearer<TFmtStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmtStruct?, TFmtStruct?[]>
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableReadOnlySpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmtStruct?>)ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate 
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerReadOnlySpanAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TRevealBase> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloaked, TCloakedFilterBase, TRevealBase, TCloaked[]?>
    where TCloaked : TRevealBase?, TCloakedFilterBase?
    where TRevealBase : notnull
{
    public TCloaked[]? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerReadOnlySpan);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloaked>)ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerReadOnlySpan
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate 
                | AcceptsAnyNullableClass | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerNullableClassReadOnlySpanAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TRevealBase> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloaked?, TCloakedFilterBase?, TRevealBase, TCloaked?[]?>
    where TCloaked : class?, TRevealBase?, TCloakedFilterBase?
    where TRevealBase : notnull
{
    public TCloaked?[]? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableReadOnlySpan);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableReadOnlySpan)
              , (ReadOnlySpan<TCloaked?>)ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerNullableReadOnlySpan
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate 
                | AcceptsAnyNullableStruct | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerReadOnlySpanAlwaysAddFilteredStringBearer<TCloakedStruct> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]?>
       where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerReadOnlySpan);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloakedStruct?>)ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerReadOnlySpan
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate 
                | AcceptsTypeNonNullable | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerReadOnlySpanAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearer, TBearer[]>
    where TBearer : IStringBearer?, TBearerBase?
{
    public TBearer[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerReadOnlySpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearer>)ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate 
                | AcceptsNullableClass | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerNullableClassReadOnlySpanAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearer?, TBearerBase?, TBearer?[]>
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerNullableReadOnlySpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerNullableReadOnlySpan)
              , (ReadOnlySpan<TBearer?>)ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerNullableReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate 
                | AcceptsNullableStruct | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerReadOnlySpanAlwaysAddFilteredStringBearer<TBearerStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerReadOnlySpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerReadOnlySpan),
                (ReadOnlySpan<TBearerStruct?>)ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsString 
                | AcceptsClass | SupportsValueFormatString)]
public class FieldStringReadOnlySpanAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<string, string[]>
  
{
    public string[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringReadOnlySpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringReadOnlySpan)
              , (ReadOnlySpan<string>)ComplexTypeCollectionFieldAlwaysAddFilteredStringReadOnlySpan.AsSpan()
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate | AcceptsString 
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringNullableReadOnlySpanAlwaysAddFilteredStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<string?, string?[]>
{
    public string?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringReadOnlySpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringReadOnlySpan),
                (ReadOnlySpan<string?>)ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate 
                | AcceptsClass | AcceptsNullableClass | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceReadOnlySpanAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TCharSeq, TCharSeq[]>
    where TCharSeq : ICharSequence?, TCharSeqFilterBase?
{
    public TCharSeq[]? ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceReadOnlySpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeq
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceReadOnlySpan)
              , (ReadOnlySpan<TCharSeq>)ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}


[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate 
                | AcceptsClass | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderReadOnlySpanAlwaysAddFilteredStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder, StringBuilder[]>
{
    public StringBuilder[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderReadOnlySpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderReadOnlySpan)
              , (ReadOnlySpan<StringBuilder>)ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate 
                | AcceptsNullableClass  | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderNullableReadOnlySpanAlwaysAddFilteredStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder?, StringBuilder?[]>
{
    public StringBuilder?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBuilderReadOnlySpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBuilderReadOnlySpan)
              , (ReadOnlySpan<StringBuilder?>)ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBuilderReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate 
                | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchReadOnlySpanAlwaysAddFilteredStringBearer<TAny, TAnyFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TAny, TAny[]>
    where TAny : TAnyFilterBase?
{
    public TAny[]? ComplexTypeCollectionFieldAlwaysAddFilteredMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchReadOnlySpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatch
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchReadOnlySpan)
              , (ReadOnlySpan<TAny>)ComplexTypeCollectionFieldAlwaysAddFilteredMatchReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate 
                | AcceptsNonNullableObject | SupportsValueFormatString)]
public class FieldObjectReadOnlySpanAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<object, object[]>
  
{
    public object[]? ComplexTypeCollectionFieldAlwaysAddFilteredObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectReadOnlySpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObject
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectReadOnlySpan)
              , (ReadOnlySpan<object>)ComplexTypeCollectionFieldAlwaysAddFilteredObjectReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags);

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | FilterPredicate 
                | AcceptsNullableObject | SupportsValueFormatString)]
public class FieldNullableObjectReadOnlySpanAlwaysAddFilteredStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<object?, object?[]>
{
    public object?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableObjectReadOnlySpan);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObjectNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableObjectReadOnlySpan)
              , (ReadOnlySpan<object?>)ComplexTypeCollectionFieldAlwaysAddFilteredNullableObjectReadOnlySpan
              , ElementPredicate
              , ValueFormatString, FormattingFlags);

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolArrayAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool, bool[]>
  
{
    public bool[]? ComplexTypeCollectionFieldAlwaysAddFilteredBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolArray);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredBoolArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolArrayAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool?, bool?[]>
  
{
    public bool?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolArray);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | FilterPredicate 
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableArrayAlwaysAddFilteredStringBearer<TFmt, TFmtBase> : FormattedFilteredCollectionFieldMoldScaffold<TFmt, TFmt[]>
   where TFmt : ISpanFormattable?, TFmtBase?
{
    public TFmt[]? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableArray);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | FilterPredicate 
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableArrayAlwaysAddFilteredStringBearer<TFmtStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmtStruct?, TFmtStruct?[]>
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableArray);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsAnyExceptNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerArrayAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TRevealBase> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloaked, TCloakedFilterBase, TRevealBase, TCloaked[]?>
       where TCloaked : TRevealBase, TCloakedFilterBase
       where TRevealBase : notnull
{
    public TCloaked[]? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerArray);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerArray, ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsAnyNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerArrayAlwaysAddFilteredStringBearer<TCloakedStruct> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerArray);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerArray
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerArrayAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearer?, TBearer?[]>
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerArray);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerArrayAlwaysAddFilteredStringBearer<TBearerStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerArray);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsString 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringArrayAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<string?, string?[]>
  
{
    public string?[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringArray);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsCharSequence 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceArrayAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TCharSeq?, TCharSeq?[]>
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceArray);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeq
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsStringBuilder 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderArrayAlwaysAddFilteredStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder?, StringBuilder?[]>
{
    public StringBuilder?[]? ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderArray);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsAnyGeneric 
                | SupportsValueFormatString)]
public class FieldMatchArrayAlwaysAddFilteredStringBearer<TAny, TAnyFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TAny, TAny[]>
    where TAny : TAnyFilterBase
{
    public TAny[]? ComplexTypeCollectionFieldAlwaysAddFilteredMatchArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchArray);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .AlwaysAddFilteredMatch
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredMatchArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsAnyGeneric 
                | SupportsValueFormatString)]
public class FieldObjectArrayAlwaysAddFilteredStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<object?, object?[]>
{
    public object?[]? ComplexTypeCollectionFieldAlwaysAddFilteredObjectArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectArray);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObject
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectArray)
              , ComplexTypeCollectionFieldAlwaysAddFilteredObjectArray
              , ElementPredicate
              , ValueFormatString, FormattingFlags);

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolListAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool, IReadOnlyList<bool>>
  
{
    public IReadOnlyList<bool>? ComplexTypeCollectionFieldAlwaysAddFilteredBoolList
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredBoolList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredBoolList
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolListAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool?, IReadOnlyList<bool?>>
  
{
    public IReadOnlyList<bool?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableBoolList
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | FilterPredicate 
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableListAlwaysAddFilteredStringBearer<TFmt, TFmtBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmt?, IReadOnlyList<TFmt?>>
    where TFmt : ISpanFormattable, TFmtBase
{
    public IReadOnlyList<TFmt?>? ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredSpanFormattableList
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | FilterPredicate 
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableListAlwaysAddFilteredStringBearer<TFmtStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmtStruct?, IReadOnlyList<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableSpanFormattableList
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsAnyExceptNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerListAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TRevealBase> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloaked, TCloakedFilterBase, TRevealBase, IReadOnlyList<TCloaked>?>
       where TCloaked : TRevealBase, TCloakedFilterBase
       where TRevealBase : notnull
{
    public IReadOnlyList<TCloaked>? ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerList);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCloakedBearerList, ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsAnyNullableStruct 
                | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerListAlwaysAddFilteredStringBearer<TCloakedStruct> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, IReadOnlyList<TCloakedStruct?>?>
  where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerList);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableCloakedBearerList
              , ElementPredicate
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerListAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearer?, IReadOnlyList<TBearer?>>
    where TBearer : IStringBearer, TBearerBase
{
    public IReadOnlyList<TBearer?>? ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBearerList
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerListAlwaysAddFilteredStringBearer<TBearerStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TBearerStruct?, IReadOnlyList<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredNullableStringBearerList
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsString 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringListAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<string?, IReadOnlyList<string?>>
  
{
    public IReadOnlyList<string?>? ComplexTypeCollectionFieldAlwaysAddFilteredStringList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringList
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsCharSequence 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceListAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TCharSeq?, IReadOnlyList<TCharSeq?>>
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IReadOnlyList<TCharSeq?>? ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeq
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredCharSequenceList
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsStringBuilder 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderListAlwaysAddFilteredStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder?, IReadOnlyList<StringBuilder?>>
{
    public IReadOnlyList<StringBuilder?>? ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredStringBuilderList
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsAnyGeneric 
                | SupportsValueFormatString)]
public class FieldMatchListAlwaysAddFilteredStringBearer<TAny, TAnyFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TAny, IReadOnlyList<TAny>>
    where TAny : TAnyFilterBase
{
    public IReadOnlyList<TAny>? ComplexTypeCollectionFieldAlwaysAddFilteredMatchList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatch
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredMatchList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredMatchList
              , ElementPredicate
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsNullableClass 
                | SupportsValueFormatString)]
public class FieldObjectListAlwaysAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<object?, IReadOnlyList<object?>>
  
{
    public IReadOnlyList<object?>? ComplexTypeCollectionFieldAlwaysAddFilteredObjectList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectList);

    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObject
               (nameof(ComplexTypeCollectionFieldAlwaysAddFilteredObjectList)
              , ComplexTypeCollectionFieldAlwaysAddFilteredObjectList
              , ElementPredicate
              , ValueFormatString, FormattingFlags);

}
