// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    OrderedCollectionFields;

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolEnumerableAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool, IEnumerable<bool>>
{
    public IEnumerable<bool>? ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolEnumerableAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool?, IEnumerable<bool?>>

{
    public IEnumerable<bool?>? ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsSpanFormattableExceptNullableStruct
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

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsOnlyNullableStructSpanFormattable
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

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyExceptNullableStruct |
                  SupportsValueRevealer)]
public class FieldCloakedBearerEnumerableAlwaysAddAllStringBearer<TCloaked, TRevealBase> :
    RevealerCollectionFieldMoldScaffold<TCloaked, TRevealBase, IEnumerable<TCloaked>>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public IEnumerable<TCloaked>? ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerable, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyNullableStruct |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerEnumerableAlwaysAddAllStringBearer<TCloakedStruct> :
    RevealerCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct, IEnumerable<TCloakedStruct?>>
    where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerable, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsTypeAllButNullableStruct | AcceptsStringBearer)]
public class FieldStringBearerEnumerableAlwaysAddAllStringBearer<TBearer> : CollectionFieldMoldScaffold<TBearer, IEnumerable<TBearer>>
    where TBearer : IStringBearer
{
    public IEnumerable<TBearer>? ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerEnumerableAlwaysAddAllStringBearer<TBearerStruct> : CollectionFieldMoldScaffold<TBearerStruct?,
    IEnumerable<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsString
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringEnumerableAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string?, IEnumerable<string?>>

{
    public IEnumerable<string?>? ComplexTypeCollectionFieldAlwaysAddAllStringEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllStringEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsCharSequence
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

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsStringBuilder | AcceptsEnumerable | AlwaysWrites
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderEnumerableAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<StringBuilder?, IEnumerable<StringBuilder?>>

{
    public IEnumerable<StringBuilder?>? ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchEnumerableAlwaysAddAllStringBearer<TAny> : FormattedCollectionFieldMoldScaffold<TAny, IEnumerable<TAny>>

{
    public IEnumerable<TAny>? ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatchEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldObjectEnumerableAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object?, IEnumerable<object?>>

{
    public IEnumerable<object?>? ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerable);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObjectEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerable)
              , ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerable, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolEnumeratorAlwaysAddAllStringBearer : FormattedEnumeratorFieldMoldScaffold<bool, IEnumerator<bool>>
{
    public IEnumerator<bool>? ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolEnumeratorAlwaysAddAllStringBearer : FormattedEnumeratorFieldMoldScaffold<bool?, IEnumerator<bool?>>

{
    public IEnumerator<bool?>? ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsSpanFormattableExceptNullableStruct
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

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsOnlyNullableStructSpanFormattable
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

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyExceptNullableStruct |
                  SupportsValueRevealer)]
public class FieldCloakedBearerEnumeratorAlwaysAddAllStringBearer<TCloaked, TRevealBase> :
    RevealerEnumeratorFieldMoldScaffold<TCloaked, TRevealBase, IEnumerator<TCloaked>>
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public IEnumerator<TCloaked>? ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerator, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyNullableStruct |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerEnumeratorAlwaysAddAllStringBearer<TCloakedStruct> :
    RevealerEnumeratorFieldMoldScaffold<TCloakedStruct?, TCloakedStruct, IEnumerator<TCloakedStruct?>>
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerator, ValueRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsTypeAllButNullableStruct | AcceptsStringBearer)]
public class FieldStringBearerEnumeratorAlwaysAddAllStringBearer<TBearer> : EnumeratorFieldMoldScaffold<TBearer, IEnumerator<TBearer>>
    where TBearer : IStringBearer
{
    public IEnumerator<TBearer>? ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerEnumeratorAlwaysAddAllStringBearer<TBearerStruct> : EnumeratorFieldMoldScaffold<TBearerStruct?,
    IEnumerator<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsString
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringEnumeratorAlwaysAddAllStringBearer : FormattedEnumeratorFieldMoldScaffold<string?, IEnumerator<string?>>

{
    public IEnumerator<string?>? ComplexTypeCollectionFieldAlwaysAddAllStringEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllStringEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsCharSequence
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

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsStringBuilder | AcceptsEnumerator | AlwaysWrites
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderEnumeratorAlwaysAddAllStringBearer : FormattedEnumeratorFieldMoldScaffold<StringBuilder?, IEnumerator<StringBuilder?>>

{
    public IEnumerator<StringBuilder?>? ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchEnumeratorAlwaysAddAllStringBearer<TAny> : FormattedEnumeratorFieldMoldScaffold<TAny, IEnumerator<TAny>>

{
    public IEnumerator<TAny>? ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatchEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerator, ValueFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | AlwaysWrites
                | AcceptsNullableObject | SupportsValueFormatString)]
public class FieldObjectEnumeratorAlwaysAddAllStringBearer : FormattedEnumeratorFieldMoldScaffold<object?, IEnumerator<object?>>

{
    public IEnumerator<object?>? ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerator);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObjectEnumerate
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerator)
              , ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerator, ValueFormatString)
           .Complete();
}
