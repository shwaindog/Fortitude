using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.TypeGeneratePartFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    OrderedCollectionFields;

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct)]
public class BoolEnumerableWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerable<bool>?>
{
    public IEnumerable<bool>? ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct)]
public class NullableBoolEnumerableWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerable<bool?>?>
{
    public IEnumerable<bool?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableEnumerableWhenPopulatedAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<IEnumerable<TFmt?>?>,
    ISupportsSingleFormatString where TFmt : ISpanFormattable
{
    public IEnumerable<TFmt?>? ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class NullableSpanFormattableEnumerableWhenPopulatedAddAllStringBearer<TFmtStruct> : IStringBearer, IMoldSupportedValue<IEnumerable<TFmtStruct?>?>
  , ISupportsSingleFormatString where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TFmtStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class CloakedBearerEnumerableWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase> : IStringBearer
  , IMoldSupportedValue<IEnumerable<TCloaked>?>, ISupportsSingleRevealer<TCloakedBase>
    where TCloaked : TCloakedBase
{
    public IEnumerable<TCloaked>? ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedBase> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerable, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class NullableCustomBearerEnumerableWhenPopulatedAddAllStringBearer<TCloakedStruct> : IStringBearer
  , IMoldSupportedValue<IEnumerable<TCloakedStruct?>?>, ISupportsSingleRevealer<TCloakedStruct>
    where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCustomBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TCloakedStruct?>? Value { get; set; }

    public PalantírReveal<TCloakedStruct> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCustomBearerEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCustomBearerEnumerable, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerEnumerableWhenPopulatedAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<IEnumerable<TBearer>?>
    where TBearer : IStringBearer
{
    public IEnumerable<TBearer>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TBearer>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerEnumerableWhenPopulatedAddAllStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<IEnumerable<TBearerStruct?>?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerable)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class StringEnumerableWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerable<string?>?>, ISupportsSingleFormatString
{
    public IEnumerable<string?>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class CharSequenceEnumerableWhenPopulatedAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<IEnumerable<TCharSeq?>?>
  , ISupportsSingleFormatString
    where TCharSeq : ICharSequence
{
    public IEnumerable<TCharSeq?>? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class StringBuilderEnumerableWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerable<StringBuilder?>?>
  , ISupportsSingleFormatString
{
    public IEnumerable<StringBuilder?>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsAny | OneFormatString)]
public class MatchEnumerableWhenPopulatedAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<IEnumerable<T>?>, ISupportsSingleFormatString
{
    public IEnumerable<T>? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<T>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsAny | OneFormatString)]
public class ObjectEnumerableWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerable<object?>?>, ISupportsSingleFormatString
{
    public IEnumerable<object?>? ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerable
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<object?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerable)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerable, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct)]
public class BoolEnumeratorWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerator<bool>?>
{
    public IEnumerator<bool>? ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<bool>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllBoolEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct)]
public class NullableBoolEnumeratorWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerator<bool?>?>
{
    public IEnumerator<bool?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<bool?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableBoolEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class SpanFormattableEnumeratorWhenPopulatedAddAllStringBearer<TFmt> : IStringBearer, IMoldSupportedValue<IEnumerator<TFmt>?>
  , ISupportsSingleFormatString where TFmt : ISpanFormattable
{
    public IEnumerator<TFmt>? ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TFmt>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllSpanFormattableEnumerator, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct | AcceptsSpanFormattable |
                  AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | OneFormatString)]
public class NullableSpanFormattableEnumeratorWhenPopulatedAddAllStringBearer<TStructFmt> : IStringBearer, IMoldSupportedValue<IEnumerator<TStructFmt?>?>
  , ISupportsSingleFormatString where TStructFmt : struct, ISpanFormattable
{
    public IEnumerator<TStructFmt?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TStructFmt?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableSpanFormattableEnumerator, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class CloakedBearerEnumeratorWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase> : IStringBearer, IMoldSupportedValue<IEnumerator<TCloaked>?>
  , ISupportsSingleRevealer<TCloakedBase> where TCloaked : TCloakedBase
{
    public IEnumerator<TCloaked>? ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TCloaked>? Value { get; set; }

    public PalantírReveal<TCloakedBase> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCloakedBearerEnumerator, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct
                | AcceptsSpanFormattable | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | AcceptsStringBearer |
                  OnePalantirRevealer)]
public class NullableCloakedBearerEnumeratorWhenPopulatedAddAllStringBearer<TCloakedStruct> : IStringBearer
  , IMoldSupportedValue<IEnumerator<TCloakedStruct?>?>, ISupportsSingleRevealer<TCloakedStruct>
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TCloakedStruct?>? Value { get; set; }

    public PalantírReveal<TCloakedStruct> PalantirRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableCloakedBearerEnumerator, PalantirRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsStringBearer)]
public class StringBearerEnumeratorWhenPopulatedAddAllStringBearer<TBearer> : IStringBearer, IMoldSupportedValue<IEnumerator<TBearer>?>
    where TBearer : IStringBearer
{
    public IEnumerator<TBearer>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TBearer>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBearerEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsNullableStruct | AcceptsStringBearer)]
public class NullableStringBearerEnumeratorWhenPopulatedAddAllStringBearer<TBearerStruct> : IStringBearer, IMoldSupportedValue<IEnumerator<TBearerStruct?>?>
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TBearerStruct?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllNullableStringBearerEnumerator)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class StringEnumeratorWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerator<string?>?>, ISupportsSingleFormatString
{
    public IEnumerator<string?>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<string?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringEnumerator, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class CharSequenceEnumeratorWhenPopulatedAddAllStringBearer<TCharSeq> : IStringBearer, IMoldSupportedValue<IEnumerator<TCharSeq?>?>
  , ISupportsSingleFormatString where TCharSeq : ICharSequence
{
    public IEnumerator<TCharSeq?>? ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<TCharSeq?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeqEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllCharSequenceEnumerator, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsChars | OneFormatString)]
public class StringBuilderEnumeratorWhenPopulatedAddAllStringBearer : IStringBearer, IMoldSupportedValue<IEnumerator<StringBuilder?>?>
  , ISupportsSingleFormatString
{
    public IEnumerator<StringBuilder?>? ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<StringBuilder?>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllStringBuilderEnumerator, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsAny | OneFormatString)]
public class MatchEnumeratorWhenPopulatedAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<IEnumerator<T>?>, ISupportsSingleFormatString
{
    public IEnumerator<T>? ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<T>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatchEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllMatchEnumerator, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}

[TypeGeneratePart(ComplexType | AcceptsCollection | OnlyPopulatedWrites | AcceptsAny | OneFormatString)]
public class ObjectEnumeratorWhenPopulatedAddAllStringBearer<T> : IStringBearer, IMoldSupportedValue<IEnumerator<T>?>, ISupportsSingleFormatString
    where T : class
{
    public IEnumerator<T>? ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerator
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<T>? Value { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObjectEnumerate
               (nameof(ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerator)
              , ComplexTypeCollectionFieldWhenPopulatedAddAllObjectEnumerator, FormatString)
           .Complete();

    public string? FormatString { get; set; }
}
