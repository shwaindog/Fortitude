using System.Net;
using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexType.OrderedCollectionFields;

public class BoolSpanAlwaysAddAllStringBearer(bool[] value) : IStringBearer
{
    public bool[] AlwaysAddAllBoolSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllBoolSpan), AlwaysAddAllBoolSpan.AsSpan())
           .Complete();
}

public class NullableBoolSpanAlwaysAddAllStringBearer(bool?[] value) : IStringBearer
{
    public bool?[] AlwaysAddAllNullableBoolSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllNullableBoolSpan), AlwaysAddAllNullableBoolSpan.AsSpan())
           .Complete();
}

public class SpanFormattableSpanAlwaysAddAllStringBearer<TFmt>(TFmt[] value) : IStringBearer
    where TFmt : ISpanFormattable
{
    public TFmt[] AlwaysAddAllSpanFormattableStructSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllSpanFormattableStructSpan), AlwaysAddAllSpanFormattableStructSpan.AsSpan())
           .Complete();
}

public class SpanFormattableNullableClassSpanAlwaysAddAllStringBearer<TFmt>(TFmt?[] value) : IStringBearer
    where TFmt : class, ISpanFormattable
{
    public TFmt?[] AlwaysAddAllSpanFormattableNullableClassSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllNullable(nameof(AlwaysAddAllSpanFormattableNullableClassSpan), AlwaysAddAllSpanFormattableNullableClassSpan.AsSpan())
           .Complete();
}

public class SpanFormattableNullableStructSpanAlwaysAddAllStringBearer<TFmtStruct>(TFmtStruct?[]? value) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? AlwaysAddAllSpanFormattableSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllSpanFormattableSpan), AlwaysAddAllSpanFormattableSpan)
           .Complete();
}

public class IpAddressSpanAlwaysAddAllStringBearer(IPAddress?[]? value) : IStringBearer
{
    public IPAddress?[]? AlwaysAddAllIpAddressSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllNullable(nameof(AlwaysAddAllIpAddressSpan), AlwaysAddAllIpAddressSpan.AsSpan())
           .Complete();
}

public class NullableSpanFormattableSpanAlwaysAddAllStringBearer<TFmtStruct>(TFmtStruct?[]? value) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? AlwaysAddAllNullableSpanFormattableSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllNullableSpanFormattableSpan), AlwaysAddAllNullableSpanFormattableSpan.AsSpan())
           .Complete();
}

public class CloakedBearerSpanAlwaysAddAllStringBearer<TCloaked, TCloakedBase>(
    TCloaked[] value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedBase
{
    public TCloaked[] AlwaysAddAllCloakedBearerSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(AlwaysAddAllCloakedBearerSpan), AlwaysAddAllCloakedBearerSpan.AsSpan(), palantírReveal)
           .Complete();
}

public class CloakedBearerNullableRefSpanAlwaysAddAllStringBearer<TCloaked, TCloakedBase>(
    TCloaked?[] value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : class, TCloakedBase
{
    public TCloaked?[] AlwaysAddAllCloakedBearerNullableSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllNullable(nameof(AlwaysAddAllCloakedBearerNullableSpan), AlwaysAddAllCloakedBearerNullableSpan.AsSpan(), palantírReveal)
           .Complete();
}

public class NullableCustomBearerSpanAlwaysAddAllStringBearer<TCloakedStruct>(
    TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? AlwaysAddAllNullableCustomBearerSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(AlwaysAddAllNullableCustomBearerSpan), AlwaysAddAllNullableCustomBearerSpan.AsSpan(), palantírReveal)
           .Complete();
}

public class StringBearerSpanAlwaysAddAllStringBearer<TBearer>(TBearer[]? value) : IStringBearer
    where TBearer : IStringBearer
{
    public TBearer[]? AlwaysAddAllStringBearerSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(AlwaysAddAllStringBearerSpan), AlwaysAddAllStringBearerSpan.AsSpan())
           .Complete();
}

public class StringBearerNullableRefSpanAlwaysAddAllStringBearer<TBearer>(TBearer?[]? value) : IStringBearer
    where TBearer : class, IStringBearer
{
    public TBearer?[]? AlwaysAddAllStringBearerSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllNullable(nameof(AlwaysAddAllStringBearerSpan), AlwaysAddAllStringBearerSpan.AsSpan())
           .Complete();
}

public class NullableStringBearerSpanAlwaysAddAllStringBearer<TBearerStruct>(TBearerStruct?[]? value) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? AlwaysAddAllNullableStringBearerSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(AlwaysAddAllNullableStringBearerSpan), AlwaysAddAllNullableStringBearerSpan.AsSpan())
           .Complete();
}

public class StringSpanAlwaysAddAllStringBearer(string[]? value) : IStringBearer
{
    public string[]? AlwaysAddAllStringSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllStringSpan), AlwaysAddAllStringSpan.AsSpan())
           .Complete();
}

public class StringNullableSpanAlwaysAddAllStringBearer(string?[]? value) : IStringBearer
{
    public string?[]? AlwaysAddAllStringSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllNullable(nameof(AlwaysAddAllStringSpan), AlwaysAddAllStringSpan.AsSpan())
           .Complete();
}

public class CharSequenceSpanAlwaysAddAllStringBearer<TCharSeq>(TCharSeq[]? value) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq[]? AlwaysAddAllCharSequenceSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeq(nameof(AlwaysAddAllCharSequenceSpan), AlwaysAddAllCharSequenceSpan.AsSpan())
           .Complete();
}

public class CharSequenceNullableSpanAlwaysAddAllStringBearer<TCharSeq>(TCharSeq?[]? value) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? AlwaysAddAllCharSequenceNullableSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeqNullable(nameof(AlwaysAddAllCharSequenceNullableSpan), AlwaysAddAllCharSequenceNullableSpan.AsSpan())
           .Complete();
}

public class StringBuilderSpanAlwaysAddAllStringBearer(StringBuilder[] value) : IStringBearer
{
    public StringBuilder[] AlwaysAddAllStringBuilderSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllStringBuilderSpan), AlwaysAddAllStringBuilderSpan.AsSpan())
           .Complete();
}

public class StringBuilderNullableSpanAlwaysAddAllStringBearer(StringBuilder?[] value) : IStringBearer
{
    public StringBuilder?[] AlwaysAddAllStringBuilderSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllNullable(nameof(AlwaysAddAllStringBuilderSpan), AlwaysAddAllStringBuilderSpan.AsSpan())
           .Complete();
}

public class MatchSpanAlwaysAddAllStringBearer<T>(T[]? value) : IStringBearer
{
    public T[]? AlwaysAddAllMatchSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatch(nameof(AlwaysAddAllMatchSpan), AlwaysAddAllMatchSpan.AsSpan())
           .Complete();
}

public class MatchNullableSpanAlwaysAddAllStringBearer<T>(T?[]? value) : IStringBearer
{
    public T?[]? AlwaysAddAllMatchNullableSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatch(nameof(AlwaysAddAllMatchNullableSpan), AlwaysAddAllMatchNullableSpan.AsSpan())
           .Complete();
}

public class ObjectSpanAlwaysAddAllStringBearer(object[]? value) : IStringBearer
{
    public object[]? AlwaysAddAllObjectSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObject(nameof(AlwaysAddAllObjectSpan), AlwaysAddAllObjectSpan.AsSpan())
           .Complete();
}

public class ObjectNullableSpanAlwaysAddAllStringBearer(object?[]? value) : IStringBearer
{
    public object?[]? AlwaysAddAllObjectNullableRefSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObjectNullable(nameof(AlwaysAddAllObjectNullableRefSpan), AlwaysAddAllObjectNullableRefSpan.AsSpan())
           .Complete();
}

public class BoolReadOnlySpanAlwaysAddAllStringBearer(bool[] value) : IStringBearer
{
    public bool[] AlwaysAddAllBoolReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllBoolReadOnlySpan), (ReadOnlySpan<bool>)AlwaysAddAllBoolReadOnlySpan.AsSpan())
           .Complete();
}

public class NullableBoolReadOnlySpanAlwaysAddAllStringBearer(bool?[] value) : IStringBearer
{
    public bool?[] AlwaysAddAllNullableBoolReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllNullableBoolReadOnlySpan), (ReadOnlySpan<bool?>)AlwaysAddAllNullableBoolReadOnlySpan.AsSpan())
           .Complete();
}

public class SpanFormattableReadOnlySpanAlwaysAddAllStringBearer<TFmt>(TFmt[] value) : IStringBearer
    where TFmt : ISpanFormattable
{
    public TFmt[] AlwaysAddAllSpanFormattableReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllSpanFormattableReadOnlySpan), (ReadOnlySpan<TFmt>)AlwaysAddAllSpanFormattableReadOnlySpan.AsSpan())
           .Complete();
}

public class SpanFormattableNullableRefReadOnlySpanAlwaysAddAllStringBearer<TFmt>(TFmt?[] value) : IStringBearer
    where TFmt : class, ISpanFormattable
{
    public TFmt?[] AlwaysAddAllSpanFormattableNullableRefReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllNullable(nameof(AlwaysAddAllSpanFormattableNullableRefReadOnlySpan), (ReadOnlySpan<TFmt?>)(AlwaysAddAllSpanFormattableNullableRefReadOnlySpan.AsSpan()))
           .Complete();
}

public class SpanFormattableNullableStructReadOnlySpanAlwaysAddAllStringBearer<TFmtStruct>(TFmtStruct?[]? value) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? AlwaysAddAllSpanFormattableReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllSpanFormattableReadOnlySpan), (ReadOnlySpan<TFmtStruct?>)AlwaysAddAllSpanFormattableReadOnlySpan.AsSpan())
           .Complete();
}

public class IpAddressReadOnlySpanAlwaysAddAllStringBearer(IPAddress?[]? value) : IStringBearer
{
    public IPAddress?[]? AlwaysAddAllIpAddressReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllNullable(nameof(AlwaysAddAllIpAddressReadOnlySpan), (ReadOnlySpan<IPAddress?>)AlwaysAddAllIpAddressReadOnlySpan.AsSpan())
           .Complete();
}

public class NullableReadOnlySpanFormattableSpanAlwaysAddAllStringBearer<TFmtStruct>(TFmtStruct?[]? value) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? AlwaysAddAllNullableSpanFormattableReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllNullableSpanFormattableReadOnlySpan), (ReadOnlySpan<TFmtStruct?>)AlwaysAddAllNullableSpanFormattableReadOnlySpan.AsSpan())
           .Complete();
}

public class CloakedBearerReadOnlySpanAlwaysAddAllStringBearer<TCloaked, TCloakedBase>(
    TCloaked[] value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedBase
{
    public TCloaked[] AlwaysAddAllCloakedBearerReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(AlwaysAddAllCloakedBearerReadOnlySpan), (ReadOnlySpan<TCloaked>)AlwaysAddAllCloakedBearerReadOnlySpan.AsSpan(), palantírReveal)
           .Complete();
}

public class CloakedBearerNullableRefReadOnlySpanAlwaysAddAllStringBearer<TCloaked, TCloakedBase>(
    TCloaked?[] value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : class, TCloakedBase
{
    public TCloaked?[] AlwaysAddAllCloakedBearerNullableSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllNullable(nameof(AlwaysAddAllCloakedBearerNullableSpan), (ReadOnlySpan<TCloaked?>)AlwaysAddAllCloakedBearerNullableSpan.AsSpan(), palantírReveal)
           .Complete();
}

public class NullableCustomBearerReadOnlySpanAlwaysAddAllStringBearer<TCloakedStruct>(
    TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? AlwaysAddAllNullableCustomBearerReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(AlwaysAddAllNullableCustomBearerReadOnlySpan), (ReadOnlySpan<TCloakedStruct?>)AlwaysAddAllNullableCustomBearerReadOnlySpan.AsSpan(), palantírReveal)
           .Complete();
}

public class StringBearerReadOnlySpanAlwaysAddAllStringBearer<TBearer>(TBearer[]? value) : IStringBearer
    where TBearer : IStringBearer
{
    public TBearer[]? AlwaysAddAllStringBearerReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(AlwaysAddAllStringBearerReadOnlySpan), (ReadOnlySpan<TBearer>)AlwaysAddAllStringBearerReadOnlySpan.AsSpan())
           .Complete();
}

public class StringBearerNullableRefReadOnlySpanAlwaysAddAllStringBearer<TBearer>(TBearer?[]? value) : IStringBearer
    where TBearer : class, IStringBearer
{
    public TBearer?[]? AlwaysAddAllStringBearerReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllNullable(nameof(AlwaysAddAllStringBearerReadOnlySpan), (ReadOnlySpan<TBearer?>)AlwaysAddAllStringBearerReadOnlySpan.AsSpan())
           .Complete();
}

public class NullableStringBearerReadOnlySpanAlwaysAddAllStringBearer<TBearerStruct>(TBearerStruct?[]? value) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? AlwaysAddAllNullableStringBearerReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(AlwaysAddAllNullableStringBearerReadOnlySpan), (ReadOnlySpan<TBearerStruct?>)AlwaysAddAllNullableStringBearerReadOnlySpan.AsSpan())
           .Complete();
}

public class StringReadOnlySpanAlwaysAddAllStringBearer(string[]? value) : IStringBearer
{
    public string[]? AlwaysAddAllStringReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllStringReadOnlySpan), (ReadOnlySpan<string>)AlwaysAddAllStringReadOnlySpan.AsSpan())
           .Complete();
}

public class StringNullableReadOnlySpanAlwaysAddAllStringBearer(string?[]? value) : IStringBearer
{
    public string?[]? AlwaysAddAllStringNullableReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllNullable(nameof(AlwaysAddAllStringNullableReadOnlySpan), (ReadOnlySpan<string?>)AlwaysAddAllStringNullableReadOnlySpan.AsSpan())
           .Complete();
}

public class CharSequenceReadOnlySpanAlwaysAddAllStringBearer<TCharSeq>(TCharSeq[]? value) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq[]? AlwaysAddAllCharSequenceReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeq(nameof(AlwaysAddAllCharSequenceReadOnlySpan), (ReadOnlySpan<TCharSeq>)AlwaysAddAllCharSequenceReadOnlySpan.AsSpan())
           .Complete();
}

public class CharSequenceNullableReadOnlySpanAlwaysAddAllStringBearer<TCharSeq>(TCharSeq?[]? value) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? AlwaysAddAllCharSequenceNullableSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeqNullable(nameof(AlwaysAddAllCharSequenceNullableSpan), (ReadOnlySpan<TCharSeq?>)AlwaysAddAllCharSequenceNullableSpan.AsSpan())
           .Complete();
}

public class StringBuilderReadOnlySpanAlwaysAddAllStringBearer(StringBuilder[] value) : IStringBearer
{
    public StringBuilder[] AlwaysAddAllStringBuilderReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllStringBuilderReadOnlySpan), (ReadOnlySpan<StringBuilder>)AlwaysAddAllStringBuilderReadOnlySpan.AsSpan())
           .Complete();
}

public class StringBuilderNullableReadOnlySpanAlwaysAddAllStringBearer(StringBuilder?[] value) : IStringBearer
{
    public StringBuilder?[] AlwaysAddAllStringBuilderNullableReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllNullable(nameof(AlwaysAddAllStringBuilderNullableReadOnlySpan), (ReadOnlySpan<StringBuilder?>)AlwaysAddAllStringBuilderNullableReadOnlySpan.AsSpan())
           .Complete();
}

public class MatchReadOnlySpanAlwaysAddAllStringBearer<T>(T[]? value) : IStringBearer
{
    public T[]? AlwaysAddAllMatchReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatch(nameof(AlwaysAddAllMatchReadOnlySpan), (ReadOnlySpan<T>)AlwaysAddAllMatchReadOnlySpan.AsSpan())
           .Complete();
}

public class MatchNullableReadOnlySpanAlwaysAddAllStringBearer<T>(T?[]? value) : IStringBearer
{
    public T?[]? AlwaysAddAllMatchNullableReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatch(nameof(AlwaysAddAllMatchNullableReadOnlySpan), (ReadOnlySpan<T?>)AlwaysAddAllMatchNullableReadOnlySpan.AsSpan())
           .Complete();
}

public class ObjectReadOnlySpanAlwaysAddAllStringBearer(object[]? value) : IStringBearer
{
    public object[]? AlwaysAddAllObjectSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObject(nameof(AlwaysAddAllObjectSpan), (ReadOnlySpan<object>)AlwaysAddAllObjectSpan.AsSpan())
           .Complete();
}

public class ObjectNullableReadOnlySpanAlwaysAddAllStringBearer(object?[]? value) : IStringBearer
{
    public object?[]? AlwaysAddAllObjectNullableReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObjectNullable(nameof(AlwaysAddAllObjectNullableReadOnlySpan), (ReadOnlySpan<object?>)AlwaysAddAllObjectNullableReadOnlySpan.AsSpan())
           .Complete();
}

public class BoolArrayAlwaysAddAllStringBearer(bool[]? value) : IStringBearer
{
    public bool[]? AlwaysAddAllBoolArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllBoolArray), AlwaysAddAllBoolArray)
           .Complete();
}

public class NullableBoolArrayAlwaysAddAllStringBearer(bool?[]? value) : IStringBearer
{
    public bool?[]? AlwaysAddAllNullableBoolArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllNullableBoolArray), AlwaysAddAllNullableBoolArray)
           .Complete();
}

public class SpanFormattableArrayAlwaysAddAllStringBearer<TFmt>(TFmt[]? value) : IStringBearer
    where TFmt : ISpanFormattable
{
    public TFmt?[]? AlwaysAddAllSpanFormattableArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllSpanFormattableArray), AlwaysAddAllSpanFormattableArray)
           .Complete();
}

public class SpanFormattableNullableArrayAlwaysAddAllStringBearer<TFmt>(TFmt?[]? value) : IStringBearer
    where TFmt : ISpanFormattable
{
    public TFmt?[]? AlwaysAddAllSpanFormattableArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllSpanFormattableArray), AlwaysAddAllSpanFormattableArray)
           .Complete();
}

public class IpAddressArrayAlwaysAddAllStringBearer(IPAddress?[]? value) : IStringBearer
{
    public IPAddress?[]? AlwaysAddAllSpanFormattableArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllSpanFormattableArray), AlwaysAddAllSpanFormattableArray)
           .Complete();
}

public class NullableSpanFormattableArrayAlwaysAddAllStringBearer<TFmtStruct>(TFmtStruct?[]? value) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? AlwaysAddAllNullableSpanFormattableArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllNullableSpanFormattableArray), AlwaysAddAllNullableSpanFormattableArray)
           .Complete();
}

public class CustomBearerArrayAlwaysAddAllStringBearer<TCloaked, TCloakedBase>(
    TCloaked[]? value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedBase
{
    public TCloaked[]? AlwaysAddAllCustomBearerArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(AlwaysAddAllCustomBearerArray), AlwaysAddAllCustomBearerArray, palantírReveal)
           .Complete();
}

public class NullableCustomBearerArrayAlwaysAddAllStringBearer<TCloakedStruct>(
    TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? AlwaysAddAllNullableCustomBearerArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(AlwaysAddAllNullableCustomBearerArray), AlwaysAddAllNullableCustomBearerArray, palantírReveal)
           .Complete();
}

public class StringBearerArrayAlwaysAddAllStringBearer<TBearer>(TBearer[]? value) : IStringBearer
    where TBearer : IStringBearer
{
    public TBearer[]? AlwaysAddAllStringBearerArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(AlwaysAddAllStringBearerArray), AlwaysAddAllStringBearerArray)
           .Complete();
}

public class NullableStringBearerArrayAlwaysAddAllStringBearer<TBearerStruct>(TBearerStruct?[]? value) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? AlwaysAddAllNullableStringBearerArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(AlwaysAddAllNullableStringBearerArray), AlwaysAddAllNullableStringBearerArray)
           .Complete();
}

public class StringArrayAlwaysAddAllStringBearer(string?[]? value) : IStringBearer
{
    public string?[]? AlwaysAddAllStringArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllStringArray), AlwaysAddAllStringArray)
           .Complete();
}

public class CharSequenceArrayAlwaysAddAllStringBearer<TCharSeq>(TCharSeq?[]? value) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? AlwaysAddAllCharSequenceArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeq(nameof(AlwaysAddAllCharSequenceArray), AlwaysAddAllCharSequenceArray)
           .Complete();
}

public class StringBuilderArrayAlwaysAddAllStringBearer(StringBuilder?[]? value) : IStringBearer
{
    public StringBuilder?[]? AlwaysAddAllStringBuilderArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllStringBuilderArray), AlwaysAddAllStringBuilderArray)
           .Complete();
}

public class MatchArrayAlwaysAddAllStringBearer<T>(T[]? value) : IStringBearer
{
    public T[]? AlwaysAddAllMatchArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatch(nameof(AlwaysAddAllMatchArray), AlwaysAddAllMatchArray)
           .Complete();
}

public class ObjectArrayAlwaysAddAllStringBearer(object?[]? value) : IStringBearer
{
    public object?[]? AlwaysAddAllNullableObjectArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObject(nameof(AlwaysAddAllNullableObjectArray), AlwaysAddAllNullableObjectArray)
           .Complete();
}

public class BoolListAlwaysAddAllStringBearer(List<bool>? value) : IStringBearer
{
    public IReadOnlyList<bool>? AlwaysAddAllBoolList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllBoolList), AlwaysAddAllBoolList)
           .Complete();
}

public class NullableBoolListAlwaysAddAllStringBearer(List<bool?>? value) : IStringBearer
{
    public IReadOnlyList<bool?>? AlwaysAddAllNullableBoolList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllNullableBoolList), AlwaysAddAllNullableBoolList)
           .Complete();
}

public class SpanFormattableListAlwaysAddAllStringBearer<TFmt>(List<TFmt>? value) : IStringBearer
    where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt>? AlwaysAddAllSpanFormattableList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllSpanFormattableList), AlwaysAddAllSpanFormattableList)
           .Complete();
}

public class NullableSpanFormattableListAlwaysAddAllStringBearer<TFmtStruct>(List<TFmtStruct?>? value) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? AlwaysAddAllNullableSpanFormattableList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllNullableSpanFormattableList), AlwaysAddAllNullableSpanFormattableList)
           .Complete();
}

public class CustomBearerListAlwaysAddAllStringBearer<TCloaked, TCloakedBase>(
    List<TCloaked>? value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedBase
{
    public IReadOnlyList<TCloaked>? AlwaysAddAllCustomBearerList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(AlwaysAddAllCustomBearerList), AlwaysAddAllCustomBearerList, palantírReveal)
           .Complete();
}

public class NullableCustomBearerListAlwaysAddAllStringBearer<TCloakedStruct>(
    List<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? AlwaysAddAllNullableCustomBearerList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(AlwaysAddAllNullableCustomBearerList), AlwaysAddAllNullableCustomBearerList, palantírReveal)
           .Complete();
}

public class StringBearerListAlwaysAddAllStringBearer<TBearer>(List<TBearer>? value) : IStringBearer
    where TBearer : IStringBearer
{
    public IReadOnlyList<TBearer>? AlwaysAddAllStringBearerList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(AlwaysAddAllStringBearerList), AlwaysAddAllStringBearerList)
           .Complete();
}

public class NullableStringBearerListAlwaysAddAllStringBearer<TBearerStruct>(List<TBearerStruct?>? value) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? AlwaysAddAllNullableStringBearerList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(AlwaysAddAllNullableStringBearerList), AlwaysAddAllNullableStringBearerList)
           .Complete();
}

public class StringListAlwaysAddAllStringBearer(List<string?>? value) : IStringBearer
{
    public IReadOnlyList<string?>? AlwaysAddAllStringList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllStringList), AlwaysAddAllStringList)
           .Complete();
}

public class CharSequenceListAlwaysAddAllStringBearer<TCharSeq>(List<TCharSeq?>? value) : IStringBearer
    where TCharSeq : ICharSequence
{
    public IReadOnlyList<TCharSeq?>? AlwaysAddAllCharSequenceList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllCharSeq(nameof(AlwaysAddAllCharSequenceList), AlwaysAddAllCharSequenceList)
           .Complete();
}

public class StringBuilderListAlwaysAddAllStringBearer(List<StringBuilder?>? value) : IStringBearer
{
    public IReadOnlyList<StringBuilder?>? AlwaysAddAllStringBuilderList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAll(nameof(AlwaysAddAllStringBuilderList), AlwaysAddAllStringBuilderList)
           .Complete();
}

public class MatchListAlwaysAddAllStringBearer<T>(List<T>? value) : IStringBearer
{
    public IReadOnlyList<T>? AlwaysAddAllMatchList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllMatch(nameof(AlwaysAddAllMatchList), AlwaysAddAllMatchList)
           .Complete();
}

public class ObjectListAlwaysAddAllStringBearer<T>(List<T>? value) : IStringBearer
    where T : class
{
    public IReadOnlyList<T>? AlwaysAddAllObjectList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObject(nameof(AlwaysAddAllObjectList), AlwaysAddAllObjectList)
           .Complete();
}