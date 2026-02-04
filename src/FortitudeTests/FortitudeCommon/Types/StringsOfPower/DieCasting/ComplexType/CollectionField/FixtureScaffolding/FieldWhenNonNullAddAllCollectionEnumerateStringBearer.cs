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

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolEnumerableWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool, IEnumerable<bool>>
  
{
    public IEnumerable<bool>? ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolEnumerableWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool?, IEnumerable<bool?>>
  
{
    public IEnumerable<bool?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class FieldSpanFormattableEnumerableWhenNonNullAddAllStringBearer<TFmt> : FormattedCollectionFieldMoldScaffold<TFmt, IEnumerable<TFmt>>
   where TFmt : ISpanFormattable
{
    public IEnumerable<TFmt>? ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumerableWhenNonNullAddAllStringBearer<TStructFmt> :
    FormattedCollectionFieldMoldScaffold<TStructFmt?, IEnumerable<TStructFmt?>>
   where TStructFmt : struct, ISpanFormattable
{
    public IEnumerable<TStructFmt?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyExceptNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerEnumerableWhenNonNullAddAllStringBearer<TCloaked, TRevealBase> : 
    RevealerCollectionFieldMoldScaffold<TCloaked?, TRevealBase, IEnumerable<TCloaked?>?> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public IEnumerable<TCloaked?>? ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerable
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyNullableStruct 
                | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerEnumerableWhenNonNullAddAllStringBearer<TCloakedStruct> : 
    RevealerCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct, IEnumerable<TCloakedStruct?>?> where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerable
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsTypeAllButNullableStruct
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerEnumerableWhenNonNullAddAllStringBearer<TBearer> : FormattedCollectionFieldMoldScaffold<TBearer, IEnumerable<TBearer>>
    where TBearer : IStringBearer
{
    public IEnumerable<TBearer>? ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsNullableStruct 
                | AcceptsStringBearer  | SupportsValueFormatString)]
public class FieldNullableStringBearerEnumerableWhenNonNullAddAllStringBearer<TBearerStruct> :
    FormattedCollectionFieldMoldScaffold<TBearerStruct?, IEnumerable<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsString 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringEnumerableWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string?, IEnumerable<string?>>
  
{
    public IEnumerable<string?>? ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsCharSequence 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceEnumerableWhenNonNullAddAllStringBearer<TCharSeq> :
    FormattedCollectionFieldMoldScaffold<TCharSeq?, IEnumerable<TCharSeq?>>
   where TCharSeq : ICharSequence
{
    public IEnumerable<TCharSeq?>? ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsStringBuilder 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderEnumerableWhenNonNullAddAllStringBearer :
    FormattedCollectionFieldMoldScaffold<StringBuilder?, IEnumerable<StringBuilder?>>
  
{
    public IEnumerable<StringBuilder?>? ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchEnumerableWhenNonNullAddAllStringBearer<TAny> : FormattedCollectionFieldMoldScaffold<TAny, IEnumerable<TAny>>
  
{
    public IEnumerable<TAny>? ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldObjectEnumerableWhenNonNullAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object?, IEnumerable<object?>>
  
{
    public IEnumerable<object?>? ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolEnumeratorWhenNonNullAddAllStringBearer : FormattedEnumeratorFieldMoldScaffold<bool, IEnumerator<bool>>
  
{
    public IEnumerator<bool>? ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolEnumeratorWhenNonNullAddAllStringBearer : FormattedEnumeratorFieldMoldScaffold<bool?, IEnumerator<bool?>>
  
{
    public IEnumerator<bool?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class FieldSpanFormattableEnumeratorWhenNonNullAddAllStringBearer<TFmt> : FormattedEnumeratorFieldMoldScaffold<TFmt, IEnumerator<TFmt>>
   where TFmt : ISpanFormattable
{
    public IEnumerator<TFmt>? ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumeratorWhenNonNullAddAllStringBearer<TFmtStruct> :
    FormattedEnumeratorFieldMoldScaffold<TFmtStruct?, IEnumerator<TFmtStruct?>>
   where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyExceptNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerEnumeratorWhenNonNullAddAllStringBearer<TCloaked, TRevealBase> :
    RevealerEnumeratorFieldMoldScaffold<TCloaked?, TRevealBase, IEnumerator<TCloaked?>?> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public IEnumerator<TCloaked?>? ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerator
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerEnumeratorWhenNonNullAddAllStringBearer<TCloakedStruct> :
    RevealerEnumeratorFieldMoldScaffold<TCloakedStruct?, TCloakedStruct, IEnumerator<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerator
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsTypeAllButNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerEnumeratorWhenNonNullAddAllStringBearer<TBearer> : FormattedEnumeratorFieldMoldScaffold<TBearer, IEnumerator<TBearer>>
    where TBearer : IStringBearer
{
    public IEnumerator<TBearer>? ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerEnumeratorWhenNonNullAddAllStringBearer<TBearerStruct> :
    FormattedEnumeratorFieldMoldScaffold<TBearerStruct?, IEnumerator<TBearerStruct?>> where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerator)
               , ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsString 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringEnumeratorWhenNonNullAddAllStringBearer : FormattedEnumeratorFieldMoldScaffold<string?, IEnumerator<string?>>
  
{
    public IEnumerator<string?>? ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsCharSequence 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceEnumeratorWhenNonNullAddAllStringBearer<TCharSeq> :
    FormattedEnumeratorFieldMoldScaffold<TCharSeq?, IEnumerator<TCharSeq?>>
   where TCharSeq : ICharSequence
{
    public IEnumerator<TCharSeq?>? ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsStringBuilder 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderEnumeratorWhenNonNullAddAllStringBearer :
    FormattedEnumeratorFieldMoldScaffold<StringBuilder?, IEnumerator<StringBuilder?>>
  
{
    public IEnumerator<StringBuilder?>? ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchEnumeratorWhenNonNullAddAllStringBearer<TAny> : FormattedEnumeratorFieldMoldScaffold<TAny, IEnumerator<TAny>>
  
{
    public IEnumerator<TAny>? ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldObjectEnumeratorWhenNonNullAddAllStringBearer : FormattedEnumeratorFieldMoldScaffold<object?, IEnumerator<object?>>
  
{
    public IEnumerator<object?>? ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}
