// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    OrderedCollectionFields;

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolSpanWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool, bool[]>
  
{
    public bool[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolSpanWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool?, bool?[]>
  
{
    public bool?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | NonNullWrites | AcceptsArray | CallsAsSpan | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableSpanWhenNonNullAddFilteredStringBearer<TFmt, TFmtBase> : FormattedFilteredCollectionFieldMoldScaffold<TFmt, TFmt[]>
   where TFmt : ISpanFormattable?, TFmtBase?
{
    public TFmt[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate
                | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassSpanWhenNonNullAddFilteredStringBearer<TFmt, TFmtBase> :
    FormattedFilteredCollectionFieldMoldScaffold<TFmt?, TFmtBase?, TFmt?[]>
   where TFmt : class, ISpanFormattable, TFmtBase
{
    public TFmt?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableNullableSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableNullableSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableNullableSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableSpanWhenNonNullAddFilteredStringBearer<TFmtStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmtStruct?, TFmtStruct?[]>
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate
                | AcceptsTypeAllButNullableStruct | SupportsValueRevealer)]
public class FieldCloakedBearerSpanWhenNonNullAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase> :
    RevealerFilteredCollectionFieldMoldScaffold<TCloaked, TCloakedFilterBase, TCloakedRevealBase, TCloaked[]>
    where TCloaked : TCloakedRevealBase?, TCloakedFilterBase?
    where TCloakedRevealBase : notnull
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerSpan.AsSpan()
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate
                | AcceptsAnyNullableClass | SupportsValueRevealer)]
public class FieldCloakedBearerNullableClassSpanWhenNonNullAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase> :
    RevealerFilteredCollectionFieldMoldScaffold<TCloaked?, TCloakedFilterBase?, TCloakedRevealBase, TCloaked?[]>
    where TCloaked : class, TCloakedRevealBase, TCloakedFilterBase
    where TCloakedRevealBase : notnull
{
    public TCloaked?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerNullableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerNullableSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerNullableSpan.AsSpan()
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate
                | AcceptsAnyNullableStruct | SupportsValueRevealer)]
public class FieldNullableCloakedBearerSpanWhenNonNullAddFilteredStringBearer<TCloakedStruct> :
    RevealerFilteredCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]>
   where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerSpan.AsSpan()
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate
                | AcceptsTypeAllButNullableStruct | AcceptsStringBearer)]
public class FieldStringBearerSpanWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>
    : FilteredCollectionFieldMoldScaffold<TBearer, TBearer[]>
    where TBearer : IStringBearer?, TBearerBase?
{
    public TBearer[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerSpan.AsSpan(), ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsNullableClass
                | AcceptsStringBearer)]
public class FieldStringBearerNullableClassSpanWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>
    : FilteredCollectionFieldMoldScaffold<TBearer?, TBearer?[]>
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerNullableSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerNullableSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerNullableSpan.AsSpan()
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsStringBearer)]
public class FieldNullableStringBearerSpanWhenNonNullAddFilteredStringBearer<TBearerStruct>
    : FilteredCollectionFieldMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerSpan.AsSpan()
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsString
                | AcceptsClass | SupportsValueFormatString)]
public class FieldStringSpanWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<string, string[]>
  
{
    public string[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsString
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringNullableSpanWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<string?, string?[]>
  
{
    public string?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenNonNullAddFilteredNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceSpanWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TCharSeq, TCharSeq[]>
    where TCharSeq : ICharSequence?, TCharSeqFilterBase?
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeq
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsCharSequence
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceNullableSpanWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TCharSeq?, TCharSeq?[]>
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCharSequenceSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeq
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCharSequenceSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCharSequenceSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsStringBuilder
                | AcceptsClass | SupportsValueFormatString)]
public class FieldStringBuilderSpanWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder, StringBuilder[]>
  
{
    public StringBuilder[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsStringBuilder
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderNullableSpanWhenNonNullAddFilteredStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder?, StringBuilder?[]>
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBuilderSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBuilderSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBuilderSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBuilderSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate
                | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchSpanWhenNonNullAddFilteredStringBearer<TAny, TAnyFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TAny, TAny[]>
    where TAny : TAnyFilterBase?
{
    public TAny[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredMatch
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchSpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsNonNullableObject
                | SupportsValueFormatString)]
public class FieldObjectSpanWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<object, object[]>
  
{
    public object[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObject
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectSpan.AsSpan()
              , ElementPredicate, ValueFormatString);
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | NonNullWrites | FilterPredicate | AcceptsNullableObject
                | SupportsValueFormatString)]
public class FieldNullableObjectSpanWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<object?, object?[]>
  
{
    public object?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableObjectSpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObjectNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableObjectSpan)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableObjectSpan.AsSpan()
              , ElementPredicate, ValueFormatString);
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolReadOnlySpanWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool, bool[]>
  
{
    public bool[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolReadOnlySpan)
              , (ReadOnlySpan<bool>)ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate
                | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolReadOnlySpanWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool?, bool?[]>
  
{
    public bool?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolReadOnlySpan)
              , (ReadOnlySpan<bool?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableReadOnlySpanWhenNonNullAddFilteredStringBearer<TFmt, TFmtBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmt, TFmt[]>
    where TFmt : ISpanFormattable?, TFmtBase?
{
    public TFmt[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmt>)ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate
                | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassReadOnlySpanWhenNonNullAddFilteredStringBearer<TFmt, TFmtBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmt?, TFmtBase?, TFmt?[]>
    where TFmt : class, ISpanFormattable, TFmtBase
{
    public TFmt?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableNullableReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableNullableReadOnlySpan)
              , (ReadOnlySpan<TFmt?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableNullableReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableReadOnlySpanWhenNonNullAddFilteredStringBearer<TFmtStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmtStruct?, TFmtStruct?[]>
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmtStruct?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate
                | AcceptsTypeAllButNullableStruct | SupportsValueRevealer)]
public class FieldCloakedBearerReadOnlySpanWhenNonNullAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : RevealerFilteredCollectionFieldMoldScaffold<TCloaked, TCloakedFilterBase, TCloakedRevealBase, TCloaked[]>
    where TCloaked : TCloakedRevealBase?, TCloakedFilterBase?
    where TCloakedRevealBase : notnull
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerReadOnlySpan);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloaked>)ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerReadOnlySpan
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate
                | AcceptsAnyNullableClass | SupportsValueRevealer)]
public class FieldCloakedBearerNullableClassReadOnlySpanWhenNonNullAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloaked?, TCloakedFilterBase?, TCloakedRevealBase, TCloaked?[]>
    where TCloaked : class, TCloakedRevealBase, TCloakedFilterBase
    where TCloakedRevealBase : notnull
{
    public TCloaked?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerNullableReadOnlySpan);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerNullableReadOnlySpan)
              , (ReadOnlySpan<TCloaked?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerNullableReadOnlySpan
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate
                | AcceptsAnyNullableStruct | SupportsValueRevealer)]
public class FieldNullableCloakedBearerReadOnlySpanWhenNonNullAddFilteredStringBearer<TCloakedStruct> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerReadOnlySpan);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloakedStruct?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerReadOnlySpan
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate
                | AcceptsTypeAllButNullableStruct | AcceptsStringBearer)]
public class FieldStringBearerReadOnlySpanWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>
    : FilteredCollectionFieldMoldScaffold<TBearer, TBearer[]>
    where TBearer : IStringBearer?, TBearerBase?
{
    public TBearer[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearer>)ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerReadOnlySpan
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate
                | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerNullableClassReadOnlySpanWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>
    : FilteredCollectionFieldMoldScaffold<TBearer?, TBearer?[]>
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerNullableReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerNullableReadOnlySpan)
              , (ReadOnlySpan<TBearer?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerNullableReadOnlySpan
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate
                | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerReadOnlySpanWhenNonNullAddFilteredStringBearer<TBearerStruct>
    : FilteredCollectionFieldMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerReadOnlySpan),
                (ReadOnlySpan<TBearerStruct?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerReadOnlySpan
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsString
                | AcceptsClass | SupportsValueFormatString)]
public class FieldStringReadOnlySpanWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<string, string[]>
  
{
    public string[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringReadOnlySpan)
              , (ReadOnlySpan<string>)ComplexTypeCollectionFieldWhenNonNullAddFilteredStringReadOnlySpan.AsSpan()
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate | AcceptsString
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringNullableReadOnlySpanWhenNonNullAddFilteredStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<string?, string?[]>
{
    public string?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringReadOnlySpan),
                (ReadOnlySpan<string?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate
                | AcceptsCharSequence | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceReadOnlySpanWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TCharSeq, TCharSeq[]>
    where TCharSeq : ICharSequence?, TCharSeqFilterBase?
{
    public TCharSeq[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeq
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceReadOnlySpan)
              , (ReadOnlySpan<TCharSeq>)ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate
                | AcceptsCharSequence | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceNullableReadOnlySpanWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TCharSeq?, TCharSeq?[]>
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCharSequenceReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeq
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCharSequenceReadOnlySpan),
                (ReadOnlySpan<TCharSeq?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCharSequenceReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate
                | AcceptsClass | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderReadOnlySpanWhenNonNullAddFilteredStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder, StringBuilder[]>
{
    public StringBuilder[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderReadOnlySpan)
              , (ReadOnlySpan<StringBuilder>)ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate
                | AcceptsNullableClass | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderNullableReadOnlySpanWhenNonNullAddFilteredStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder?, StringBuilder?[]>
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBuilderReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBuilderReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBuilderReadOnlySpan)
              , (ReadOnlySpan<StringBuilder?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBuilderReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate
                | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchReadOnlySpanWhenNonNullAddFilteredStringBearer<TAny, TAnyFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TAny, TAny[]>
    where TAny : TAnyFilterBase?
{
    public TAny[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredMatch
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchReadOnlySpan)
              , (ReadOnlySpan<TAny>)ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchReadOnlySpan
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate
                | AcceptsNonNullableObject | SupportsValueFormatString)]
public class FieldObjectReadOnlySpanWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<object, object[]>
  
{
    public object[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObject
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectReadOnlySpan)
              , (ReadOnlySpan<object>)ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectReadOnlySpan
              , ElementPredicate, ValueFormatString);
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | NonNullWrites | FilterPredicate
                | AcceptsNullableObject | SupportsValueFormatString)]
public class FieldNullableObjectReadOnlySpanWhenNonNullAddFilteredStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<object?, object?[]>
{
    public object?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableObjectReadOnlySpan);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObjectNullable
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableObjectReadOnlySpan)
              , (ReadOnlySpan<object?>)ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableObjectReadOnlySpan
              , ElementPredicate, ValueFormatString);
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolArrayWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool, bool[]>
  
{
    public bool[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolArray, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolArrayWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool?, bool?[]>
  
{
    public bool?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolArray, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullWrites | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableArrayWhenNonNullAddFilteredStringBearer<TFmt, TFmtBase> :
    FormattedFilteredCollectionFieldMoldScaffold<TFmt?, TFmt?[]>
   where TFmt : ISpanFormattable, TFmtBase
{
    public TFmt?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableArray
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullWrites | FilterPredicate
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableArrayWhenNonNullAddFilteredStringBearer<TFmtStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmtStruct?, TFmtStruct?[]>
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableArray
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct |
                  SupportsValueRevealer)]
public class FieldCloakedBearerArrayWhenNonNullAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
    : RevealerFilteredCollectionFieldMoldScaffold<TCloaked, TCloakedFilterBase, TCloakedRevealBase, TCloaked[]>
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
    where TCloakedRevealBase : notnull
{
    public TCloaked[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerArray);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerArray, ElementPredicate
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsAnyNullableStruct |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerArrayWhenNonNullAddFilteredStringBearer<TCloakedStruct> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerArray);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerArray
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsTypeAllButNullableStruct
                | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerArrayWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>
    : FilteredCollectionFieldMoldScaffold<TBearer?, TBearer?[]>
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerArray, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsStringBearer)]
public class FieldNullableStringBearerArrayWhenNonNullAddFilteredStringBearer<TBearerStruct>
    : FilteredCollectionFieldMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerArray
              , ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsString
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringArrayWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<string?, string?[]>
  
{
    public string?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringArray
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceArrayWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TCharSeq?, TCharSeq?[]>
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeq
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceArray
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsStringBuilder
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderArrayWhenNonNullAddFilteredStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder?, StringBuilder?[]>
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderArray
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString)]
public class FieldMatchArrayWhenNonNullAddFilteredStringBearer<TAny, TAnyFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TAny?, TAny?[]>
    where TAny : TAnyFilterBase
{
    public TAny?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenNonNullAddFilteredMatch
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchArray
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsNullableObject
                     | SupportsValueFormatString)]
public class FieldObjectArrayWhenNonNullAddFilteredStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<object?, object?[]>
{
    public object?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectArray);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObject
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectArray)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectArray, ElementPredicate, ValueFormatString);
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolListWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool, IReadOnlyList<bool>>
  
{
    public IReadOnlyList<bool>? ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredBoolList, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolListWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<bool?, IReadOnlyList<bool?>>
  
{
    public IReadOnlyList<bool?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableBoolList, ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableListWhenNonNullAddFilteredStringBearer<TFmt, TFmtBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmt?, IReadOnlyList<TFmt?>>
    where TFmt : ISpanFormattable, TFmtBase
{
    public IReadOnlyList<TFmt?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredSpanFormattableList
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableListWhenNonNullAddFilteredStringBearer<TFmtStruct>
    : FormattedFilteredCollectionFieldMoldScaffold<TFmtStruct?, IReadOnlyList<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableSpanFormattableList
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct |
                  SupportsValueRevealer)]
public class FieldCloakedBearerListWhenNonNullAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase> : 
    RevealerFilteredCollectionFieldMoldScaffold<TCloaked, TCloakedFilterBase, TCloakedRevealBase, IReadOnlyList<TCloaked>>
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
    where TCloakedRevealBase : notnull
{
    public IReadOnlyList<TCloaked>? ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCloakedBearerList, ElementPredicate
              , ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyNullableStruct
                | SupportsValueRevealer)]
public class FieldNullableCloakedBearerListWhenNonNullAddFilteredStringBearer<TCloakedStruct> :
    RevealerFilteredCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct?, TCloakedStruct, IReadOnlyList<TCloakedStruct?>>
    where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableCloakedBearerList
              , ElementPredicate, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsTypeAllButNullableStruct
                | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerListWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>
    : FilteredCollectionFieldMoldScaffold<TBearer?, IReadOnlyList<TBearer?>>
    where TBearer : IStringBearer, TBearerBase
{
    public IReadOnlyList<TBearer?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | AcceptsStringBearer)]
public class FieldNullableStringBearerListWhenNonNullAddFilteredStringBearer<TBearerStruct>
    : FilteredCollectionFieldMoldScaffold<TBearerStruct?, IReadOnlyList<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredNullableStringBearerList, ElementPredicate)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsString
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringListWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<string?, IReadOnlyList<string?>>
  
{
    public IReadOnlyList<string?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringList
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceListWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TCharSeq, TCharSeqFilterBase, IReadOnlyList<TCharSeq>>
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IReadOnlyList<TCharSeq?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeq
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredCharSequenceList
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsStringBuilder
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderListWhenNonNullAddFilteredStringBearer
    : FormattedFilteredCollectionFieldMoldScaffold<StringBuilder?, StringBuilder?[]>
{
    public StringBuilder?[]? ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredStringBuilderList
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString)]
public class FieldMatchListWhenNonNullAddFilteredStringBearer<TAny, TAnyFilterBase>
    : FormattedFilteredCollectionFieldMoldScaffold<TAny?, IReadOnlyList<TAny?>>
    where TAny : TAnyFilterBase
{
    public IReadOnlyList<TAny?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredMatch
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredMatchList
              , ElementPredicate, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableObject
                | SupportsValueFormatString)]
public class FieldObjectListWhenNonNullAddFilteredStringBearer : FormattedFilteredCollectionFieldMoldScaffold<object?, IReadOnlyList<object?>>
  
{
    public IReadOnlyList<object?>? ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList);


    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObject
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList)
              , ComplexTypeCollectionFieldWhenNonNullAddFilteredObjectList
              , ElementPredicate, ValueFormatString);
}
