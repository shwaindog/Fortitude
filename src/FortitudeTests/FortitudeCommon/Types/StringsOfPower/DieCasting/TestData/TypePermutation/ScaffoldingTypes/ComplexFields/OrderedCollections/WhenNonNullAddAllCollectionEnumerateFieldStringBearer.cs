using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexFields.OrderedCollections;

public class BoolEnumerableWhenNonNullAddAllStringBearer(IEnumerable<bool>? value) : IStringBearer
{
    public IEnumerable<bool>? WhenNonNullAddAllBoolEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate(nameof(WhenNonNullAddAllBoolEnumerable), WhenNonNullAddAllBoolEnumerable)
           .Complete();
}

public class NullableBoolEnumerableWhenNonNullAddAllStringBearer(IEnumerable<bool?>? value) : IStringBearer
{
    public IEnumerable<bool?>? WhenNonNullAddAllNullableBoolEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate(nameof(WhenNonNullAddAllNullableBoolEnumerable), WhenNonNullAddAllNullableBoolEnumerable)
           .Complete();
}

public class SpanFormattableEnumerableWhenNonNullAddAllStringBearer<TFmt>(IEnumerable<TFmt>? value) : IStringBearer
    where TFmt : ISpanFormattable
{
    public IEnumerable<TFmt>? WhenNonNullAddAllSpanFormattableEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate(nameof(WhenNonNullAddAllSpanFormattableEnumerable), WhenNonNullAddAllSpanFormattableEnumerable)
           .Complete();
}

public class NullableSpanFormattableEnumerableWhenNonNullAddAllStringBearer<TStructFmt>(IEnumerable<TStructFmt?>? value) : IStringBearer
    where TStructFmt : struct, ISpanFormattable
{
    public IEnumerable<TStructFmt?>? WhenNonNullAddAllNullableSpanFormattableEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate(nameof(WhenNonNullAddAllNullableSpanFormattableEnumerable), WhenNonNullAddAllNullableSpanFormattableEnumerable)
           .Complete();
}

public class CustomBearerEnumerableWhenNonNullAddAllStringBearer<TCloaked, TCloakedBase>(
    IEnumerable<TCloaked>? value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedBase
{
    public IEnumerable<TCloaked>? WhenNonNullAddAllCustomBearerEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate(nameof(WhenNonNullAddAllCustomBearerEnumerable), WhenNonNullAddAllCustomBearerEnumerable, palantírReveal)
           .Complete();
}

public class NullableCustomBearerEnumerableWhenNonNullAddAllStringBearer<TCloakedStruct>(
    IEnumerable<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? WhenNonNullAddAllNullableCustomBearerEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate(nameof(WhenNonNullAddAllNullableCustomBearerEnumerable), WhenNonNullAddAllNullableCustomBearerEnumerable, palantírReveal)
           .Complete();
}

public class StringBearerEnumerableWhenNonNullAddAllStringBearer<TBearer>(IEnumerable<TBearer>? value) : IStringBearer
    where TBearer : IStringBearer
{
    public IEnumerable<TBearer>? WhenNonNullAddAllStringBearerEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate(nameof(WhenNonNullAddAllStringBearerEnumerable), WhenNonNullAddAllStringBearerEnumerable)
           .Complete();
}

public class NullableStringBearerEnumerableWhenNonNullAddAllStringBearer<TBearerStruct>(IEnumerable<TBearerStruct?>? value) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? WhenNonNullAddAllNullableStringBearerEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate(nameof(WhenNonNullAddAllNullableStringBearerEnumerable), WhenNonNullAddAllNullableStringBearerEnumerable)
           .Complete();
}

public class StringEnumerableWhenNonNullAddAllStringBearer(IEnumerable<string?>? value) : IStringBearer
{
    public IEnumerable<string?>? WhenNonNullAddAllStringEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate(nameof(WhenNonNullAddAllStringEnumerable), WhenNonNullAddAllStringEnumerable)
           .Complete();
}

public class CharSequenceEnumerableWhenNonNullAddAllStringBearer<TCharSeq>(IEnumerable<TCharSeq?>? value) : IStringBearer
    where TCharSeq : ICharSequence
{
    public IEnumerable<TCharSeq?>? WhenNonNullAddAllCharSequenceEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeqEnumerate(nameof(WhenNonNullAddAllCharSequenceEnumerable), WhenNonNullAddAllCharSequenceEnumerable)
           .Complete();
}

public class StringBuilderEnumerableWhenNonNullAddAllStringBearer(IEnumerable<StringBuilder?>? value) : IStringBearer
{
    public IEnumerable<StringBuilder?>? WhenNonNullAddAllStringBuilderEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate(nameof(WhenNonNullAddAllStringBuilderEnumerable), WhenNonNullAddAllStringBuilderEnumerable)
           .Complete();
}

public class MatchEnumerableWhenNonNullAddAllStringBearer<T>(IEnumerable<T>? value) : IStringBearer
{
    public IEnumerable<T>? WhenNonNullAddAllStringBuilderEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatchEnumerate(nameof(WhenNonNullAddAllStringBuilderEnumerable), WhenNonNullAddAllStringBuilderEnumerable)
           .Complete();
}

public class BoolEnumeratorWhenNonNullAddAllStringBearer(IEnumerator<bool>? value) : IStringBearer
{
    public IEnumerator<bool>? WhenNonNullAddAllBoolEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate(nameof(WhenNonNullAddAllBoolEnumerator), WhenNonNullAddAllBoolEnumerator)
           .Complete();
}

public class NullableBoolEnumeratorWhenNonNullAddAllStringBearer(IEnumerator<bool?>? value) : IStringBearer
{
    public IEnumerator<bool?>? WhenNonNullAddAllNullableBoolEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate(nameof(WhenNonNullAddAllNullableBoolEnumerator), WhenNonNullAddAllNullableBoolEnumerator)
           .Complete();
}

public class SpanFormattableEnumeratorWhenNonNullAddAllStringBearer<TFmt>(IEnumerator<TFmt>? value) : IStringBearer
    where TFmt : ISpanFormattable
{
    public IEnumerator<TFmt>? WhenNonNullAddAllSpanFormattableEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate(nameof(WhenNonNullAddAllSpanFormattableEnumerator), WhenNonNullAddAllSpanFormattableEnumerator)
           .Complete();
}

public class NullableSpanFormattableEnumeratorWhenNonNullAddAllStringBearer<TFmtStruct>(IEnumerator<TFmtStruct?>? value) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerator<TFmtStruct?>? WhenNonNullAddAllNullableSpanFormattableEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate(nameof(WhenNonNullAddAllNullableSpanFormattableEnumerator), WhenNonNullAddAllNullableSpanFormattableEnumerator)
           .Complete();
}

public class CustomBearerEnumeratorWhenNonNullAddAllStringBearer<TCloaked, TCloakedBase>(
    IEnumerator<TCloaked>? value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedBase
{
    public IEnumerator<TCloaked>? WhenNonNullAddAllCustomBearerEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate(nameof(WhenNonNullAddAllCustomBearerEnumerator), WhenNonNullAddAllCustomBearerEnumerator, palantírReveal)
           .Complete();
}

public class NullableCustomBearerEnumeratorWhenNonNullAddAllStringBearer<TCloakedStruct>(
    IEnumerator<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? WhenNonNullAddAllNullableCustomBearerEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate(nameof(WhenNonNullAddAllNullableCustomBearerEnumerator), WhenNonNullAddAllNullableCustomBearerEnumerator, palantírReveal)
           .Complete();
}

public class StringBearerEnumeratorWhenNonNullAddAllStringBearer<TBearer>(IEnumerator<TBearer>? value) : IStringBearer
    where TBearer : IStringBearer
{
    public IEnumerator<TBearer>? WhenNonNullAddAllStringBearerEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate(nameof(WhenNonNullAddAllStringBearerEnumerator), WhenNonNullAddAllStringBearerEnumerator)
           .Complete();
}

public class NullableStringBearerEnumeratorWhenNonNullAddAllStringBearer<TBearerStruct>(IEnumerator<TBearerStruct?>? value) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? WhenNonNullAddAllNullableStringBearerEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAllEnumerate(nameof(WhenNonNullAddAllNullableStringBearerEnumerator), WhenNonNullAddAllNullableStringBearerEnumerator)
           .Complete();
}

public class StringEnumeratorWhenNonNullAddAllStringBearer(IEnumerator<string?>? value) : IStringBearer
{
    public IEnumerator<string?>? WhenNonNullAddAllStringEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate(nameof(WhenNonNullAddAllStringEnumerator), WhenNonNullAddAllStringEnumerator)
           .Complete();
}

public class CharSequenceEnumeratorWhenNonNullAddAllStringBearer<TCharSeq>(IEnumerator<TCharSeq?>? value) : IStringBearer
    where TCharSeq : ICharSequence
{
    public IEnumerator<TCharSeq?>? WhenNonNullAddAllCharSequenceEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeqEnumerate(nameof(WhenNonNullAddAllCharSequenceEnumerator), WhenNonNullAddAllCharSequenceEnumerator)
           .Complete();
}

public class StringBuilderEnumeratorWhenNonNullAddAllStringBearer(IEnumerator<StringBuilder?>? value) : IStringBearer
{
    public IEnumerator<StringBuilder?>? WhenNonNullAddAllStringBuilderEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllEnumerate(nameof(WhenNonNullAddAllStringBuilderEnumerator), WhenNonNullAddAllStringBuilderEnumerator)
           .Complete();
}

public class MatchEnumeratorWhenNonNullAddAllStringBearer<T>(IEnumerator<T>? value) : IStringBearer
{
    public IEnumerator<T>? WhenNonNullAddAllMatchEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatchEnumerate(nameof(WhenNonNullAddAllMatchEnumerator), WhenNonNullAddAllMatchEnumerator)
           .Complete();
}