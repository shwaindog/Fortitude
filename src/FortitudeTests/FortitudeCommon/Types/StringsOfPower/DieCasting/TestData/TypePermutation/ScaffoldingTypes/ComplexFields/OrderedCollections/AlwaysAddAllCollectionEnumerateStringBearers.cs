using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexFields.OrderedCollections;

public class BoolEnumerableAlwaysAddAllStringBearer(IEnumerable<bool>? value) : IStringBearer
{
    public IEnumerable<bool>? AlwaysAddAllBoolEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(AlwaysAddAllBoolEnumerable), AlwaysAddAllBoolEnumerable)
           .Complete();
}

public class NullableBoolEnumerableAlwaysAddAllStringBearer(IEnumerable<bool?>? value) : IStringBearer
{
    public IEnumerable<bool?>? AlwaysAddAllNullableBoolEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(AlwaysAddAllNullableBoolEnumerable), AlwaysAddAllNullableBoolEnumerable)
           .Complete();
}

public class SpanFormattableEnumerableAlwaysAddAllStringBearer<TFmt>(IEnumerable<TFmt>? value) : IStringBearer
    where TFmt : ISpanFormattable
{
    public IEnumerable<TFmt>? AlwaysAddAllSpanFormattableEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(AlwaysAddAllSpanFormattableEnumerable), AlwaysAddAllSpanFormattableEnumerable)
           .Complete();
}

public class NullableSpanFormattableEnumerableAlwaysAddAllStringBearer<TFmtStruct>(IEnumerable<TFmtStruct?>? value) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? AlwaysAddAllNullableSpanFormattableEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(AlwaysAddAllNullableSpanFormattableEnumerable), AlwaysAddAllNullableSpanFormattableEnumerable)
           .Complete();
}

public class CustomBearerEnumerableAlwaysAddAllStringBearer<TCloaked, TCloakedBase>(
    IEnumerable<TCloaked>? value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedBase
{
    public IEnumerable<TCloaked>? AlwaysAddAllCustomBearerEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate(nameof(AlwaysAddAllCustomBearerEnumerable), AlwaysAddAllCustomBearerEnumerable, palantírReveal)
           .Complete();
}

public class NullableCustomBearerEnumerableAlwaysAddAllStringBearer<TCloakedStruct>(
    IEnumerable<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? AlwaysAddAllNullableCustomBearerEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate(nameof(AlwaysAddAllNullableCustomBearerEnumerable), AlwaysAddAllNullableCustomBearerEnumerable, palantírReveal)
           .Complete();
}

public class StringBearerEnumerableAlwaysAddAllStringBearer<TBearer>(IEnumerable<TBearer>? value) : IStringBearer
    where TBearer : IStringBearer
{
    public IEnumerable<TBearer>? AlwaysAddAllStringBearerEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate(nameof(AlwaysAddAllStringBearerEnumerable), AlwaysAddAllStringBearerEnumerable)
           .Complete();
}

public class NullableStringBearerEnumerableAlwaysAddAllStringBearer<TBearerStruct>(IEnumerable<TBearerStruct?>? value) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? AlwaysAddAllNullableStringBearerEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate(nameof(AlwaysAddAllNullableStringBearerEnumerable), AlwaysAddAllNullableStringBearerEnumerable)
           .Complete();
}

public class StringEnumerableAlwaysAddAllStringBearer(IEnumerable<string?>? value) : IStringBearer
{
    public IEnumerable<string?>? AlwaysAddAllStringEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(AlwaysAddAllStringEnumerable), AlwaysAddAllStringEnumerable)
           .Complete();
}

public class CharSequenceEnumerableAlwaysAddAllStringBearer<TCharSeq>(IEnumerable<TCharSeq?>? value) : IStringBearer
    where TCharSeq : ICharSequence
{
    public IEnumerable<TCharSeq?>? AlwaysAddAllCharSequenceEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeqEnumerate(nameof(AlwaysAddAllCharSequenceEnumerable), AlwaysAddAllCharSequenceEnumerable)
           .Complete();
}

public class StringBuilderEnumerableAlwaysAddAllStringBearer(IEnumerable<StringBuilder?>? value) : IStringBearer
{
    public IEnumerable<StringBuilder?>? AlwaysAddAllStringBuilderEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(AlwaysAddAllStringBuilderEnumerable), AlwaysAddAllStringBuilderEnumerable)
           .Complete();
}

public class MatchEnumerableAlwaysAddAllStringBearer<T>(IEnumerable<T>? value) : IStringBearer
{
    public IEnumerable<T>? AlwaysAddAllMatchEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatchEnumerate(nameof(AlwaysAddAllMatchEnumerable), AlwaysAddAllMatchEnumerable)
           .Complete();
}

public class ObjectEnumerableAlwaysAddAllStringBearer(IEnumerable<object?>? value) : IStringBearer
{
    public IEnumerable<object?>? AlwaysAddAllObjectEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObjectEnumerate(nameof(AlwaysAddAllObjectEnumerable), AlwaysAddAllObjectEnumerable)
           .Complete();
}

public class BoolEnumeratorAlwaysAddAllStringBearer(IEnumerator<bool>? value) : IStringBearer
{
    public IEnumerator<bool>? AlwaysAddAllBoolEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(AlwaysAddAllBoolEnumerator), AlwaysAddAllBoolEnumerator)
           .Complete();
}

public class NullableBoolEnumeratorAlwaysAddAllStringBearer(IEnumerator<bool?>? value) : IStringBearer
{
    public IEnumerator<bool?>? AlwaysAddAllNullableBoolEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(AlwaysAddAllNullableBoolEnumerator), AlwaysAddAllNullableBoolEnumerator)
           .Complete();
}

public class SpanFormattableEnumeratorAlwaysAddAllStringBearer<TFmt>(IEnumerator<TFmt>? value) : IStringBearer
    where TFmt : ISpanFormattable
{
    public IEnumerator<TFmt>? AlwaysAddAllSpanFormattableEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(AlwaysAddAllSpanFormattableEnumerator), AlwaysAddAllSpanFormattableEnumerator)
           .Complete();
}

public class NullableSpanFormattableEnumeratorAlwaysAddAllStringBearer<TStructFmt>(IEnumerator<TStructFmt?>? value) : IStringBearer
    where TStructFmt : struct, ISpanFormattable
{
    public IEnumerator<TStructFmt?>? AlwaysAddAllNullableSpanFormattableEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(AlwaysAddAllNullableSpanFormattableEnumerator), AlwaysAddAllNullableSpanFormattableEnumerator)
           .Complete();
}

public class CustomBearerEnumeratorAlwaysAddAllStringBearer<TCloaked, TCloakedBase>(IEnumerator<TCloaked>? value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedBase
{
    public IEnumerator<TCloaked>? AlwaysAddAllCustomBearerEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate(nameof(AlwaysAddAllCustomBearerEnumerator), AlwaysAddAllCustomBearerEnumerator, palantírReveal)
           .Complete();
}

public class NullableCustomBearerEnumeratorAlwaysAddAllStringBearer<TCloakedStruct>(
    IEnumerator<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? AlwaysAddAllNullableCustomBearerEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate(nameof(AlwaysAddAllNullableCustomBearerEnumerator), AlwaysAddAllNullableCustomBearerEnumerator, palantírReveal)
           .Complete();
}

public class StringBearerEnumeratorAlwaysAddAllStringBearer<TBearer>(IEnumerator<TBearer>? value) : IStringBearer
    where TBearer : IStringBearer
{
    public IEnumerator<TBearer>? AlwaysAddAllStringBearerEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate(nameof(AlwaysAddAllStringBearerEnumerator), AlwaysAddAllStringBearerEnumerator)
           .Complete();
}

public class NullableStringBearerEnumeratorAlwaysAddAllStringBearer<TBearerStruct>(IEnumerator<TBearerStruct?>? value) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? AlwaysAddAllNullableStringBearerEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllEnumerate(nameof(AlwaysAddAllNullableStringBearerEnumerator), AlwaysAddAllNullableStringBearerEnumerator)
           .Complete();
}

public class StringEnumeratorAlwaysAddAllStringBearer(IEnumerator<string?>? value) : IStringBearer
{
    public IEnumerator<string?>? AlwaysAddAllStringEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(AlwaysAddAllStringEnumerator), AlwaysAddAllStringEnumerator)
           .Complete();
}

public class CharSequenceEnumeratorAlwaysAddAllStringBearer<TCharSeq>(IEnumerator<TCharSeq?>? value) : IStringBearer
    where TCharSeq : ICharSequence
{
    public IEnumerator<TCharSeq?>? AlwaysAddAllCharSequenceEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeqEnumerate(nameof(AlwaysAddAllCharSequenceEnumerator), AlwaysAddAllCharSequenceEnumerator)
           .Complete();
}

public class StringBuilderEnumeratorAlwaysAddAllStringBearer(IEnumerator<StringBuilder?>? value) : IStringBearer
{
    public IEnumerator<StringBuilder?>? AlwaysAddAllStringBuilderEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllEnumerate(nameof(AlwaysAddAllStringBuilderEnumerator), AlwaysAddAllStringBuilderEnumerator)
           .Complete();
}

public class MatchEnumeratorAlwaysAddAllStringBearer<T>(IEnumerator<T>? value) : IStringBearer
{
    public IEnumerator<T>? AlwaysAddAllMatchEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatchEnumerate(nameof(AlwaysAddAllMatchEnumerator), AlwaysAddAllMatchEnumerator)
           .Complete();
}

public class ObjectEnumeratorAlwaysAddAllStringBearer<T>(IEnumerator<T>? value) : IStringBearer
    where T : class
{
    public IEnumerator<T>? AlwaysAddAllObjectEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObjectEnumerate(nameof(AlwaysAddAllObjectEnumerator), AlwaysAddAllObjectEnumerator)
           .Complete();
}