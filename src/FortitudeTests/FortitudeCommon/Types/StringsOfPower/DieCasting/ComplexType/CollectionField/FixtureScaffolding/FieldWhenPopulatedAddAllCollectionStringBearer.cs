using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField.FixtureScaffolding;

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolSpanWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool, bool[]>
{
    public bool[] ComplexTypeCollectionFieldWhenPopulatedAddAllBoolSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllBoolSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolSpanWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool?, bool?[]>
{
    public bool?[] ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites 
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableSpanWhenPopulatedAddAllStringBearer<TFmt> : FormattedCollectionFieldMoldScaffold<TFmt, TFmt[]>
    where TFmt : ISpanFormattable?
{
    public TFmt[] ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites 
                | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassSpanWhenPopulatedAddAllStringBearer<TFmt> : FormattedCollectionFieldMoldScaffold<TFmt?, TFmt?[]>
    where TFmt : class, ISpanFormattable
{
    public TFmt?[] ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableClassSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableClassSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableClassSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableClassSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites 
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableSpanWhenPopulatedAddAllStringBearer<TFmtStruct> : FormattedCollectionFieldMoldScaffold<TFmtStruct?, TFmtStruct?[]>
   where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites 
                | AcceptsTypeAllButNullableStruct | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerSpanWhenPopulatedAddAllStringBearer<TCloaked, TRevealBase> : 
    RevealerCollectionFieldMoldScaffold<TCloaked, TRevealBase, TCloaked[]> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked[] ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerSpan);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerSpan.AsSpan()
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsAnyNullableClass 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerNullableClassSpanWhenPopulatedAddAllStringBearer<TCloaked, TRevealBase> : 
    RevealerCollectionFieldMoldScaffold<TCloaked?, TRevealBase, TCloaked?[]> 
    where TCloaked : class, TRevealBase
    where TRevealBase : notnull
{
    public TCloaked?[] ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan.AsSpan()
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsAnyNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerSpanWhenPopulatedAddAllStringBearer<TCloakedStruct> : 
    RevealerCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerSpan.AsSpan()
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites 
                | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerSpanWhenPopulatedAddAllStringBearer<TBearer> : FormattedCollectionFieldMoldScaffold<TBearer, TBearer[]>
    where TBearer : IStringBearer?
{
    public TBearer[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsNullableClass 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerNullableClassSpanWhenPopulatedAddAllStringBearer<TBearer> : FormattedCollectionFieldMoldScaffold<TBearer?, TBearer?[]>
    where TBearer : class, IStringBearer
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerSpanWhenPopulatedAddAllStringBearer<TBearerStruct> : FormattedCollectionFieldMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsString 
                | AcceptsClass | SupportsValueFormatString)]
public class FieldStringSpanWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string, string[]>
{
    public string[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsString 
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringNullableSpanWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string?, string?[]>
{
    public string?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsCharSequence 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceSpanWhenPopulatedAddAllStringBearer<TCharSeq> : FormattedCollectionFieldMoldScaffold<TCharSeq, TCharSeq[]>
    where TCharSeq : ICharSequence?
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsCharSequence 
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceNullableSpanWhenPopulatedAddAllStringBearer<TCharSeq> : FormattedCollectionFieldMoldScaffold<TCharSeq?, TCharSeq?[]>
   where TCharSeq : ICharSequence
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsStringBuilder 
                | AcceptsClass | SupportsValueFormatString)]
public class FieldStringBuilderSpanWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<StringBuilder, StringBuilder[]>
{
    public StringBuilder[] ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsStringBuilder 
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderNullableSpanWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<StringBuilder?, StringBuilder?[]>
  
{
    public StringBuilder?[] ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsAnyGeneric 
                | SupportsValueFormatString)]
public class FieldMatchSpanWhenPopulatedAddAllStringBearer<TAny> : FormattedCollectionFieldMoldScaffold<TAny, TAny[]>
{
    public TAny[]? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllMatchSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsNonNullableObject 
                | SupportsValueFormatString)]
public class FieldObjectSpanWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object, object[]>
{
    public object[]? ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullAndPopulatedWrites | AcceptsNullableObject 
                | SupportsValueFormatString)]
public class FieldNullableObjectSpanWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object?, object?[]>
{
    public object?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableRefSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableRefSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObjectNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableRefSpan)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableRefSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolReadOnlySpanWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool, bool[]>
{
    public bool[] ComplexTypeCollectionFieldWhenPopulatedAddAllBoolReadOnlySpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolReadOnlySpan)
              , (ReadOnlySpan<bool>)ComplexTypeCollectionFieldWhenPopulatedAddAllBoolReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolReadOnlySpanWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool?, bool?[]>
{
    public bool?[] ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolReadOnlySpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolReadOnlySpan)
              , (ReadOnlySpan<bool?>)ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites 
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableReadOnlySpanWhenPopulatedAddAllStringBearer<TFmt> : FormattedCollectionFieldMoldScaffold<TFmt, TFmt[]>
    where TFmt : ISpanFormattable?
{
    public TFmt[] ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmt>)ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan
              , ValueFormatString
              , FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites 
                | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassReadOnlySpanWhenPopulatedAddAllStringBearer<TFmt> : FormattedCollectionFieldMoldScaffold<TFmt?, TFmt?[]>
   where TFmt : class, ISpanFormattable
{
    public TFmt?[] ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableRefReadOnlySpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableRefReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableRefReadOnlySpan)
              , (ReadOnlySpan<TFmt?>)ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableNullableRefReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites 
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldNullableSpanFormattableReadOnlySpanWhenPopulatedAddAllStringBearer<TFmtStruct> : FormattedCollectionFieldMoldScaffold<TFmtStruct?, TFmtStruct?[]>
   where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmtStruct?>)ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites 
                | AcceptsTypeAllButNullableStruct | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerReadOnlySpanWhenPopulatedAddAllStringBearer<TCloaked, TRevealBase> : 
    RevealerCollectionFieldMoldScaffold<TCloaked, TRevealBase, TCloaked[]> 
    where TCloaked : TRevealBase?
    where TRevealBase : notnull
{
    public TCloaked[] ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloaked>)ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerReadOnlySpan
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites 
                | AcceptsAnyNullableClass | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerNullableClassReadOnlySpanWhenPopulatedAddAllStringBearer<TCloaked, TRevealBase> : 
  RevealerCollectionFieldMoldScaffold<TCloaked?, TRevealBase, TCloaked?[]> 
    where TCloaked : class, TRevealBase
    where TRevealBase : notnull
{
    public TCloaked?[] ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan)
              , (ReadOnlySpan<TCloaked?>)ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerNullableSpan.AsSpan()
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites 
                | AcceptsAnyNullableStruct | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerReadOnlySpanWhenPopulatedAddAllStringBearer<TCloakedStruct> : 
    RevealerCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloakedStruct?>)ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerReadOnlySpan
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites 
                | AcceptsTypeAllButNullableStruct | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerReadOnlySpanWhenPopulatedAddAllStringBearer<TBearer> : FormattedCollectionFieldMoldScaffold<TBearer, TBearer[]>
    where TBearer : IStringBearer?
{
    public TBearer[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearer>)ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsNullableClass 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerNullableClassReadOnlySpanWhenPopulatedAddAllStringBearer<TBearer> : FormattedCollectionFieldMoldScaffold<TBearer?, TBearer?[]>
    where TBearer : class, IStringBearer
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearer?>)ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerReadOnlySpanWhenPopulatedAddAllStringBearer<TBearerStruct> : FormattedCollectionFieldMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearerStruct?>)ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsString 
                | AcceptsClass | SupportsValueFormatString)]
public class FieldStringReadOnlySpanWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string, string[]>
{
    public string[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringReadOnlySpan)
              , (ReadOnlySpan<string>)ComplexTypeCollectionFieldWhenPopulatedAddAllStringReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsString 
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringNullableReadOnlySpanWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string?, string?[]>
{
    public string?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringNullableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringNullableReadOnlySpan)
              , (ReadOnlySpan<string?>)ComplexTypeCollectionFieldWhenPopulatedAddAllStringNullableReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsCharSequence 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceReadOnlySpanWhenPopulatedAddAllStringBearer<TCharSeq> : FormattedCollectionFieldMoldScaffold<TCharSeq, TCharSeq[]>
    where TCharSeq : ICharSequence?
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceReadOnlySpan)
              , (ReadOnlySpan<TCharSeq>)ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsCharSequence 
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceNullableReadOnlySpanWhenPopulatedAddAllStringBearer<TCharSeq> : FormattedCollectionFieldMoldScaffold<TCharSeq?, TCharSeq?[]>
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan)
              , (ReadOnlySpan<TCharSeq?>)ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceNullableSpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsStringBuilder 
                | AcceptsClass | SupportsValueFormatString)]
public class FieldStringBuilderReadOnlySpanWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<StringBuilder, StringBuilder[]>
  
{
    public StringBuilder[] ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderReadOnlySpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderReadOnlySpan)
              , (ReadOnlySpan<StringBuilder>)ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites | AcceptsStringBuilder 
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderNullableReadOnlySpanWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<StringBuilder?, StringBuilder?[]>
  
{
    public StringBuilder?[] ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderNullableReadOnlySpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderNullableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderNullableReadOnlySpan)
              , (ReadOnlySpan<StringBuilder?>)ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderNullableReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites 
                | AcceptsOnlyNonNullableGeneric | SupportsValueFormatString)]
public class FieldMatchReadOnlySpanWhenPopulatedAddAllStringBearer<TAny> : FormattedCollectionFieldMoldScaffold<TAny, TAny[]>
{
    public TAny[]? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchReadOnlySpan)
              , (ReadOnlySpan<TAny>)ComplexTypeCollectionFieldWhenPopulatedAddAllMatchReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites 
                | AcceptsNonNullableObject | SupportsValueFormatString)]
public class FieldObjectReadOnlySpanWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object, object[]>
{
    public object[]? ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan)
              , (ReadOnlySpan<object>)ComplexTypeCollectionFieldWhenPopulatedAddAllObjectSpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullAndPopulatedWrites 
                | AcceptsNullableObject | SupportsValueFormatString)]
public class FieldNullableObjectReadOnlySpanWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object?, object?[]>
{
    public object?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObjectNullable
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableReadOnlySpan)
              , (ReadOnlySpan<object?>)ComplexTypeCollectionFieldWhenPopulatedAddAllObjectNullableReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolArrayWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool, bool[]>
{
    public bool[]? ComplexTypeCollectionFieldWhenPopulatedAddAllBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllBoolArray
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | AcceptsNullableStruct 
                | SupportsValueFormatString)]
public class FieldNullableBoolArrayWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool?, bool?[]>
{
    public bool?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolArray
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | AcceptsSpanFormattableExceptNullableStruct 
                | SupportsValueFormatString)]
public class FieldSpanFormattableArrayWhenPopulatedAddAllStringBearer<TFmt> : FormattedCollectionFieldMoldScaffold<TFmt?, TFmt?[]>
    where TFmt : ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableArray
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | AcceptsOnlyNullableStructSpanFormattable 
                | SupportsValueFormatString)]
public class FieldNullableSpanFormattableArrayWhenPopulatedAddAllStringBearer<TFmtStruct> : FormattedCollectionFieldMoldScaffold<TFmtStruct?, TFmtStruct?[]>
   where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableArray
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | AcceptsAnyExceptNullableStruct |
                  SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerArrayWhenPopulatedAddAllStringBearer<TCloaked, TRevealBase> : 
    RevealerCollectionFieldMoldScaffold<TCloaked, TRevealBase, TCloaked[]?> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerArray);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerArray
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | AcceptsAnyNullableStruct |
                  SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerArrayWhenPopulatedAddAllStringBearer<TCloakedStruct> : 
    RevealerCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerArray
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | AcceptsTypeAllButNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerArrayWhenPopulatedAddAllStringBearer<TBearer> : FormattedCollectionFieldMoldScaffold<TBearer, TBearer[]>
    where TBearer : IStringBearer
{
    public TBearer[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerArray
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | AcceptsNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerArrayWhenPopulatedAddAllStringBearer<TBearerStruct> : FormattedCollectionFieldMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerArray
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | AcceptsString 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringArrayWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string?, string?[]>
{
    public string?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringArray
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | AcceptsCharSequence 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceArrayWhenPopulatedAddAllStringBearer<TCharSeq> : FormattedCollectionFieldMoldScaffold<TCharSeq?, TCharSeq?[]>
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceArray
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | AcceptsStringBuilder 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderArrayWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<StringBuilder?, StringBuilder?[]>
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderArray
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | AcceptsAnyGeneric 
                | SupportsValueFormatString)]
public class FieldMatchArrayWhenPopulatedAddAllStringBearer<TAny> : FormattedCollectionFieldMoldScaffold<TAny, TAny[]>
{
    public TAny[]? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllMatchArray
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | NonNullAndPopulatedWrites | AcceptsNullableObject 
                | SupportsValueFormatString)]
public class FieldObjectArrayWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object?, object?[]>
{
    public object?[]? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableObjectArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableObjectArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableObjectArray)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableObjectArray
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolListWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool, IReadOnlyList<bool>>
{
    public IReadOnlyList<bool>? ComplexTypeCollectionFieldWhenPopulatedAddAllBoolList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllBoolList
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolListWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool?, IReadOnlyList<bool?>>
{
    public IReadOnlyList<bool?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolList
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | AcceptsSpanFormattableExceptNullableStruct 
                | SupportsValueFormatString)]
public class FieldSpanFormattableListWhenPopulatedAddAllStringBearer<TFmt> : FormattedCollectionFieldMoldScaffold<TFmt, IReadOnlyList<TFmt>>
    where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt>? ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableList
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | AcceptsOnlyNullableStructSpanFormattable 
                | SupportsValueFormatString)]
public class FieldNullableSpanFormattableListWhenPopulatedAddAllStringBearer<TFmtStruct> : FormattedCollectionFieldMoldScaffold<TFmtStruct?
  , IReadOnlyList<TFmtStruct?>> where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableList
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | AcceptsAnyExceptNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerListWhenPopulatedAddAllStringBearer<TCloaked, TRevealBase> : 
    RevealerCollectionFieldMoldScaffold<TCloaked, TRevealBase, IReadOnlyList<TCloaked>?> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public IReadOnlyList<TCloaked>? ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerList
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | AcceptsAnyNullableStruct 
                | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerListWhenPopulatedAddAllStringBearer<TCloakedStruct> : 
  RevealerCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct, IReadOnlyList<TCloakedStruct?>?> where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerList);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerList
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerListWhenPopulatedAddAllStringBearer<TBearer> : FormattedCollectionFieldMoldScaffold<TBearer?, IReadOnlyList<TBearer?>>
    where TBearer : IStringBearer
{
    public IReadOnlyList<TBearer?>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerList
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | AcceptsNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerListWhenPopulatedAddAllStringBearer<TBearerStruct> : 
    FormattedCollectionFieldMoldScaffold<TBearerStruct?, IReadOnlyList<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerList
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | AcceptsString 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringListWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string?, IReadOnlyList<string?>>
{
    public IReadOnlyList<string?>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringList
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | AcceptsCharSequence 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceListWhenPopulatedAddAllStringBearer<TCharSeq> : FormattedCollectionFieldMoldScaffold<TCharSeq?
  , IReadOnlyList<TCharSeq?>> where TCharSeq : ICharSequence
{
    public IReadOnlyList<TCharSeq?>? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceList
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | AcceptsStringBuilder 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderListWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<StringBuilder?, IReadOnlyList<StringBuilder?>>
  
{
    public IReadOnlyList<StringBuilder?>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderList
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | AcceptsAnyGeneric 
                | SupportsValueFormatString)]
public class FieldMatchListWhenPopulatedAddAllStringBearer<TAny> : FormattedCollectionFieldMoldScaffold<TAny, IReadOnlyList<TAny>>
{
    public IReadOnlyList<TAny>? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllMatchList
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | NonNullAndPopulatedWrites | AcceptsNonNullableObject 
                | SupportsValueFormatString)]
public class FieldObjectListWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object?, IReadOnlyList<object?>>
{
    public IReadOnlyList<object?>? ComplexTypeCollectionFieldWhenPopulatedAddAllObjectList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObject
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectList)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllObjectList
              , ValueFormatString, FormattingFlags)
           .Complete();

}
