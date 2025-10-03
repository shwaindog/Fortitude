using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.TypeGeneratePartFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.OrderedCollectionFields;

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct)]
public class BoolEnumerableAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerable<bool>?>
{
    public IEnumerable<bool>? ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerable), ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsNullableStruct)]
public class NullableBoolEnumerableAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerable<bool?>?>
{
    public IEnumerable<bool?>? ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerable), ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableEnumerableAlwaysAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<IEnumerable<TFmt?>?>, 
    ISupportsSingleFormatString where TFmt : ISpanFormattable
{
    public IEnumerable<TFmt?>? ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerable)
                                                , ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class NullableSpanFormattableEnumerableAlwaysAddAllStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<IEnumerable<TFmtStruct?>?>
  , ISupportsSingleFormatString where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerable)
                                                , ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class CloakedBearerEnumerableAlwaysAddAllStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<IEnumerable<TCloaked>?>
    where TCloaked : TCloakedBase
{
    public IEnumerable<TCloaked>? ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedBase> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerable)
                                                   , ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerable, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class NullableCustomBearerEnumerableAlwaysAddAllStringBearer<TCloakedStruct>: IStringBearer, IMoldSupportedValue<IEnumerable<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableCustomBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TCloakedStruct?>? Value { get; set; }

    public PalantírReveal<TCloakedStruct> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCustomBearerEnumerable)
                                                   , ComplexTypeCollectionFieldAlwaysAddAllNullableCustomBearerEnumerable, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerEnumerableAlwaysAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<IEnumerable<TBearer>?>
    where TBearer : IStringBearer
{
    public IEnumerable<TBearer>? ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TBearer>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerable)
                                                   , ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerEnumerableAlwaysAddAllStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<IEnumerable<TBearerStruct?>?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerable)
                                                   , ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | OneFormatString)]
public class StringEnumerableAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerable<string?>?>, ISupportsSingleFormatString
{
    public IEnumerable<string?>? ComplexTypeCollectionFieldAlwaysAddAllStringEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllStringEnumerable)
                                                , ComplexTypeCollectionFieldAlwaysAddAllStringEnumerable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | OneFormatString)]
public class CharSequenceEnumerableAlwaysAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<IEnumerable<TCharSeq?>?>, ISupportsSingleFormatString
    where TCharSeq : ICharSequence
{
    public IEnumerable<TCharSeq?>? ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeqEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerable)
                                                       , ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | OneFormatString)]
public class StringBuilderEnumerableAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerable<StringBuilder?>?>, ISupportsSingleFormatString
{
    public IEnumerable<StringBuilder?>? ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerable)
                                                , ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsAny | OneFormatString)]
public class MatchEnumerableAlwaysAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<IEnumerable<T>?>, ISupportsSingleFormatString
{
    public IEnumerable<T>? ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<T>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatchEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerable)
                                                     , ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsAny | OneFormatString)]
public class ObjectEnumerableAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerable<object?>?>, ISupportsSingleFormatString
{
    public IEnumerable<object?>? ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObjectEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerable)
                                                      , ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct)]
public class BoolEnumeratorAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerator<bool>?>
{
    public IEnumerator<bool>? ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerator)
                                                , ComplexTypeCollectionFieldAlwaysAddAllBoolEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct)]
public class NullableBoolEnumeratorAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerator<bool?>?>
{
    public IEnumerator<bool?>? ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerator)
                                                , ComplexTypeCollectionFieldAlwaysAddAllNullableBoolEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableEnumeratorAlwaysAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<IEnumerator<TFmt>?>
  , ISupportsSingleFormatString where TFmt : ISpanFormattable
{
    public IEnumerator<TFmt>? ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TFmt>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerator)
                                                , ComplexTypeCollectionFieldAlwaysAddAllSpanFormattableEnumerator, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class NullableSpanFormattableEnumeratorAlwaysAddAllStringBearer<TStructFmt> : IStringBearer, IMoldSupportedValue<IEnumerator<TStructFmt?>?>
  , ISupportsSingleFormatString where TStructFmt : struct, ISpanFormattable
{
    public IEnumerator<TStructFmt?>? ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TStructFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerator)
                                                , ComplexTypeCollectionFieldAlwaysAddAllNullableSpanFormattableEnumerator, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class CloakedBearerEnumeratorAlwaysAddAllStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<IEnumerator<TCloaked>?>
  , ISupportsSingleRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public IEnumerator<TCloaked>? ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedBase> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerator)
                                                   , ComplexTypeCollectionFieldAlwaysAddAllCloakedBearerEnumerator, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class NullableCloakedBearerEnumeratorAlwaysAddAllStringBearer<TCloakedStruct> : IStringBearer, IMoldSupportedValue<IEnumerator<TCloakedStruct?>?>
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TCloakedStruct?>? Value { get; set; }

    public PalantírReveal<TCloakedStruct> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerator)
                                                   , ComplexTypeCollectionFieldAlwaysAddAllNullableCloakedBearerEnumerator, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerEnumeratorAlwaysAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<IEnumerator<TBearer>?>
    where TBearer : IStringBearer
{
    public IEnumerator<TBearer>? ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TBearer>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerator)
                                                   , ComplexTypeCollectionFieldAlwaysAddAllStringBearerEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerEnumeratorAlwaysAddAllStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<IEnumerator<TBearerStruct?>?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerator)
                                                   , ComplexTypeCollectionFieldAlwaysAddAllNullableStringBearerEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | OneFormatString)]
public class StringEnumeratorAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerator<string?>?>, ISupportsSingleFormatString
{
    public IEnumerator<string?>? ComplexTypeCollectionFieldAlwaysAddAllStringEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllStringEnumerator)
                                                , ComplexTypeCollectionFieldAlwaysAddAllStringEnumerator, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | OneFormatString)]
public class CharSequenceEnumeratorAlwaysAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<IEnumerator<TCharSeq?>?>
  , ISupportsSingleFormatString where TCharSeq : ICharSequence
{
    public IEnumerator<TCharSeq?>? ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeqEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerator)
                                                       , ComplexTypeCollectionFieldAlwaysAddAllCharSequenceEnumerator, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsChars | OneFormatString)]
public class StringBuilderEnumeratorAlwaysAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerator<StringBuilder?>?>
  , ISupportsSingleFormatString
{
    public IEnumerator<StringBuilder?>? ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerator)
                                                , ComplexTypeCollectionFieldAlwaysAddAllStringBuilderEnumerator, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsAny | OneFormatString)]
public class MatchEnumeratorAlwaysAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<IEnumerator<T>?>, ISupportsSingleFormatString
{
    public IEnumerator<T>? ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<T>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatchEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerator)
                                                     , ComplexTypeCollectionFieldAlwaysAddAllMatchEnumerator, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | AlwaysWrites | AcceptsAny | OneFormatString)]
public class ObjectEnumeratorAlwaysAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<IEnumerator<T>?>, ISupportsSingleFormatString 
    where T : class
{
    public IEnumerator<T>? ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<T>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObjectEnumerate(nameof(ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerator)
                                                      , ComplexTypeCollectionFieldAlwaysAddAllObjectEnumerator, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}