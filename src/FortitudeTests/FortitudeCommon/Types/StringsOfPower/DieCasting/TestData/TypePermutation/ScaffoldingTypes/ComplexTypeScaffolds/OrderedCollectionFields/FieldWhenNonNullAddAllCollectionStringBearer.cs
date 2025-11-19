// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    OrderedCollectionFields;

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolSpanWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool, bool[]>
{
    public bool[] ComplexTypeCollectionFieldWhenNonNullAddAllBoolSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllBoolSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolSpanWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool?, bool?[]>
{
    public bool?[] ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsNonNullableSpanFormattable
                | SupportsValueFormatString)]
public class FieldSpanFormattableSpanWhenNonNullAddAllStringBearer<TFmt> : FormattedCollectionFieldMoldScaffold<TFmt, TFmt[]>
    where TFmt : ISpanFormattable
{
    public TFmt[] ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsOnlyNullableClassSpanFormattable
                | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassSpanWhenNonNullAddAllStringBearer<TFmt> : FormattedCollectionFieldMoldScaffold<TFmt?, TFmt?[]>
    where TFmt : class, ISpanFormattable
{
    public TFmt?[] ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableNullableClassSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableNullableClassSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableNullableClassSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableNullableClassSpan.AsSpan()
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class FieldNullableSpanFormattableSpanWhenNonNullAddAllStringBearer<TFmtStruct> :
    FormattedCollectionFieldMoldScaffold<TFmtStruct?, TFmtStruct?[]>
   where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsAnyNonNullable |
                  SupportsValueRevealer)]
public class FieldCloakedBearerSpanWhenNonNullAddAllStringBearer<TCloaked, TRevealBase> : 
    RevealerCollectionFieldMoldScaffold<TCloaked, TRevealBase, TCloaked[]> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked[] ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerSpan.AsSpan(), ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsAnyNullableClass |
                  SupportsValueRevealer)]
public class FieldCloakedBearerNullableClassSpanWhenNonNullAddAllStringBearer<TCloaked, TRevealBase> : 
    RevealerCollectionFieldMoldScaffold<TCloaked?, TRevealBase, TCloaked?[]> 
    where TCloaked : class, TRevealBase
    where TRevealBase : notnull
{
    public TCloaked?[] ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerNullableSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerNullableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerNullableSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerNullableSpan.AsSpan()
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsAnyNullableStruct |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerSpanWhenNonNullAddAllStringBearer<TCloakedStruct> : 
    RevealerCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerSpan.AsSpan(), ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsTypeNonNullable
                | AcceptsStringBearer)]
public class FieldStringBearerSpanWhenNonNullAddAllStringBearer<TBearer> : CollectionFieldMoldScaffold<TBearer, TBearer[]>
    where TBearer : IStringBearer
{
    public TBearer[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerNullableClassSpanWhenNonNullAddAllStringBearer<TBearer> : CollectionFieldMoldScaffold<TBearer?, TBearer?[]>
    where TBearer : class, IStringBearer
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsNullableStruct
                | AcceptsStringBearer)]
public class FieldNullableStringBearerSpanWhenNonNullAddAllStringBearer<TBearerStruct> : CollectionFieldMoldScaffold<TBearerStruct?,
    TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerSpan.AsSpan())
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsString 
                | AcceptsClass | SupportsValueFormatString)]
public class FieldStringSpanWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string, string[]>
{
    public string[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsString 
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringNullableSpanWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string?, string?[]>
  
{
    public string?[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsCharSequence
                | AcceptsClass | SupportsValueFormatString)]
public class FieldCharSequenceSpanWhenNonNullAddAllStringBearer<TCharSeq> : FormattedCollectionFieldMoldScaffold<TCharSeq, TCharSeq[]>
    where TCharSeq : ICharSequence
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsCharSequence
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceNullableSpanWhenNonNullAddAllStringBearer<TCharSeq> : FormattedCollectionFieldMoldScaffold<TCharSeq?, TCharSeq?[]>
   where TCharSeq : ICharSequence
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceNullableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeqNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceNullableSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceNullableSpan.AsSpan()
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsStringBuilder
                | AcceptsClass | SupportsValueFormatString)]
public class FieldStringBuilderSpanWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<StringBuilder, StringBuilder[]>
  
{
    public StringBuilder[] ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsStringBuilder
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderNullableSpanWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<StringBuilder?, StringBuilder?[]>
  
{
    public StringBuilder?[] ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsOnlyNonNullableGeneric
                | SupportsValueFormatString)]
public class FieldMatchSpanWhenNonNullAddAllStringBearer<TAny> : FormattedCollectionFieldMoldScaffold<TAny, TAny[]>
{
    public TAny[]? ComplexTypeCollectionFieldWhenNonNullAddAllMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllMatchSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsAnyGeneric
                | SupportsValueFormatString)]
public class FieldMatchNullableSpanWhenNonNullAddAllStringBearer<TAny> : FormattedCollectionFieldMoldScaffold<TAny?, TAny?[]>
  
{
    public TAny?[]? ComplexTypeCollectionFieldWhenNonNullAddAllMatchNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchNullableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchNullableSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllMatchNullableSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsNonNullableObject
                | SupportsValueFormatString)]
public class FieldObjectSpanWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object, object[]>
{
    public object[]? ComplexTypeCollectionFieldWhenNonNullAddAllObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllObject
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllObjectSpan.AsSpan(), ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | AcceptsNullableObject
                | SupportsValueFormatString)]
public class FieldNullableObjectSpanWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object?, object?[]>
  
{
    public object?[]? ComplexTypeCollectionFieldWhenNonNullAddAllObjectNullableRefSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectNullableRefSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllObjectNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectNullableRefSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddAllObjectNullableRefSpan.AsSpan()
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolReadOnlySpanWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool, bool[]>
{
    public bool[] ComplexTypeCollectionFieldWhenNonNullAddAllBoolReadOnlySpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolReadOnlySpan)
              , (ReadOnlySpan<bool>)ComplexTypeCollectionFieldWhenNonNullAddAllBoolReadOnlySpan
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolReadOnlySpanWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool?, bool?[]>
  
{
    public bool?[] ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolReadOnlySpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolReadOnlySpan)
              , (ReadOnlySpan<bool?>)ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolReadOnlySpan
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites
                | AcceptsNonNullableSpanFormattable | SupportsValueFormatString)]
public class FieldSpanFormattableReadOnlySpanWhenNonNullAddAllStringBearer<TFmt> : FormattedCollectionFieldMoldScaffold<TFmt, TFmt[]>
    where TFmt : ISpanFormattable
{
    public TFmt[] ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableReadOnlySpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmt>)ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableReadOnlySpan
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites
                | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassReadOnlySpanWhenNonNullAddAllStringBearer<TFmt> : FormattedCollectionFieldMoldScaffold<TFmt?, TFmt?[]>
   where TFmt : class, ISpanFormattable
{
    public TFmt?[] ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableNullableRefReadOnlySpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableNullableRefReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableNullableRefReadOnlySpan)
              , (ReadOnlySpan<TFmt?>)ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableNullableRefReadOnlySpan, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString | SupportsCustomHandling)]
public class FieldNullableSpanFormattableReadOnlySpanWhenNonNullAddAllStringBearer<TFmtStruct> :
    FormattedCollectionFieldMoldScaffold<TFmtStruct?, TFmtStruct?[]>
   where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmtStruct?>)ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableReadOnlySpan
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites
                | AcceptsAnyNonNullable | AcceptsStringBearer | SupportsValueRevealer)]
public class FieldCloakedBearerReadOnlySpanWhenNonNullAddAllStringBearer<TCloaked, TRevealBase> :
    RevealerCollectionFieldMoldScaffold<TCloaked, TRevealBase, TCloaked[]> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked[] ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerReadOnlySpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerReadOnlySpan);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloaked>)ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerReadOnlySpan
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsAnyNullableClass |
                  SupportsValueRevealer)]
public class FieldCloakedBearerNullableClassReadOnlySpanWhenNonNullAddAllStringBearer<TCloaked, TRevealBase> :
    RevealerCollectionFieldMoldScaffold<TCloaked?, TRevealBase, TCloaked?[]> 
    where TCloaked : class, TRevealBase
    where TRevealBase : notnull
{
    public TCloaked?[] ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerNullableSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerNullableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerNullableSpan)
              , (ReadOnlySpan<TCloaked?>)ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerNullableSpan.AsSpan()
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsAnyNullableStruct |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerReadOnlySpanWhenNonNullAddAllStringBearer<TCloakedStruct> :
    RevealerCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloakedStruct?>)ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerReadOnlySpan
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsTypeNonNullable
                | AcceptsStringBearer)]
public class FieldStringBearerReadOnlySpanWhenNonNullAddAllStringBearer<TBearer> : CollectionFieldMoldScaffold<TBearer, TBearer[]>
    where TBearer : IStringBearer
{
    public TBearer[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearer>)ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerReadOnlySpan)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsNullableClass
                | AcceptsStringBearer)]
public class
    FieldStringBearerNullableClassReadOnlySpanWhenNonNullAddAllStringBearer<TBearer> : CollectionFieldMoldScaffold<TBearer?, TBearer?[]>
    where TBearer : class, IStringBearer
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearer?>)ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerReadOnlySpan)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsNullableStruct
                | AcceptsStringBearer)]
public class FieldNullableStringBearerReadOnlySpanWhenNonNullAddAllStringBearer<TBearerStruct> : CollectionFieldMoldScaffold<TBearerStruct?,
    TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearerStruct?>)ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerReadOnlySpan)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsString
                | AcceptsClass | SupportsValueFormatString)]
public class FieldStringReadOnlySpanWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string, string[]>
{
    public string[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringReadOnlySpan)
              , (ReadOnlySpan<string>)ComplexTypeCollectionFieldWhenNonNullAddAllStringReadOnlySpan, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsString
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringNullableReadOnlySpanWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string?, string?[]>
  
{
    public string?[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringNullableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringNullableReadOnlySpan)
              , (ReadOnlySpan<string?>)ComplexTypeCollectionFieldWhenNonNullAddAllStringNullableReadOnlySpan
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsCharSequence
                | AcceptsClass | SupportsValueFormatString)]
public class FieldCharSequenceReadOnlySpanWhenNonNullAddAllStringBearer<TCharSeq> : FormattedCollectionFieldMoldScaffold<TCharSeq, TCharSeq[]>
    where TCharSeq : ICharSequence
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceReadOnlySpan)
              , (ReadOnlySpan<TCharSeq>)ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceReadOnlySpan
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | AcceptsCharSequence
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceNullableReadOnlySpanWhenNonNullAddAllStringBearer<TCharSeq> :
    FormattedCollectionFieldMoldScaffold<TCharSeq?, TCharSeq?[]>
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceNullableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeqNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceNullableSpan)
              , (ReadOnlySpan<TCharSeq?>)ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceNullableSpan
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | NonNullWrites | AcceptsArray | CallsAsReadOnlySpan | AcceptsStringBuilder
                | AcceptsClass | SupportsValueFormatString)]
public class FieldStringBuilderReadOnlySpanWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<StringBuilder, StringBuilder[]>
  
{
    public StringBuilder[] ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderReadOnlySpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderReadOnlySpan)
              , (ReadOnlySpan<StringBuilder>)ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderReadOnlySpan
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | NonNullWrites | AcceptsArray | CallsAsReadOnlySpan | AcceptsStringBuilder
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderNullableReadOnlySpanWhenNonNullAddAllStringBearer :
    FormattedCollectionFieldMoldScaffold<StringBuilder?, StringBuilder?[]>
  
{
    public StringBuilder?[] ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderNullableReadOnlySpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderNullableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderNullableReadOnlySpan)
              , (ReadOnlySpan<StringBuilder?>)ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderNullableReadOnlySpan
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | NonNullWrites | AcceptsArray | CallsAsReadOnlySpan | AcceptsOnlyNonNullableGeneric
                | SupportsValueFormatString)]
public class FieldMatchReadOnlySpanWhenNonNullAddAllStringBearer<TAny> : FormattedCollectionFieldMoldScaffold<TAny, TAny[]>
  
{
    public TAny[]? ComplexTypeCollectionFieldWhenNonNullAddAllMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchReadOnlySpan)
              , (ReadOnlySpan<TAny>)ComplexTypeCollectionFieldWhenNonNullAddAllMatchReadOnlySpan, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | NonNullWrites | AcceptsArray | CallsAsReadOnlySpan | AcceptsAnyGeneric
                | SupportsValueFormatString)]
public class FieldMatchNullableReadOnlySpanWhenNonNullAddAllStringBearer<TAny> : FormattedCollectionFieldMoldScaffold<TAny?, TAny?[]>
  
{
    public TAny?[]? ComplexTypeCollectionFieldWhenNonNullAddAllMatchNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchNullableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchNullableReadOnlySpan)
              , (ReadOnlySpan<TAny?>)ComplexTypeCollectionFieldWhenNonNullAddAllMatchNullableReadOnlySpan
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | NonNullWrites | AcceptsArray | CallsAsReadOnlySpan | AcceptsNullableObject
                | SupportsValueFormatString)]
public class FieldObjectReadOnlySpanWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object, object[]>
{
    public object[]? ComplexTypeCollectionFieldWhenNonNullAddAllObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllObject
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectSpan)
              , (ReadOnlySpan<object>)ComplexTypeCollectionFieldWhenNonNullAddAllObjectSpan, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | NonNullWrites | AcceptsArray | CallsAsReadOnlySpan | AcceptsNonNullableObject
                | SupportsValueFormatString)]
public class FieldNullableObjectReadOnlySpanWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object?, object?[]>
  
{
    public object?[]? ComplexTypeCollectionFieldWhenNonNullAddAllObjectNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectNullableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllObjectNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectNullableReadOnlySpan)
              , (ReadOnlySpan<object?>)ComplexTypeCollectionFieldWhenNonNullAddAllObjectNullableReadOnlySpan
              , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | NonNullWrites | AcceptsArray | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolArrayWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool, bool[]>
{
    public bool[]? ComplexTypeCollectionFieldWhenNonNullAddAllBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolArray)
              , ComplexTypeCollectionFieldWhenNonNullAddAllBoolArray, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | NonNullWrites | AcceptsArray | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolArrayWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool?, bool?[]>
{
    public bool?[]? ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolArray)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolArray, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | NonNullWrites | AcceptsArray | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class FieldSpanFormattableArrayWhenNonNullAddAllStringBearer<TFmt> : FormattedCollectionFieldMoldScaffold<TFmt, TFmt[]>
    where TFmt : ISpanFormattable
{
    public TFmt[]? ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll(nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableArray)
                                            , ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableArray
                                            , ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | NonNullWrites | AcceptsArray | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class FieldNullableSpanFormattableArrayWhenNonNullAddAllStringBearer<TStructFmt> :
    FormattedCollectionFieldMoldScaffold<TStructFmt?, TStructFmt?[]>
   where TStructFmt : struct, ISpanFormattable
{
    public TStructFmt?[]? ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableArray)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableArray, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | NonNullWrites | AcceptsArray | AcceptsAnyExceptNullableStruct | SupportsValueRevealer)]
public class FieldCloakedBearerArrayWhenNonNullAddAllStringBearer<TCloaked, TRevealBase> : 
    RevealerCollectionFieldMoldScaffold<TCloaked, TRevealBase, TCloaked[]> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerArray
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullWrites | AcceptsAnyNullableStruct | SupportsValueRevealer)]
public class FieldNullableCloakedBearerArrayWhenNonNullAddAllStringBearer<TCloakedStruct> :
    RevealerCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerArray
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullWrites | AcceptsTypeAllButNullableStruct | AcceptsStringBearer)]
public class FieldStringBearerArrayWhenNonNullAddAllStringBearer<TBearer> : CollectionFieldMoldScaffold<TBearer, TBearer[]>
    where TBearer : IStringBearer
{
    public TBearer[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll(nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerArray)
                                               , ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerArray)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerArrayWhenNonNullAddAllStringBearer<TBearerStruct> : CollectionFieldMoldScaffold<TBearerStruct?,
    TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerArray)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerArray)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullWrites | AcceptsString 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringArrayWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string?, string?[]>
{
    public string?[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringArray)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringArray, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullWrites | AcceptsCharSequence 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceArrayWhenNonNullAddAllStringBearer<TCharSeq> : FormattedCollectionFieldMoldScaffold<TCharSeq?, TCharSeq?[]>
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceArray)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceArray, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullWrites | AcceptsStringBuilder 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderArrayWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<StringBuilder?, StringBuilder?[]>
  
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderArray)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderArray, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullWrites | AcceptsAnyGeneric 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldMatchArrayWhenNonNullAddAllStringBearer<TAny> : FormattedCollectionFieldMoldScaffold<TAny, TAny[]>
{
    public TAny[]? ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderArray)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderArray, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullWrites | AcceptsNullableObject | SupportsValueFormatString)]
public class FieldObjectArrayWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object?, object?[]>
{
    public object?[]? ComplexTypeCollectionFieldWhenNonNullAddAllObjectArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllObject
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectArray)
              , ComplexTypeCollectionFieldWhenNonNullAddAllObjectArray, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolListWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool, IReadOnlyList<bool>>
{
    public IReadOnlyList<bool>? ComplexTypeCollectionFieldWhenNonNullAddAllBoolList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllBoolList, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolListWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool?, IReadOnlyList<bool?>>
  
{
    public IReadOnlyList<bool?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolList, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class FieldSpanFormattableListWhenNonNullAddAllStringBearer<TFmt> : FormattedCollectionFieldMoldScaffold<TFmt, IReadOnlyList<TFmt>>
   where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt>? ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableList, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class FieldNullableSpanFormattableListWhenNonNullAddAllStringBearer<TFmtStruct> :
    FormattedCollectionFieldMoldScaffold<TFmtStruct?, IReadOnlyList<TFmtStruct?>>
   where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableList, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | AcceptsAnyExceptNullableStruct | SupportsValueRevealer)]
public class FieldCloakedBearerListWhenNonNullAddAllStringBearer<TCloaked, TRevealBase> :
    RevealerCollectionFieldMoldScaffold<TCloaked, TRevealBase, IReadOnlyList<TCloaked>> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public IReadOnlyList<TCloaked>? ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerList, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | AcceptsAnyNullableStruct | SupportsValueRevealer)]
public class FieldNullableCloakedBearerListWhenNonNullAddAllStringBearer<TCloakedStruct> :
    RevealerCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct, IReadOnlyList<TCloakedStruct?>> where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerList, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | AcceptsTypeAllButNullableStruct | AcceptsStringBearer)]
public class FieldStringBearerListWhenNonNullAddAllStringBearer<TBearer> : CollectionFieldMoldScaffold<TBearer, IReadOnlyList<TBearer>>
    where TBearer : IStringBearer
{
    public IReadOnlyList<TBearer>? ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerList)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerListWhenNonNullAddAllStringBearer<TBearerStruct> : CollectionFieldMoldScaffold<TBearerStruct?,
    IReadOnlyList<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerList)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | AcceptsString 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringListWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string?, IReadOnlyList<string?>>
  
{
    public IReadOnlyList<string?>? ComplexTypeCollectionFieldWhenNonNullAddAllStringList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringList, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | AcceptsCharSequence 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceListWhenNonNullAddAllStringBearer<TCharSeq> : FormattedCollectionFieldMoldScaffold<TCharSeq?, IReadOnlyList<TCharSeq?>>
   where TCharSeq : ICharSequence
{
    public IReadOnlyList<TCharSeq?>? ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceList, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | AcceptsStringBuilder 
               | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderListWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<StringBuilder?, IReadOnlyList<StringBuilder?>>
  
{
    public IReadOnlyList<StringBuilder?>? ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderList, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | NonNullWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchListWhenNonNullAddAllStringBearer<TAny> : FormattedCollectionFieldMoldScaffold<TAny, IReadOnlyList<TAny>>
  
{
    public IReadOnlyList<TAny>? ComplexTypeCollectionFieldWhenNonNullAddAllMatchList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatch
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllMatchList, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsNullableObject | NonNullWrites | SupportsValueFormatString)]
public class FieldObjectListWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object?, IReadOnlyList<object?>>
  
{
    public IReadOnlyList<object?>? ComplexTypeCollectionFieldWhenNonNullAddAllObjectList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllObject
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectList)
              , ComplexTypeCollectionFieldWhenNonNullAddAllObjectList, ValueFormatString)
           .Complete();
}
