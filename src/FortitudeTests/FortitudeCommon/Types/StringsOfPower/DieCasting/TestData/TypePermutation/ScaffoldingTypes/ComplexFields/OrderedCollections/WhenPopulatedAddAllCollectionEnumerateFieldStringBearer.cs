using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexFields.OrderedCollections;

public class BoolEnumerableWhenPopulatedAddAllStringBearer(IEnumerable<bool>? value) : IStringBearer
{
    public IEnumerable<bool>? WhenPopulatedAddAllBoolEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate(nameof(WhenPopulatedAddAllBoolEnumerable), WhenPopulatedAddAllBoolEnumerable)
           .Complete();
}

public class NullableBoolEnumerableWhenPopulatedAddAllStringBearer(IEnumerable<bool?>? value) : IStringBearer
{
    public IEnumerable<bool?>? WhenPopulatedAddAllNullableBoolEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate(nameof(WhenPopulatedAddAllNullableBoolEnumerable), WhenPopulatedAddAllNullableBoolEnumerable)
           .Complete();
}

public class SpanFormattableEnumerableWhenPopulatedAddAllStringBearer<TFmt>(IEnumerable<TFmt>? value) : IStringBearer
    where TFmt : ISpanFormattable
{
    public IEnumerable<TFmt>? WhenPopulatedAddAllSpanFormattableEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate(nameof(WhenPopulatedAddAllSpanFormattableEnumerable), WhenPopulatedAddAllSpanFormattableEnumerable)
           .Complete();
}

public class NullableSpanFormattableEnumerableWhenPopulatedAddAllStringBearer<TFmtStruct>(IEnumerable<TFmtStruct?>? value) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public IEnumerable<TFmtStruct?>? WhenPopulatedAddAllNullableSpanFormattableEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate(nameof(WhenPopulatedAddAllNullableSpanFormattableEnumerable), WhenPopulatedAddAllNullableSpanFormattableEnumerable)
           .Complete();
}

public class CustomBearerEnumerableWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase>(
    IEnumerable<TCloaked>? value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedBase
{
    public IEnumerable<TCloaked>? WhenPopulatedAddAllCustomBearerEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate(nameof(WhenPopulatedAddAllCustomBearerEnumerable), WhenPopulatedAddAllCustomBearerEnumerable, palantírReveal)
           .Complete();
}

public class NullableCustomBearerEnumerableWhenPopulatedAddAllStringBearer<TCloakedStruct>(
    IEnumerable<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public IEnumerable<TCloakedStruct?>? WhenPopulatedAddAllNullableCustomBearerEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate(nameof(WhenPopulatedAddAllNullableCustomBearerEnumerable), WhenPopulatedAddAllNullableCustomBearerEnumerable, palantírReveal)
           .Complete();
}

public class StringBearerEnumerableWhenPopulatedAddAllStringBearer<TBearer>(IEnumerable<TBearer>? value) : IStringBearer
    where TBearer : IStringBearer
{
    public IEnumerable<TBearer>? WhenPopulatedAddAllStringBearerEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate(nameof(WhenPopulatedAddAllStringBearerEnumerable), WhenPopulatedAddAllStringBearerEnumerable)
           .Complete();
}

public class NullableStringBearerEnumerableWhenPopulatedAddAllStringBearer<TBearerStruct>(IEnumerable<TBearerStruct?>? value) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerable<TBearerStruct?>? WhenPopulatedAddAllNullableStringBearerEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate(nameof(WhenPopulatedAddAllNullableStringBearerEnumerable), WhenPopulatedAddAllNullableStringBearerEnumerable)
           .Complete();
}

public class StringEnumerableWhenPopulatedAddAllStringBearer(IEnumerable<string?>? value) : IStringBearer
{
    public IEnumerable<string?>? WhenPopulatedAddAllStringEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate(nameof(WhenPopulatedAddAllStringEnumerable), WhenPopulatedAddAllStringEnumerable)
           .Complete();
}

public class CharSequenceEnumerableWhenPopulatedAddAllStringBearer<TCharSeq>(IEnumerable<TCharSeq?>? value) : IStringBearer
    where TCharSeq : ICharSequence
{
    public IEnumerable<TCharSeq?>? WhenPopulatedAddAllCharSequenceEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeqEnumerate(nameof(WhenPopulatedAddAllCharSequenceEnumerable), WhenPopulatedAddAllCharSequenceEnumerable)
           .Complete();
}

public class StringBuilderEnumerableWhenPopulatedAddAllStringBearer(IEnumerable<StringBuilder?>? value) : IStringBearer
{
    public IEnumerable<StringBuilder?>? WhenPopulatedAddAllStringBuilderEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate(nameof(WhenPopulatedAddAllStringBuilderEnumerable), WhenPopulatedAddAllStringBuilderEnumerable)
           .Complete();
}

public class MatchEnumerableWhenPopulatedAddAllStringBearer<T>(IEnumerable<T>? value) : IStringBearer
{
    public IEnumerable<T>? WhenPopulatedAddAllMatchEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatchEnumerate(nameof(WhenPopulatedAddAllMatchEnumerable), WhenPopulatedAddAllMatchEnumerable)
           .Complete();
}

public class ObjectEnumerableWhenPopulatedAddAllStringBearer<T>(IEnumerable<T>? value) : IStringBearer
where T : class
{
    public IEnumerable<T>? WhenPopulatedAddAllObjectEnumerable { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObjectEnumerate(nameof(WhenPopulatedAddAllObjectEnumerable), WhenPopulatedAddAllObjectEnumerable)
           .Complete();
}

public class BoolEnumeratorWhenPopulatedAddAllStringBearer(IEnumerator<bool>? value) : IStringBearer
{
    public IEnumerator<bool>? WhenPopulatedAddAllBoolEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate(nameof(WhenPopulatedAddAllBoolEnumerator), WhenPopulatedAddAllBoolEnumerator)
           .Complete();
}

public class NullableBoolEnumeratorWhenPopulatedAddAllStringBearer(IEnumerator<bool?>? value) : IStringBearer
{
    public IEnumerator<bool?>? WhenPopulatedAddAllNullableBoolEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate(nameof(WhenPopulatedAddAllNullableBoolEnumerator), WhenPopulatedAddAllNullableBoolEnumerator)
           .Complete();
}

public class SpanFormattableEnumeratorWhenPopulatedAddAllStringBearer<TFmt>(IEnumerator<TFmt>? value) : IStringBearer
    where TFmt : ISpanFormattable
{
    public IEnumerator<TFmt>? WhenPopulatedAddAllSpanFormattableEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate(nameof(WhenPopulatedAddAllSpanFormattableEnumerator), WhenPopulatedAddAllSpanFormattableEnumerator)
           .Complete();
}

public class NullableSpanFormattableEnumeratorWhenPopulatedAddAllStringBearer<TStructFmt>(IEnumerator<TStructFmt?>? value) : IStringBearer
    where TStructFmt : struct, ISpanFormattable
{
    public IEnumerator<TStructFmt?>? WhenPopulatedAddAllNullableSpanFormattableEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate(nameof(WhenPopulatedAddAllNullableSpanFormattableEnumerator), WhenPopulatedAddAllNullableSpanFormattableEnumerator)
           .Complete();
}

public class CustomBearerEnumeratorWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase>(IEnumerator<TCloaked>? value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedBase
{
    public IEnumerator<TCloaked>? WhenPopulatedAddAllCustomBearerEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate(nameof(WhenPopulatedAddAllCustomBearerEnumerator), WhenPopulatedAddAllCustomBearerEnumerator, palantírReveal)
           .Complete();
}

public class NullableCustomBearerEnumeratorWhenPopulatedAddAllStringBearer<TCloakedStruct>(
    IEnumerator<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public IEnumerator<TCloakedStruct?>? WhenPopulatedAddAllNullableCustomBearerEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate(nameof(WhenPopulatedAddAllNullableCustomBearerEnumerator), WhenPopulatedAddAllNullableCustomBearerEnumerator, palantírReveal)
           .Complete();
}

public class StringBearerEnumeratorWhenPopulatedAddAllStringBearer<TBearer>(IEnumerator<TBearer>? value) : IStringBearer
    where TBearer : IStringBearer
{
    public IEnumerator<TBearer>? WhenPopulatedAddAllStringBearerEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate(nameof(WhenPopulatedAddAllStringBearerEnumerator), WhenPopulatedAddAllStringBearerEnumerator)
           .Complete();
}

public class NullableStringBearerEnumeratorWhenPopulatedAddAllStringBearer<TBearerStruct>(IEnumerator<TBearerStruct?>? value) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public IEnumerator<TBearerStruct?>? WhenPopulatedAddAllNullableStringBearerEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAllEnumerate(nameof(WhenPopulatedAddAllNullableStringBearerEnumerator), WhenPopulatedAddAllNullableStringBearerEnumerator)
           .Complete();
}

public class StringEnumeratorWhenPopulatedAddAllStringBearer(IEnumerator<string?>? value) : IStringBearer
{
    public IEnumerator<string?>? WhenPopulatedAddAllStringEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate(nameof(WhenPopulatedAddAllStringEnumerator), WhenPopulatedAddAllStringEnumerator)
           .Complete();
}

public class CharSequenceEnumeratorWhenPopulatedAddAllStringBearer<TCharSeq>(IEnumerator<TCharSeq?>? value) : IStringBearer
    where TCharSeq : ICharSequence
{
    public IEnumerator<TCharSeq?>? WhenPopulatedAddAllCharSequenceEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeqEnumerate(nameof(WhenPopulatedAddAllCharSequenceEnumerator), WhenPopulatedAddAllCharSequenceEnumerator)
           .Complete();
}

public class StringBuilderEnumeratorWhenPopulatedAddAllStringBearer(IEnumerator<StringBuilder?>? value) : IStringBearer
{
    public IEnumerator<StringBuilder?>? WhenPopulatedAddAllStringBuilderEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllEnumerate(nameof(WhenPopulatedAddAllStringBuilderEnumerator), WhenPopulatedAddAllStringBuilderEnumerator)
           .Complete();
}

public class MatchEnumeratorWhenPopulatedAddAllStringBearer<T>(IEnumerator<T>? value) : IStringBearer
{
    public IEnumerator<T>? WhenPopulatedAddAllMatchEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatchEnumerate(nameof(WhenPopulatedAddAllMatchEnumerator), WhenPopulatedAddAllMatchEnumerator)
           .Complete();
}

public class ObjectEnumeratorWhenPopulatedAddAllStringBearer<T>(IEnumerator<T>? value) : IStringBearer
    where T : class
{
    public IEnumerator<T>? WhenPopulatedAddAllObjectEnumerator { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObjectEnumerate(nameof(WhenPopulatedAddAllObjectEnumerator), WhenPopulatedAddAllObjectEnumerator)
           .Complete();
}