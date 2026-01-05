// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField.
    FixtureScaffolding;

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolEnumerableWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool, IEnumerable<bool>>
  
{
    public IEnumerable<bool>? ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolEnumerableWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool?, IEnumerable<bool?>>
  
{
    public IEnumerable<bool?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableEnumerableWhenPopulatedAddAllStringBearer<TFmt> : FormattedCollectionFieldMoldScaffold<TFmt?, IEnumerable<TFmt?>>
    where TFmt : ISpanFormattable
{
    public IEnumerable<TFmt?>? ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumerableWhenPopulatedAddAllStringBearer<TFmtStruct> :
    FormattedCollectionFieldMoldScaffold<TFmtStruct?, IEnumerable<TFmtStruct?>>
   where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsAnyExceptNullableStruct
                | AcceptsStringBearer | SupportsValueRevealer)]
public class FieldCloakedBearerEnumerableWhenPopulatedAddAllStringBearer<TCloaked, TRevealBase> :
    RevealerCollectionFieldMoldScaffold<TCloaked, TRevealBase, IEnumerable<TCloaked>?>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public IEnumerable<TCloaked>? ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerable);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerable, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsAnyNullableStruct |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerEnumerableWhenPopulatedAddAllStringBearer<TCloakedStruct> :
    RevealerCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct, IEnumerable<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerEnumerable);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerEnumerable, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsTypeAllButNullableStruct
                | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerEnumerableWhenPopulatedAddAllStringBearer<TBearer> : CollectionFieldMoldScaffold<TBearer, IEnumerable<TBearer>>
    where TBearer : IStringBearer
{
    public IEnumerable<TBearer>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerable)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsNullableStruct
                | AcceptsStringBearer)]
public class FieldNullableStringBearerEnumerableWhenPopulatedAddAllStringBearer<TBearerStruct> : CollectionFieldMoldScaffold<TBearerStruct?,
    IEnumerable<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerable)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsString
                | AcceptsClass | AcceptsNullableClass  | SupportsValueFormatString)]
public class FieldStringEnumerableWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string?, IEnumerable<string?>>
  
{
    public IEnumerable<string?>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceEnumerableWhenPopulatedAddAllStringBearer<TCharSeq> :
    FormattedCollectionFieldMoldScaffold<TCharSeq?, IEnumerable<TCharSeq?>>
    where TCharSeq : ICharSequence
{
    public IEnumerable<TCharSeq?>? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsStringBuilder
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderEnumerableWhenPopulatedAddAllStringBearer :
    FormattedCollectionFieldMoldScaffold<StringBuilder?, IEnumerable<StringBuilder?>>
  
{
    public IEnumerable<StringBuilder?>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsAnyGeneric
                | SupportsValueFormatString)]
public class FieldMatchEnumerableWhenPopulatedAddAllStringBearer<TAny> : FormattedCollectionFieldMoldScaffold<TAny, IEnumerable<TAny>>
  
{
    public IEnumerable<TAny>? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullAndPopulatedWrites | AcceptsNullableObject
                | SupportsValueFormatString)]
public class FieldObjectEnumerableWhenPopulatedAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object?, IEnumerable<object?>>
  
{
    public IEnumerable<object?>? ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolEnumeratorWhenPopulatedAddAllStringBearer : FormattedEnumeratorFieldMoldScaffold<bool, IEnumerator<bool>>
{
    public IEnumerator<bool>? ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolEnumeratorWhenPopulatedAddAllStringBearer : FormattedEnumeratorFieldMoldScaffold<bool?, IEnumerator<bool?>>
  
{
    public IEnumerator<bool?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableEnumeratorWhenPopulatedAddAllStringBearer<TFmt> : FormattedEnumeratorFieldMoldScaffold<TFmt, IEnumerator<TFmt>>
   where TFmt : ISpanFormattable
{
    public IEnumerator<TFmt>? ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumeratorWhenPopulatedAddAllStringBearer<TStructFmt> :
    FormattedEnumeratorFieldMoldScaffold<TStructFmt?, IEnumerator<TStructFmt?>>
   where TStructFmt : struct, ISpanFormattable
{
    public IEnumerator<TStructFmt?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsAnyExceptNullableStruct |
                  SupportsValueRevealer)]
public class FieldCloakedBearerEnumeratorWhenPopulatedAddAllStringBearer<TCloaked, TRevealBase> :
    RevealerEnumeratorFieldMoldScaffold<TCloaked, TRevealBase, IEnumerator<TCloaked>?> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public IEnumerator<TCloaked>? ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerator);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerator, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsAnyNullableStruct |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerEnumeratorWhenPopulatedAddAllStringBearer<TCloakedStruct> :
    RevealerEnumeratorFieldMoldScaffold<TCloakedStruct?, TCloakedStruct, IEnumerator<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerEnumerator);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerEnumerator, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsTypeAllButNullableStruct
                | AcceptsNullableClass | AcceptsStringBearer)]
public class FieldStringBearerEnumeratorWhenPopulatedAddAllStringBearer<TBearer> : EnumeratorFieldMoldScaffold<TBearer?, IEnumerator<TBearer?>>
    where TBearer : IStringBearer
{
    public IEnumerator<TBearer?>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerator)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsNullableStruct
                | AcceptsStringBearer)]
public class FieldNullableStringBearerEnumeratorWhenPopulatedAddAllStringBearer<TBearerStruct> : EnumeratorFieldMoldScaffold<TBearerStruct?,
    IEnumerator<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerator)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsString
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringEnumeratorWhenPopulatedAddAllStringBearer : FormattedEnumeratorFieldMoldScaffold<string?, IEnumerator<string?>>
  
{
    public IEnumerator<string?>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceEnumeratorWhenPopulatedAddAllStringBearer<TCharSeq> : FormattedEnumeratorFieldMoldScaffold<TCharSeq?, IEnumerator<TCharSeq?>>
   where TCharSeq : ICharSequence
{
    public IEnumerator<TCharSeq?>? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsStringBuilder
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderEnumeratorWhenPopulatedAddAllStringBearer :
    FormattedEnumeratorFieldMoldScaffold<StringBuilder?, IEnumerator<StringBuilder?>>
  
{
    public IEnumerator<StringBuilder?>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsAnyGeneric
                | SupportsValueFormatString)]
public class FieldMatchEnumeratorWhenPopulatedAddAllStringBearer<TAny> : FormattedEnumeratorFieldMoldScaffold<TAny, IEnumerator<TAny>>
  
{
    public IEnumerator<TAny>? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullAndPopulatedWrites | AcceptsNullableObject
                | SupportsValueFormatString)]
public class FieldObjectEnumeratorWhenPopulatedAddAllStringBearer : FormattedEnumeratorFieldMoldScaffold<object, IEnumerator<object>>
  
{
    public IEnumerator<object?>? ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerator
    {
        get => Value;
        set => Value = value!;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerator, ValueFormatString)
           .Complete();
}
