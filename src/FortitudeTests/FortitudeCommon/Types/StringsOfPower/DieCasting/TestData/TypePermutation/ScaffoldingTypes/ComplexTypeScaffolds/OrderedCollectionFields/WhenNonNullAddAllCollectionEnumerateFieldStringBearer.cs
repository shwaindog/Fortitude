// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.TypeGeneratePartFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    OrderedCollectionFields;

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsStruct)]
public class BoolEnumerableWhenNonNullAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerable<bool>?>
{
    public IEnumerable<bool>? ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsNullableStruct)]
public class NullableBoolEnumerableWhenNonNullAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerable<bool?>?>
{
    public IEnumerable<bool?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableEnumerableWhenNonNullAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<IEnumerable<TFmt>?>
  , ISupportsSingleFormatString where TFmt : ISpanFormattable
{
    public IEnumerable<TFmt>? ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TFmt>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerable
              , FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class NullableSpanFormattableEnumerableWhenNonNullAddAllStringBearer<TStructFmt> : IStringBearer
  , IMoldSupportedValue<IEnumerable<TStructFmt?>?>, ISupportsSingleFormatString where TStructFmt : struct, ISpanFormattable
{
    public IEnumerable<TStructFmt?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TStructFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class CloakedBearerEnumerableWhenNonNullAddAllStringBearer<TCloaked, TCloakedBase>
    : IStringBearer, IMoldSupportedValue<IEnumerable<TCloaked>?>, ISupportsSingleRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public IEnumerable<TCloaked>? ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedBase> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerable, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsNullableStruct | AcceptsSpanFormattable | AcceptsIntegerNumber
                | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer | OnePalantirRevealer)]
public class NullableCloakedBearerEnumerableWhenNonNullAddAllStringBearer<TCloakedStruct>
    : IStringBearer, IMoldSupportedValue<IEnumerable<TCloakedStruct?>?>, ISupportsSingleRevealer<TCloakedStruct> where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TCloakedStruct?>? Value { get; set; }

    public PalantírReveal<TCloakedStruct> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerable, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerEnumerableWhenNonNullAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<IEnumerable<TBearer>?>
    where TBearer : IStringBearer
{
    public IEnumerable<TBearer>? ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TBearer>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerEnumerableWhenNonNullAddAllStringBearer<TBearerStruct> : IStringBearer
  , IMoldSupportedValue<IEnumerable<TBearerStruct?>?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsChars | OneFormatString)]
public class StringEnumerableWhenNonNullAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerable<string?>?>, ISupportsSingleFormatString
{
    public IEnumerable<string?>? ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsChars | OneFormatString)]
public class CharSequenceEnumerableWhenNonNullAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<IEnumerable<TCharSeq?>?>
  , ISupportsSingleFormatString where TCharSeq : ICharSequence
{
    public IEnumerable<TCharSeq?>? ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsChars | OneFormatString)]
public class StringBuilderEnumerableWhenNonNullAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerable<StringBuilder?>?>
  , ISupportsSingleFormatString
{
    public IEnumerable<StringBuilder?>? ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsAny | OneFormatString)]
public class MatchEnumerableWhenNonNullAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<IEnumerable<T>?>
  , ISupportsSingleFormatString
{
    public IEnumerable<T>? ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<T>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsAny | OneFormatString)]
public class ObjectEnumerableWhenNonNullAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerable<object?>?>
  , ISupportsSingleFormatString
{
    public IEnumerable<object?>? ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerable)
              , ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsStruct)]
public class BoolEnumeratorWhenNonNullAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerator<bool>?>
{
    public IEnumerator<bool>? ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllBoolEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsNullableStruct)]
public class NullableBoolEnumeratorWhenNonNullAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerator<bool?>?>
{
    public IEnumerator<bool?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableBoolEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableEnumeratorWhenNonNullAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<IEnumerator<TFmt>?>
  , ISupportsSingleFormatString where TFmt : ISpanFormattable
{
    public IEnumerator<TFmt>? ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TFmt>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllSpanFormattableEnumerator, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class NullableSpanFormattableEnumeratorWhenNonNullAddAllStringBearer<TFmtStruct> : IStringBearer
  , IMoldSupportedValue<IEnumerator<TFmtStruct?>?>, ISupportsSingleFormatString where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableSpanFormattableEnumerator, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class CloakedBearerEnumeratorWhenNonNullAddAllStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<IEnumerator<TCloaked>?>
  , ISupportsSingleRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public IEnumerator<TCloaked>? ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedBase> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCloakedBearerEnumerator, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class NullableCloakedBearerEnumeratorWhenNonNullAddAllStringBearer<TCloakedStruct> : IStringBearer
  , IMoldSupportedValue<IEnumerator<TCloakedStruct?>?>, ISupportsSingleRevealer<TCloakedStruct>
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TCloakedStruct?>? Value { get; set; }

    public PalantírReveal<TCloakedStruct> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllNullableCloakedBearerEnumerator, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerEnumeratorWhenNonNullAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<IEnumerator<TBearer>?>
    where TBearer : IStringBearer
{
    public IEnumerator<TBearer>? ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TBearer>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBearerEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerEnumeratorWhenNonNullAddAllStringBearer<TBearerStruct> : IStringBearer
  , IMoldSupportedValue<IEnumerator<TBearerStruct?>?> where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate(nameof(ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerator)
                                                        , ComplexTypeCollectionFieldWhenNonNullAddAllNullableStringBearerEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsChars | OneFormatString)]
public class StringEnumeratorWhenNonNullAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerator<string?>?>, ISupportsSingleFormatString
{
    public IEnumerator<string?>? ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringEnumerator, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsChars | OneFormatString)]
public class CharSequenceEnumeratorWhenNonNullAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<IEnumerator<TCharSeq?>?>
  , ISupportsSingleFormatString where TCharSeq : ICharSequence
{
    public IEnumerator<TCharSeq?>? ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllCharSequenceEnumerator, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | NonNullWrites | AcceptsChars | OneFormatString)]
public class StringBuilderEnumeratorWhenNonNullAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerator<StringBuilder?>?>
  , ISupportsSingleFormatString
{
    public IEnumerator<StringBuilder?>? ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllStringBuilderEnumerator, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsAny | OneFormatString)]
public class MatchEnumeratorWhenNonNullAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<IEnumerator<T>?>, ISupportsSingleFormatString
{
    public IEnumerator<T>? ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<T>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllMatchEnumerator, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsAny | OneFormatString)]
public class ObjectEnumeratorWhenNonNullAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerator<object?>?>, ISupportsSingleFormatString
{
    public IEnumerator<object?>? ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerator)
              , ComplexTypeCollectionFieldWhenNonNullAddAllObjectEnumerator, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}
