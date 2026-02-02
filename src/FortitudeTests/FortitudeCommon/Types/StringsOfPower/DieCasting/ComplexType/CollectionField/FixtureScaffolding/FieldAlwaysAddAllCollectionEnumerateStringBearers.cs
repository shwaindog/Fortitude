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

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolEnumerableAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool, IEnumerable<bool>>
{
    public IEnumerable<bool>? ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolEnumerableAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool?, IEnumerable<bool?>>

{
    public IEnumerable<bool?>? ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class FieldSpanFormattableEnumerableAlwaysAddAllStringBearer<TFmt> : FormattedCollectionFieldMoldScaffold<TFmt?, IEnumerable<TFmt?>>
    where TFmt : ISpanFormattable
{
    public IEnumerable<TFmt?>? ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumerableAlwaysAddAllStringBearer<TFmtStruct> :
    FormattedCollectionFieldMoldScaffold<TFmtStruct?, IEnumerable<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyExceptNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerEnumerableAlwaysAddAllStringBearer<TCloaked, TRevealBase> :
    RevealerCollectionFieldMoldScaffold<TCloaked, TRevealBase, IEnumerable<TCloaked>?>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public IEnumerable<TCloaked>? ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerable
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerEnumerableAlwaysAddAllStringBearer<TCloakedStruct> :
    RevealerCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct, IEnumerable<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerable
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsTypeAllButNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerEnumerableAlwaysAddAllStringBearer<TBearer> : FormattedCollectionFieldMoldScaffold<TBearer, IEnumerable<TBearer>>
    where TBearer : IStringBearer
{
    public IEnumerable<TBearer>? ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerEnumerableAlwaysAddAllStringBearer<TBearerStruct> : FormattedCollectionFieldMoldScaffold<TBearerStruct?,
    IEnumerable<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsString
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringEnumerableAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string?, IEnumerable<string?>>

{
    public IEnumerable<string?>? ComplexTypeCollectionFieldAlwaysAddAllStringEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllStringEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceEnumerableAlwaysAddAllStringBearer<TCharSeq> : FormattedCollectionFieldMoldScaffold<TCharSeq?, IEnumerable<TCharSeq?>>
    where TCharSeq : ICharSequence
{
    public IEnumerable<TCharSeq?>? ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsStringBuilder | AcceptsEnumerable | AlwaysWrites
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderEnumerableAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<StringBuilder?, IEnumerable<StringBuilder?>>

{
    public IEnumerable<StringBuilder?>? ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchEnumerableAlwaysAddAllStringBearer<TAny> : FormattedCollectionFieldMoldScaffold<TAny, IEnumerable<TAny>>

{
    public IEnumerable<TAny>? ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatchEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldObjectEnumerableAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object?, IEnumerable<object?>>

{
    public IEnumerable<object?>? ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObjectEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerable
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolEnumeratorAlwaysAddAllStringBearer : FormattedEnumeratorFieldMoldScaffold<bool, IEnumerator<bool>>
{
    public IEnumerator<bool>? ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolEnumeratorAlwaysAddAllStringBearer : FormattedEnumeratorFieldMoldScaffold<bool?, IEnumerator<bool?>>

{
    public IEnumerator<bool?>? ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class FieldSpanFormattableEnumeratorAlwaysAddAllStringBearer<TFmt> : FormattedEnumeratorFieldMoldScaffold<TFmt, IEnumerator<TFmt>>
    where TFmt : ISpanFormattable
{
    public IEnumerator<TFmt>? ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumeratorAlwaysAddAllStringBearer<TFmtStruct> :
    FormattedEnumeratorFieldMoldScaffold<TFmtStruct?, IEnumerator<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyExceptNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerEnumeratorAlwaysAddAllStringBearer<TCloaked, TRevealBase> :
    RevealerEnumeratorFieldMoldScaffold<TCloaked, TRevealBase, IEnumerator<TCloaked>?>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public IEnumerator<TCloaked>? ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerator
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString )]
public class FieldNullableCloakedBearerEnumeratorAlwaysAddAllStringBearer<TCloakedStruct> :
    RevealerEnumeratorFieldMoldScaffold<TCloakedStruct?, TCloakedStruct, IEnumerator<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerator
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsTypeAllButNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerEnumeratorAlwaysAddAllStringBearer<TBearer> : FormattedEnumeratorFieldMoldScaffold<TBearer, IEnumerator<TBearer>>
    where TBearer : IStringBearer
{
    public IEnumerator<TBearer>? ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer
                     | SupportsValueFormatString)]
public class FieldNullableStringBearerEnumeratorAlwaysAddAllStringBearer<TBearerStruct> : FormattedEnumeratorFieldMoldScaffold<TBearerStruct?,
    IEnumerator<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsString
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringEnumeratorAlwaysAddAllStringBearer : FormattedEnumeratorFieldMoldScaffold<string?, IEnumerator<string?>>

{
    public IEnumerator<string?>? ComplexTypeCollectionFieldAlwaysAddAllStringEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllStringEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceEnumeratorAlwaysAddAllStringBearer<TCharSeq> : FormattedEnumeratorFieldMoldScaffold<TCharSeq?, IEnumerator<TCharSeq?>>
    where TCharSeq : ICharSequence
{
    public IEnumerator<TCharSeq?>? ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsStringBuilder | AcceptsEnumerator | AlwaysWrites
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderEnumeratorAlwaysAddAllStringBearer : FormattedEnumeratorFieldMoldScaffold<StringBuilder?, IEnumerator<StringBuilder?>>

{
    public IEnumerator<StringBuilder?>? ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchEnumeratorAlwaysAddAllStringBearer<TAny> : FormattedEnumeratorFieldMoldScaffold<TAny, IEnumerator<TAny>>

{
    public IEnumerator<TAny>? ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatchEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites
                | AcceptsNullableObject | SupportsValueFormatString)]
public class FieldObjectEnumeratorAlwaysAddAllStringBearer : FormattedEnumeratorFieldMoldScaffold<object?, IEnumerator<object?>>

{
    public IEnumerator<object?>? ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObjectEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerator
              , ValueFormatString, FormattingFlags)
           .Complete();
}
