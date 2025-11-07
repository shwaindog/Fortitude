// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    OrderedCollectionFields;

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolEnumerableWhenNonNullAddAllStringBearer : IMoldSupportedValue<IEnumerable<bool>?>, ISupportsValueFormatString
{
    public IEnumerable<bool>? ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerable);
    public IEnumerable<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolEnumerableWhenNonNullAddAllStringBearer : IMoldSupportedValue<IEnumerable<bool?>?>, ISupportsValueFormatString
{
    public IEnumerable<bool?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerable);
    public IEnumerable<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsSpanFormattableExceptNullableStruct 
                | SupportsValueFormatString)]
public class FieldSpanFormattableEnumerableWhenNonNullAddAllStringBearer<TFmt> : IMoldSupportedValue<IEnumerable<TFmt>?>
  , ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public IEnumerable<TFmt>? ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerable);
    public IEnumerable<TFmt>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerable
              , ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsOnlyNullableStructSpanFormattable 
                | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumerableWhenNonNullAddAllStringBearer<TStructFmt> : 
  IMoldSupportedValue<IEnumerable<TStructFmt?>?>, ISupportsValueFormatString where TStructFmt : struct, ISpanFormattable
{
    public IEnumerable<TStructFmt?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerable);
    public IEnumerable<TStructFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyExceptNullableStruct |
                  SupportsValueRevealer)]
public class FieldCloakedBearerEnumerableWhenNonNullAddAllStringBearer<TCloaked, TCloakedBase>
    : IMoldSupportedValue<IEnumerable<TCloaked>?>, ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public IEnumerable<TCloaked>? ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerable);
    public IEnumerable<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerable, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyNullableStruct | SupportsValueRevealer)]
public class FieldNullableCloakedBearerEnumerableWhenNonNullAddAllStringBearer<TCloakedStruct>
    : IMoldSupportedValue<IEnumerable<TCloakedStruct?>?>, ISupportsValueRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerable);
    public IEnumerable<TCloakedStruct?>? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerable, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsTypeAllButNullableStruct 
                | AcceptsStringBearer)]
public class FieldStringBearerEnumerableWhenNonNullAddAllStringBearer<TBearer> : IMoldSupportedValue<IEnumerable<TBearer>?>
    where TBearer : IStringBearer
{
    public IEnumerable<TBearer>? ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerable);
    public IEnumerable<TBearer>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerable)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerEnumerableWhenNonNullAddAllStringBearer<TBearerStruct> : 
  IMoldSupportedValue<IEnumerable<TBearerStruct?>?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerable);
    public IEnumerable<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerable)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsString | SupportsValueFormatString)]
public class FieldStringEnumerableWhenNonNullAddAllStringBearer : IMoldSupportedValue<IEnumerable<string?>?>, ISupportsValueFormatString
{
    public IEnumerable<string?>? ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerable);
    public IEnumerable<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceEnumerableWhenNonNullAddAllStringBearer<TCharSeq> : IMoldSupportedValue<IEnumerable<TCharSeq?>?>
  , ISupportsValueFormatString where TCharSeq : ICharSequence
{
    public IEnumerable<TCharSeq?>? ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerable);
    public IEnumerable<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderEnumerableWhenNonNullAddAllStringBearer : IMoldSupportedValue<IEnumerable<StringBuilder?>?>
  , ISupportsValueFormatString
{
    public IEnumerable<StringBuilder?>? ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerable);
    public IEnumerable<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchEnumerableWhenNonNullAddAllStringBearer<TAny> : IMoldSupportedValue<IEnumerable<TAny>?>
  , ISupportsValueFormatString
{
    public IEnumerable<TAny>? ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerable);
    public IEnumerable<TAny>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldObjectEnumerableWhenNonNullAddAllStringBearer : IMoldSupportedValue<IEnumerable<object?>?>
  , ISupportsValueFormatString
{
    public IEnumerable<object?>? ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerable);
    public IEnumerable<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerable, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsStruct | SupportsValueFormatString)]
public class FieldBoolEnumeratorWhenNonNullAddAllStringBearer : IMoldSupportedValue<IEnumerator<bool>?>, ISupportsValueFormatString
{
    public IEnumerator<bool>? ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerator);
    public IEnumerator<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsNullableStruct | SupportsValueFormatString)]
public class FieldNullableBoolEnumeratorWhenNonNullAddAllStringBearer : IMoldSupportedValue<IEnumerator<bool?>?>, ISupportsValueFormatString
{
    public IEnumerator<bool?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerator);
    public IEnumerator<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsSpanFormattableExceptNullableStruct 
                | SupportsValueFormatString)]
public class FieldSpanFormattableEnumeratorWhenNonNullAddAllStringBearer<TFmt> : IMoldSupportedValue<IEnumerator<TFmt>?>
  , ISupportsValueFormatString where TFmt : ISpanFormattable
{
    public IEnumerator<TFmt>? ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerator);
    public IEnumerator<TFmt>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsOnlyNullableStructSpanFormattable 
                | SupportsValueFormatString)]
public class FieldNullableSpanFormattableEnumeratorWhenNonNullAddAllStringBearer<TFmtStruct> : 
  IMoldSupportedValue<IEnumerator<TFmtStruct?>?>, ISupportsValueFormatString where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerator);
    public IEnumerator<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyExceptNullableStruct |
                  SupportsValueRevealer)]
public class FieldCloakedBearerEnumeratorWhenNonNullAddAllStringBearer<TCloaked, TCloakedBase> : IMoldSupportedValue<IEnumerator<TCloaked>?>
  , ISupportsValueRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public IEnumerator<TCloaked>? ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerator);
    public IEnumerator<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerator, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyNullableStruct |
                  SupportsValueRevealer)]
public class FieldNullableCloakedBearerEnumeratorWhenNonNullAddAllStringBearer<TCloakedStruct> : 
  IMoldSupportedValue<IEnumerator<TCloakedStruct?>?>, ISupportsValueRevealer<TCloakedStruct>
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerator);
    public IEnumerator<TCloakedStruct?>? Value { get; set; }

    public PalantírReveal<TCloakedStruct> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TCloakedStruct>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerator, ValueRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsTypeAllButNullableStruct | AcceptsStringBearer)]
public class FieldStringBearerEnumeratorWhenNonNullAddAllStringBearer<TBearer> : IMoldSupportedValue<IEnumerator<TBearer>?>
    where TBearer : IStringBearer
{
    public IEnumerator<TBearer>? ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerator);
    public IEnumerator<TBearer>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerator)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class FieldNullableStringBearerEnumeratorWhenNonNullAddAllStringBearer<TBearerStruct> : 
  IMoldSupportedValue<IEnumerator<TBearerStruct?>?> where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerator);
    public IEnumerator<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate(nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerator)
                                                        , ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerator)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsString | SupportsValueFormatString)]
public class FieldStringEnumeratorWhenNonNullAddAllStringBearer : IMoldSupportedValue<IEnumerator<string?>?>, ISupportsValueFormatString
{
    public IEnumerator<string?>? ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerator);
    public IEnumerator<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsCharSequence | SupportsValueFormatString)]
public class FieldCharSequenceEnumeratorWhenNonNullAddAllStringBearer<TCharSeq> : IMoldSupportedValue<IEnumerator<TCharSeq?>?>
  , ISupportsValueFormatString where TCharSeq : ICharSequence
{
    public IEnumerator<TCharSeq?>? ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerator);
    public IEnumerator<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsStringBuilder | SupportsValueFormatString)]
public class FieldStringBuilderEnumeratorWhenNonNullAddAllStringBearer : IMoldSupportedValue<IEnumerator<StringBuilder?>?>
  , ISupportsValueFormatString
{
    public IEnumerator<StringBuilder?>? ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerator);
    public IEnumerator<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldMatchEnumeratorWhenNonNullAddAllStringBearer<TAny> : IMoldSupportedValue<IEnumerator<TAny>?>, ISupportsValueFormatString
{
    public IEnumerator<TAny>? ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerator);
    public IEnumerator<TAny>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | CollectionCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyGeneric | SupportsValueFormatString)]
public class FieldObjectEnumeratorWhenNonNullAddAllStringBearer : IMoldSupportedValue<IEnumerator<object?>?>, ISupportsValueFormatString
{
    public IEnumerator<object?>? ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerator);
    public IEnumerator<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerator, ValueFormatString)
           .Complete();

    public string? ValueFormatString { get; set; }

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}
