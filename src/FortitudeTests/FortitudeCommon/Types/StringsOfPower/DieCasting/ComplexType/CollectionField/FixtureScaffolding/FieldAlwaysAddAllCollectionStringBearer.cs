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

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolSpanAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool, bool[]>
{
    public bool[] ComplexTypeCollectionFieldAlwaysAddAllBoolSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllBoolSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolSpanAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool?, bool?[]>
{
    public bool?[] ComplexTypeCollectionFieldAlwaysAddAllNullableBoolSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableBoolSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class FieldSpanFormattableSpanAlwaysAddAllStringBearer<TFmt> : FormattedCollectionFieldMoldScaffold<TFmt, TFmt[]>
    where TFmt : ISpanFormattable?
{
    public TFmt[] ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableStructSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableStructSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableStructSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableStructSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsOnlyNullableClassSpanFormattable
                | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassSpanAlwaysAddAllStringBearer<TFmt> : FormattedCollectionFieldMoldScaffold<TFmt?, TFmt?[]>
    where TFmt : class, ISpanFormattable
{
    public TFmt?[] ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableNullableClassSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableNullableClassSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableNullableClassSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableNullableClassSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class FieldNullableSpanFormattableSpanAlwaysAddAllStringBearer<TFmtStruct> : FormattedCollectionFieldMoldScaffold<TFmtStruct?, TFmtStruct?[]>
   where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsAnyExceptNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerSpanAlwaysAddAllStringBearer<TCloaked, TRevealBase> : 
    RevealerCollectionFieldMoldScaffold<TCloaked, TRevealBase, TCloaked[]> 
    where TCloaked : TRevealBase?
    where TRevealBase : notnull
{
    public TCloaked[] ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerSpan.AsSpan(), ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsNullableClass 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerNullableClassSpanAlwaysAddAllStringBearer<TCloaked, TRevealBase> : 
    RevealerCollectionFieldMoldScaffold<TCloaked, TRevealBase, TCloaked[]> 
    where TCloaked : class?, TRevealBase?
    where TRevealBase : notnull
{
    public TCloaked[] ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerNullableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerNullableSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerNullableSpan.AsSpan(), ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsAnyNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerSpanAlwaysAddAllStringBearer<TCloakedStruct> : 
    RevealerCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]?>
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerSpan.AsSpan(), ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AlwaysWrites | AcceptsArray | CallsAsSpan | AcceptsTypeAllButNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerSpanAlwaysAddAllStringBearer<TBearer> : FormattedCollectionFieldMoldScaffold<TBearer, TBearer[]>
    where TBearer : IStringBearer?
{
    public TBearer[]? ComplexTypeCollectionFieldAlwaysAddAllStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBearerSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsNullableClass 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerNullableClassSpanAlwaysAddAllStringBearer<TBearer> : FormattedCollectionFieldMoldScaffold<TBearer?, TBearer?[]>
    where TBearer : class?, IStringBearer?
{
    public TBearer?[]? ComplexTypeCollectionFieldAlwaysAddAllStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBearerSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerSpanAlwaysAddAllStringBearer<TBearerStruct> : FormattedCollectionFieldMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsString 
                | AcceptsClass | SupportsValueFormatString)]
public class FieldStringSpanAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string, string[]>
{
    public string[]? ComplexTypeCollectionFieldAlwaysAddAllStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllStringSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsString 
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringNullableSpanAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string?, string?[]>
{
    public string?[]? ComplexTypeCollectionFieldAlwaysAddAllStringSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllStringSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceSpanAlwaysAddAllStringBearer<TCharSeq> : FormattedCollectionFieldMoldScaffold<TCharSeq, TCharSeq[]>
    where TCharSeq : ICharSequence?
{
    public TCharSeq[]? ComplexTypeCollectionFieldAlwaysAddAllCharSequenceSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllCharSequenceSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsStringBuilder
               | AcceptsClass | SupportsValueFormatString)]
public class FieldStringBuilderSpanAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<StringBuilder, StringBuilder[]>
  
{
    public StringBuilder[] ComplexTypeCollectionFieldAlwaysAddAllStringBuilderSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBuilderSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsStringBuilder 
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderNullableSpanAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<StringBuilder?, StringBuilder?[]>
  
{
    public StringBuilder?[] ComplexTypeCollectionFieldAlwaysAddAllStringBuilderSpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBuilderSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsAnyGeneric
                | SupportsValueFormatString)]
public class FieldMatchSpanAlwaysAddAllStringBearer<TAny> : FormattedCollectionFieldMoldScaffold<TAny, TAny[]>
{
    public TAny[]? ComplexTypeCollectionFieldAlwaysAddAllMatchSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatch
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllMatchSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsNullableObject
                | SupportsValueFormatString)]
public class FieldObjectSpanAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object, object[]>
{
    public object[]? ComplexTypeCollectionFieldAlwaysAddAllObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObject
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllObjectSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsSpan | AlwaysWrites | AcceptsNonNullableObject |
                  SupportsValueFormatString)]
public class FieldNullableObjectSpanAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object?, object?[]>
{
    public object?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableObjectSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObjectNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableObjectSpan)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableObjectSpan.AsSpan()
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsStruct
                | SupportsValueFormatString)]
public class FieldBoolReadOnlySpanAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool, bool[]>
{
    public bool[] ComplexTypeCollectionFieldAlwaysAddAllBoolReadOnlySpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolReadOnlySpan)
              , (ReadOnlySpan<bool>)ComplexTypeCollectionFieldAlwaysAddAllBoolReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueFormatString)]
public class FieldNullableBoolReadOnlySpanAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool?, bool?[]>
{
    public bool?[] ComplexTypeCollectionFieldAlwaysAddAllNullableBoolReadOnlySpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolReadOnlySpan)
              , (ReadOnlySpan<bool?>)ComplexTypeCollectionFieldAlwaysAddAllNullableBoolReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites
                | AcceptsSpanFormattableExceptNullableStruct | SupportsValueFormatString)]
public class FieldSpanFormattableReadOnlySpanAlwaysAddAllStringBearer<TFmt> : FormattedCollectionFieldMoldScaffold<TFmt, TFmt[]>
    where TFmt : ISpanFormattable?
{
    public TFmt[] ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableReadOnlySpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmt>)ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites
                | AcceptsOnlyNullableClassSpanFormattable | SupportsValueFormatString)]
public class FieldSpanFormattableNullableClassReadOnlySpanAlwaysAddAllStringBearer<TFmt> : FormattedCollectionFieldMoldScaffold<TFmt?, TFmt?[]>
   where TFmt : class, ISpanFormattable
{
    public TFmt?[] ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableNullableRefReadOnlySpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableNullableRefReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableNullableRefReadOnlySpan)
              , (ReadOnlySpan<TFmt?>)ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableNullableRefReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites
                | AcceptsOnlyNullableStructSpanFormattable | SupportsValueFormatString)]
public class FieldNullableSpanFormattableReadOnlySpanAlwaysAddAllStringBearer<TFmtStruct> :
    FormattedCollectionFieldMoldScaffold<TFmtStruct?, TFmtStruct?[]>
   where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableReadOnlySpan)
              , (ReadOnlySpan<TFmtStruct?>)ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsAnyExceptNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerReadOnlySpanAlwaysAddAllStringBearer<TCloaked, TRevealBase> : 
    RevealerCollectionFieldMoldScaffold<TCloaked, TRevealBase, TCloaked[]> 
    where TCloaked : TRevealBase?
    where TRevealBase : notnull
{
    public TCloaked[] ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloaked>)ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerReadOnlySpan
              , ValueRevealer, ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsAnyNullableClass 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerNullableClassReadOnlySpanAlwaysAddAllStringBearer<TCloaked, TRevealBase> :
    RevealerCollectionFieldMoldScaffold<TCloaked?, TRevealBase, TCloaked?[]> 
    where TCloaked : class?, TRevealBase?
    where TRevealBase : notnull
{
    public TCloaked?[] ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerNullableSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerNullableSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerNullableSpan)
              , (ReadOnlySpan<TCloaked?>)ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerNullableSpan.AsSpan()
              , ValueRevealer, ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsAnyNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerReadOnlySpanAlwaysAddAllStringBearer<TCloakedStruct> :
    RevealerCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerReadOnlySpan)
              , (ReadOnlySpan<TCloakedStruct?>)ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerReadOnlySpan
              , ValueRevealer, ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsTypeAllButNullableStruct
                     | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerReadOnlySpanAlwaysAddAllStringBearer<TBearer> : FormattedCollectionFieldMoldScaffold<TBearer, TBearer[]>
    where TBearer : IStringBearer?
{
    public TBearer[]? ComplexTypeCollectionFieldAlwaysAddAllStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearer>)ComplexTypeCollectionFieldAlwaysAddAllStringBearerReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsNullableClass
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerNullableClassReadOnlySpanAlwaysAddAllStringBearer<TBearer> : FormattedCollectionFieldMoldScaffold<TBearer?, TBearer?[]>
    where TBearer : class, IStringBearer
{
    public TBearer?[]? ComplexTypeCollectionFieldAlwaysAddAllStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearer?>)ComplexTypeCollectionFieldAlwaysAddAllStringBearerReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites 
                | AcceptsNullableStructStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerReadOnlySpanAlwaysAddAllStringBearer<TBearerStruct> : FormattedCollectionFieldMoldScaffold<TBearerStruct?,
    TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerReadOnlySpan)
              , (ReadOnlySpan<TBearerStruct?>)ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsString
                | AcceptsClass | SupportsValueFormatString)]
public class FieldStringReadOnlySpanAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string, string[]>
{
    public string[]? ComplexTypeCollectionFieldAlwaysAddAllStringReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringReadOnlySpan)
              , (ReadOnlySpan<string>)ComplexTypeCollectionFieldAlwaysAddAllStringReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsString
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringNullableReadOnlySpanAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string?, string?[]>
{
    public string?[]? ComplexTypeCollectionFieldAlwaysAddAllStringNullableReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringNullableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringNullableReadOnlySpan)
              , (ReadOnlySpan<string?>)ComplexTypeCollectionFieldAlwaysAddAllStringNullableReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsCharSequence
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceReadOnlySpanAlwaysAddAllStringBearer<TCharSeq> : FormattedCollectionFieldMoldScaffold<TCharSeq, TCharSeq[]>
    where TCharSeq : ICharSequence?
{
    public TCharSeq[]? ComplexTypeCollectionFieldAlwaysAddAllCharSequenceReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceReadOnlySpan)
              , (ReadOnlySpan<TCharSeq>)ComplexTypeCollectionFieldAlwaysAddAllCharSequenceReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}


[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsStringBuilder
                | AcceptsClass | SupportsValueFormatString)]
public class FieldStringBuilderReadOnlySpanAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<StringBuilder?, StringBuilder[]>
  
{
    public StringBuilder[] ComplexTypeCollectionFieldAlwaysAddAllStringBuilderReadOnlySpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderReadOnlySpan)
              , (ReadOnlySpan<StringBuilder>)ComplexTypeCollectionFieldAlwaysAddAllStringBuilderReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsStringBuilder
                | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderNullableReadOnlySpanAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<StringBuilder?, StringBuilder?[]>
  
{
    public StringBuilder?[] ComplexTypeCollectionFieldAlwaysAddAllStringBuilderNullableReadOnlySpan
    {
        get => Value!;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderNullableReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderNullableReadOnlySpan)
              , (ReadOnlySpan<StringBuilder?>)ComplexTypeCollectionFieldAlwaysAddAllStringBuilderNullableReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsAnyGeneric
                | CallsViaMatch | SupportsValueFormatString)]
public class FieldMatchReadOnlySpanAlwaysAddAllStringBearer<TAny> : FormattedCollectionFieldMoldScaffold<TAny, TAny[]>
{
    public TAny[]? ComplexTypeCollectionFieldAlwaysAddAllMatchReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatch
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchReadOnlySpan)
              , (ReadOnlySpan<TAny>)ComplexTypeCollectionFieldAlwaysAddAllMatchReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsNonNullableObject
                | SupportsValueFormatString)]
public class FieldObjectReadOnlySpanAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object, object[]>
{
    public object[]? ComplexTypeCollectionFieldAlwaysAddAllObjectSpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObject
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectSpan)
              , (ReadOnlySpan<object>)ComplexTypeCollectionFieldAlwaysAddAllObjectSpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | CallsAsReadOnlySpan | AlwaysWrites | AcceptsNullableObject
                | SupportsValueFormatString)]
public class FieldNullableObjectReadOnlySpanAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object?, object?[]>
{
    public object?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableObjectReadOnlySpan
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableObjectReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObjectNullable
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableObjectReadOnlySpan)
              , (ReadOnlySpan<object?>)ComplexTypeCollectionFieldAlwaysAddAllNullableObjectReadOnlySpan
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolArrayAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool, bool[]>
{
    public bool[]? ComplexTypeCollectionFieldAlwaysAddAllBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolArray)
              , ComplexTypeCollectionFieldAlwaysAddAllBoolArray
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolArrayAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool?, bool?[]>
{
    public bool?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableBoolArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolArray)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableBoolArray
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class FieldSpanFormattableArrayAlwaysAddAllStringBearer<TFmt> : FormattedCollectionFieldMoldScaffold<TFmt?, TFmt?[]>
    where TFmt : ISpanFormattable
{
    public TFmt?[]? ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableArray)
              , ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableArray
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class FieldNullableSpanFormattableArrayAlwaysAddAllStringBearer<TFmtStruct> : FormattedCollectionFieldMoldScaffold<TFmtStruct?, TFmtStruct?[]>
   where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableArray)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableArray
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | AcceptsAnyExceptNullableStruct 
                | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerArrayAlwaysAddAllStringBearer<TCloaked, TRevealBase> : 
    RevealerCollectionFieldMoldScaffold<TCloaked, TRevealBase, TCloaked[]?> 
    where TCloaked : TRevealBase
    where TRevealBase : notnull
{
    public TCloaked[]? ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerArray)
              , ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerArray
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | AcceptsAnyNullableStruct
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldNullableCloakedBearerArrayAlwaysAddAllStringBearer<TCloakedStruct> :
    RevealerCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct, TCloakedStruct?[]?> where TCloakedStruct : struct
{
    public TCloakedStruct?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerArray)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerArray
              , ValueRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | AcceptsTypeAllButNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldStringBearerArrayAlwaysAddAllStringBearer<TBearer> : FormattedCollectionFieldMoldScaffold<TBearer, TBearer[]>
    where TBearer : IStringBearer
{
    public TBearer[]? ComplexTypeCollectionFieldAlwaysAddAllStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerArray)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBearerArray
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | AcceptsNullableStruct 
                | AcceptsStringBearer | SupportsValueFormatString)]
public class FieldNullableStringBearerArrayAlwaysAddAllStringBearer<TBearerStruct> : FormattedCollectionFieldMoldScaffold<TBearerStruct?, TBearerStruct?[]>
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerArray)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerArray)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | AcceptsString 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringArrayAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string?, string?[]>
{
    public string?[]? ComplexTypeCollectionFieldAlwaysAddAllStringArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringArray)
              , ComplexTypeCollectionFieldAlwaysAddAllStringArray
              , ValueFormatString)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | AcceptsCharSequence 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceArrayAlwaysAddAllStringBearer<TCharSeq> : FormattedCollectionFieldMoldScaffold<TCharSeq?, TCharSeq?[]>
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? ComplexTypeCollectionFieldAlwaysAddAllCharSequenceArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceArray)
              , ComplexTypeCollectionFieldAlwaysAddAllCharSequenceArray, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | AcceptsStringBuilder 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderArrayAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<StringBuilder?, StringBuilder?[]>
  
{
    public StringBuilder?[]? ComplexTypeCollectionFieldAlwaysAddAllStringBuilderArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderArray)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBuilderArray
              , ValueFormatString)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchArrayAlwaysAddAllStringBearer<TAny> : FormattedCollectionFieldMoldScaffold<TAny?, TAny?[]>
{
    public TAny?[]? ComplexTypeCollectionFieldAlwaysAddAllMatchArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatch
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchArray)
              , ComplexTypeCollectionFieldAlwaysAddAllMatchArray
              , ValueFormatString)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsArray | AlwaysWrites | AcceptsNullableObject | SupportsValueFormatString)]
public class FieldObjectArrayAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object?, object?[]>
{
    public object?[]? ComplexTypeCollectionFieldAlwaysAddAllNullableObjectArray
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableObjectArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObject
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableObjectArray)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableObjectArray
              , ValueFormatString)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolListAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool, IReadOnlyList<bool>>
{
    public IReadOnlyList<bool>? ComplexTypeCollectionFieldAlwaysAddAllBoolList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolList)
              , ComplexTypeCollectionFieldAlwaysAddAllBoolList, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolListAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<bool?, IReadOnlyList<bool?>>
{
    public IReadOnlyList<bool?>? ComplexTypeCollectionFieldAlwaysAddAllNullableBoolList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolList)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableBoolList, ValueFormatString)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | AcceptsSpanFormattableExceptNullableStruct
                | SupportsValueFormatString)]
public class FieldSpanFormattableListAlwaysAddAllStringBearer<TFmt> : FormattedCollectionFieldMoldScaffold<TFmt, IReadOnlyList<TFmt>>
    where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt>? ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableList)
              , ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableList
              , ValueFormatString)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | AcceptsOnlyNullableStructSpanFormattable
                | SupportsValueFormatString)]
public class FieldNullableSpanFormattableListAlwaysAddAllStringBearer<TFmtStruct> :
    FormattedCollectionFieldMoldScaffold<TFmtStruct?, IReadOnlyList<TFmtStruct?>>
    where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableList)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableList
              , ValueFormatString)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | AcceptsAnyExceptNullableStruct 
                 | SupportsValueRevealer | SupportsValueFormatString)]
public class FieldCloakedBearerListAlwaysAddAllStringBearer<TCloaked, TRevealBase> :
    RevealerCollectionFieldMoldScaffold<TCloaked, TRevealBase, IReadOnlyList<TCloaked>?>
   where TCloaked : TRevealBase
   where TRevealBase : notnull
{
    public IReadOnlyList<TCloaked>? ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerList)
              , ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerList
              , ValueRevealer)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | AcceptsAnyNullableStruct | SupportsValueRevealer
                | SupportsValueFormatString)]
public class FieldNullableCloakedBearerListAlwaysAddAllStringBearer<TCloakedStruct> :
    RevealerCollectionFieldMoldScaffold<TCloakedStruct?, TCloakedStruct, IReadOnlyList<TCloakedStruct?>?> where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerList)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerList
              , ValueRevealer)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | AcceptsTypeAllButNullableStruct | AcceptsStringBearer
                | SupportsValueFormatString)]
public class FieldStringBearerListAlwaysAddAllStringBearer<TBearer> : FormattedCollectionFieldMoldScaffold<TBearer, IReadOnlyList<TBearer>>
    where TBearer : IStringBearer
{
    public IReadOnlyList<TBearer>? ComplexTypeCollectionFieldAlwaysAddAllStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerList)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBearerList
                , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer
                | SupportsValueFormatString)]
public class FieldNullableStringBearerListAlwaysAddAllStringBearer<TBearerStruct> : FormattedCollectionFieldMoldScaffold<TBearerStruct?,
    IReadOnlyList<TBearerStruct?>>
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerList)
              , ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerList
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | AcceptsString 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringListAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<string?, IReadOnlyList<string?>>
{
    public IReadOnlyList<string?>? ComplexTypeCollectionFieldAlwaysAddAllStringList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringList)
              , ComplexTypeCollectionFieldAlwaysAddAllStringList
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | AcceptsCharSequence 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldCharSequenceListAlwaysAddAllStringBearer<TCharSeq> : FormattedCollectionFieldMoldScaffold<TCharSeq?, IReadOnlyList<TCharSeq?>>
    where TCharSeq : ICharSequence
{
    public IReadOnlyList<TCharSeq?>? ComplexTypeCollectionFieldAlwaysAddAllCharSequenceList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeq
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceList)
              , ComplexTypeCollectionFieldAlwaysAddAllCharSequenceList
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | AcceptsStringBuilder 
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString)]
public class FieldStringBuilderListAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<StringBuilder?, IReadOnlyList<StringBuilder?>>
  
{
    public IReadOnlyList<StringBuilder?>? ComplexTypeCollectionFieldAlwaysAddAllStringBuilderList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderList)
              , ComplexTypeCollectionFieldAlwaysAddAllStringBuilderList
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchListAlwaysAddAllStringBearer<TAny> : FormattedCollectionFieldMoldScaffold<TAny, IReadOnlyList<TAny>>
{
    public IReadOnlyList<TAny>? ComplexTypeCollectionFieldAlwaysAddAllMatchList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatch
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchList)
              , ComplexTypeCollectionFieldAlwaysAddAllMatchList
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsComplexType | CollectionCardinality | AcceptsList | AlwaysWrites
                | AcceptsNullableObject | SupportsValueFormatString)]
public class FieldObjectListAlwaysAddAllStringBearer : FormattedCollectionFieldMoldScaffold<object?, IReadOnlyList<object?>>
{
    public IReadOnlyList<object?>? ComplexTypeCollectionFieldAlwaysAddAllObjectList
    {
        get => Value;
        set => Value = value;
    }

    public override string PropertyName => nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObject
               (nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectList)
              , ComplexTypeCollectionFieldAlwaysAddAllObjectList
              , ValueFormatString, FormattingFlags)
           .Complete();

}
