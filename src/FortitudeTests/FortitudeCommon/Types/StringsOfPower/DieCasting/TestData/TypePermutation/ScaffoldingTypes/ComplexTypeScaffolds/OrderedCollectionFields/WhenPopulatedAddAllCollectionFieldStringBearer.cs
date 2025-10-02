// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Net;
using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.OrderedCollectionFields;

public class BoolSpanWhenPopulatedAddAllStringBearer(bool[] value) : IStringBearer
{
    public bool[] WhenPopulatedAddAllBoolSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllBoolSpan), WhenPopulatedAddAllBoolSpan.AsSpan())
           .Complete();
}

public class NullableBoolSpanWhenPopulatedAddAllStringBearer(bool?[] value) : IStringBearer
{
    public bool?[] WhenPopulatedAddAllNullableBoolSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllNullableBoolSpan), WhenPopulatedAddAllNullableBoolSpan.AsSpan())
           .Complete();
}

public class SpanFormattableSpanWhenPopulatedAddAllStringBearer<TFmt>(TFmt[] value) : IStringBearer
    where TFmt : ISpanFormattable
{
    public TFmt[] WhenPopulatedAddAllSpanFormattableStructSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllSpanFormattableStructSpan)
                                              , WhenPopulatedAddAllSpanFormattableStructSpan.AsSpan())
           .Complete();
}

public class SpanFormattableNullableClassSpanWhenPopulatedAddAllStringBearer<TFmt>(TFmt?[] value) : IStringBearer
    where TFmt : class, ISpanFormattable
{
    public TFmt?[] WhenPopulatedAddAllSpanFormattableNullableClassSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable(nameof(WhenPopulatedAddAllSpanFormattableNullableClassSpan)
                                                      , WhenPopulatedAddAllSpanFormattableNullableClassSpan.AsSpan())
           .Complete();
}

public class SpanFormattableNullableStructSpanWhenPopulatedAddAllStringBearer<TFmtStruct>(TFmtStruct?[]? value) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? WhenPopulatedAddAllSpanFormattableSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllSpanFormattableSpan), WhenPopulatedAddAllSpanFormattableSpan)
           .Complete();
}

public class IpAddressSpanWhenPopulatedAddAllStringBearer(IPAddress?[]? value) : IStringBearer
{
    public IPAddress?[]? WhenPopulatedAddAllIpAddressSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable(nameof(WhenPopulatedAddAllIpAddressSpan), WhenPopulatedAddAllIpAddressSpan.AsSpan())
           .Complete();
}

public class NullableSpanFormattableSpanWhenPopulatedAddAllStringBearer<TFmtStruct>(TFmtStruct?[]? value) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? WhenPopulatedAddAllNullableSpanFormattableSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllNullableSpanFormattableSpan)
                                              , WhenPopulatedAddAllNullableSpanFormattableSpan.AsSpan())
           .Complete();
}

public class CloakedBearerSpanWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase>
(
    TCloaked[] value
  , PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedBase
{
    public TCloaked[] WhenPopulatedAddAllCloakedBearerSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(WhenPopulatedAddAllCloakedBearerSpan), WhenPopulatedAddAllCloakedBearerSpan.AsSpan()
                                          , palantírReveal)
           .Complete();
}

public class CloakedBearerNullableRefSpanWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase>
(
    TCloaked?[] value
  , PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : class, TCloakedBase
{
    public TCloaked?[] WhenPopulatedAddAllCloakedBearerNullableSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllNullable(nameof(WhenPopulatedAddAllCloakedBearerNullableSpan)
                                                  , WhenPopulatedAddAllCloakedBearerNullableSpan.AsSpan(), palantírReveal)
           .Complete();
}

public class NullableCustomBearerSpanWhenPopulatedAddAllStringBearer<TCloakedStruct>
(
    TCloakedStruct?[]? value
  , PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? WhenPopulatedAddAllNullableCustomBearerSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(WhenPopulatedAddAllNullableCustomBearerSpan), WhenPopulatedAddAllNullableCustomBearerSpan.AsSpan()
                                          , palantírReveal)
           .Complete();
}

public class StringBearerSpanWhenPopulatedAddAllStringBearer<TBearer>(TBearer[]? value) : IStringBearer
    where TBearer : IStringBearer
{
    public TBearer[]? WhenPopulatedAddAllStringBearerSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(WhenPopulatedAddAllStringBearerSpan), WhenPopulatedAddAllStringBearerSpan.AsSpan())
           .Complete();
}

public class StringBearerNullableRefSpanWhenPopulatedAddAllStringBearer<TBearer>(TBearer?[]? value) : IStringBearer
    where TBearer : class, IStringBearer
{
    public TBearer?[]? WhenPopulatedAddAllStringBearerSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllNullable(nameof(WhenPopulatedAddAllStringBearerSpan), WhenPopulatedAddAllStringBearerSpan.AsSpan())
           .Complete();
}

public class NullableStringBearerSpanWhenPopulatedAddAllStringBearer<TBearerStruct>(TBearerStruct?[]? value) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? WhenPopulatedAddAllNullableStringBearerSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(WhenPopulatedAddAllNullableStringBearerSpan), WhenPopulatedAddAllNullableStringBearerSpan.AsSpan())
           .Complete();
}

public class StringSpanWhenPopulatedAddAllStringBearer(string[]? value) : IStringBearer
{
    public string[]? WhenPopulatedAddAllStringSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllStringSpan), WhenPopulatedAddAllStringSpan.AsSpan())
           .Complete();
}

public class StringNullableSpanWhenPopulatedAddAllStringBearer(string?[]? value) : IStringBearer
{
    public string?[]? WhenPopulatedAddAllStringSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable(nameof(WhenPopulatedAddAllStringSpan), WhenPopulatedAddAllStringSpan.AsSpan())
           .Complete();
}

public class CharSequenceSpanWhenPopulatedAddAllStringBearer<TCharSeq>(TCharSeq[]? value) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq[]? WhenPopulatedAddAllCharSequenceSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeq(nameof(WhenPopulatedAddAllCharSequenceSpan), WhenPopulatedAddAllCharSequenceSpan.AsSpan())
           .Complete();
}

public class CharSequenceNullableSpanWhenPopulatedAddAllStringBearer<TCharSeq>(TCharSeq?[]? value) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? WhenPopulatedAddAllCharSequenceNullableSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeqNullable(nameof(WhenPopulatedAddAllCharSequenceNullableSpan)
                                                             , WhenPopulatedAddAllCharSequenceNullableSpan.AsSpan())
           .Complete();
}

public class StringBuilderSpanWhenPopulatedAddAllStringBearer(StringBuilder[] value) : IStringBearer
{
    public StringBuilder[] WhenPopulatedAddAllStringBuilderSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllStringBuilderSpan), WhenPopulatedAddAllStringBuilderSpan.AsSpan())
           .Complete();
}

public class StringBuilderNullableSpanWhenPopulatedAddAllStringBearer(StringBuilder?[] value) : IStringBearer
{
    public StringBuilder?[] WhenPopulatedAddAllStringBuilderSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable(nameof(WhenPopulatedAddAllStringBuilderSpan), WhenPopulatedAddAllStringBuilderSpan.AsSpan())
           .Complete();
}

public class MatchSpanWhenPopulatedAddAllStringBearer<T>(T[]? value) : IStringBearer
{
    public T[]? WhenPopulatedAddAllMatchSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch(nameof(WhenPopulatedAddAllMatchSpan), WhenPopulatedAddAllMatchSpan.AsSpan())
           .Complete();
}

public class MatchNullableSpanWhenPopulatedAddAllStringBearer<T>(T?[]? value) : IStringBearer
{
    public T?[]? WhenPopulatedAddAllMatchNullableSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch(nameof(WhenPopulatedAddAllMatchNullableSpan), WhenPopulatedAddAllMatchNullableSpan.AsSpan())
           .Complete();
}

public class ObjectSpanWhenPopulatedAddAllStringBearer(object[]? value) : IStringBearer
{
    public object[]? WhenPopulatedAddAllObjectSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObject(nameof(WhenPopulatedAddAllObjectSpan), WhenPopulatedAddAllObjectSpan.AsSpan())
           .Complete();
}

public class ObjectNullableSpanWhenPopulatedAddAllStringBearer(object?[]? value) : IStringBearer
{
    public object?[]? WhenPopulatedAddAllObjectNullableRefSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObjectNullable(nameof(WhenPopulatedAddAllObjectNullableRefSpan)
                                                            , WhenPopulatedAddAllObjectNullableRefSpan.AsSpan())
           .Complete();
}

public class BoolReadOnlySpanWhenPopulatedAddAllStringBearer(bool[] value) : IStringBearer
{
    public bool[] WhenPopulatedAddAllBoolReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllBoolReadOnlySpan)
                                              , (ReadOnlySpan<bool>)WhenPopulatedAddAllBoolReadOnlySpan.AsSpan())
           .Complete();
}

public class NullableBoolReadOnlySpanWhenPopulatedAddAllStringBearer(bool?[] value) : IStringBearer
{
    public bool?[] WhenPopulatedAddAllNullableBoolReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllNullableBoolReadOnlySpan)
                                              , (ReadOnlySpan<bool?>)WhenPopulatedAddAllNullableBoolReadOnlySpan.AsSpan())
           .Complete();
}

public class SpanFormattableReadOnlySpanWhenPopulatedAddAllStringBearer<TFmt>(TFmt[] value) : IStringBearer
    where TFmt : ISpanFormattable
{
    public TFmt[] WhenPopulatedAddAllSpanFormattableReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllSpanFormattableReadOnlySpan)
                                              , (ReadOnlySpan<TFmt>)WhenPopulatedAddAllSpanFormattableReadOnlySpan.AsSpan())
           .Complete();
}

public class SpanFormattableNullableRefReadOnlySpanWhenPopulatedAddAllStringBearer<TFmt>(TFmt?[] value) : IStringBearer
    where TFmt : class, ISpanFormattable
{
    public TFmt?[] WhenPopulatedAddAllSpanFormattableNullableRefReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable(nameof(WhenPopulatedAddAllSpanFormattableNullableRefReadOnlySpan)
                                                      , (ReadOnlySpan<TFmt?>)WhenPopulatedAddAllSpanFormattableNullableRefReadOnlySpan.AsSpan())
           .Complete();
}

public class SpanFormattableNullableStructReadOnlySpanWhenPopulatedAddAllStringBearer<TFmtStruct>(TFmtStruct?[]? value) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? WhenPopulatedAddAllSpanFormattableReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllSpanFormattableReadOnlySpan)
                                              , (ReadOnlySpan<TFmtStruct?>)WhenPopulatedAddAllSpanFormattableReadOnlySpan.AsSpan())
           .Complete();
}

public class IpAddressReadOnlySpanWhenPopulatedAddAllStringBearer(IPAddress?[]? value) : IStringBearer
{
    public IPAddress?[]? WhenPopulatedAddAllIpAddressReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable(nameof(WhenPopulatedAddAllIpAddressReadOnlySpan)
                                                      , (ReadOnlySpan<IPAddress?>)WhenPopulatedAddAllIpAddressReadOnlySpan.AsSpan())
           .Complete();
}

public class NullableReadOnlySpanFormattableSpanWhenPopulatedAddAllStringBearer<TFmtStruct>(TFmtStruct?[]? value) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? WhenPopulatedAddAllNullableSpanFormattableReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllNullableSpanFormattableReadOnlySpan)
                                              , (ReadOnlySpan<TFmtStruct?>)WhenPopulatedAddAllNullableSpanFormattableReadOnlySpan.AsSpan())
           .Complete();
}

public class CloakedBearerReadOnlySpanWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase>
(
    TCloaked[] value
  , PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedBase
{
    public TCloaked[] WhenPopulatedAddAllCloakedBearerReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(WhenPopulatedAddAllCloakedBearerReadOnlySpan)
                                          , (ReadOnlySpan<TCloaked>)WhenPopulatedAddAllCloakedBearerReadOnlySpan.AsSpan(), palantírReveal)
           .Complete();
}

public class CloakedBearerNullableRefReadOnlySpanWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase>
(
    TCloaked?[] value
  , PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : class, TCloakedBase
{
    public TCloaked?[] WhenPopulatedAddAllCloakedBearerNullableSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllNullable(nameof(WhenPopulatedAddAllCloakedBearerNullableSpan)
                                                  , (ReadOnlySpan<TCloaked?>)WhenPopulatedAddAllCloakedBearerNullableSpan.AsSpan(), palantírReveal)
           .Complete();
}

public class NullableCustomBearerReadOnlySpanWhenPopulatedAddAllStringBearer<TCloakedStruct>
(
    TCloakedStruct?[]? value
  , PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? WhenPopulatedAddAllNullableCustomBearerReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(WhenPopulatedAddAllNullableCustomBearerReadOnlySpan)
                                          , (ReadOnlySpan<TCloakedStruct?>)WhenPopulatedAddAllNullableCustomBearerReadOnlySpan.AsSpan()
                                          , palantírReveal)
           .Complete();
}

public class StringBearerReadOnlySpanWhenPopulatedAddAllStringBearer<TBearer>(TBearer[]? value) : IStringBearer
    where TBearer : IStringBearer
{
    public TBearer[]? WhenPopulatedAddAllStringBearerReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(WhenPopulatedAddAllStringBearerReadOnlySpan)
                                          , (ReadOnlySpan<TBearer>)WhenPopulatedAddAllStringBearerReadOnlySpan.AsSpan())
           .Complete();
}

public class StringBearerNullableRefReadOnlySpanWhenPopulatedAddAllStringBearer<TBearer>(TBearer?[]? value) : IStringBearer
    where TBearer : class, IStringBearer
{
    public TBearer?[]? WhenPopulatedAddAllStringBearerReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAllNullable(nameof(WhenPopulatedAddAllStringBearerReadOnlySpan)
                                                  , (ReadOnlySpan<TBearer?>)WhenPopulatedAddAllStringBearerReadOnlySpan.AsSpan())
           .Complete();
}

public class NullableStringBearerReadOnlySpanWhenPopulatedAddAllStringBearer<TBearerStruct>(TBearerStruct?[]? value) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? WhenPopulatedAddAllNullableStringBearerReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealAll(nameof(WhenPopulatedAddAllNullableStringBearerReadOnlySpan)
                                          , (ReadOnlySpan<TBearerStruct?>)WhenPopulatedAddAllNullableStringBearerReadOnlySpan.AsSpan())
           .Complete();
}

public class StringReadOnlySpanWhenPopulatedAddAllStringBearer(string[]? value) : IStringBearer
{
    public string[]? WhenPopulatedAddAllStringReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllStringReadOnlySpan)
                                              , (ReadOnlySpan<string>)WhenPopulatedAddAllStringReadOnlySpan.AsSpan())
           .Complete();
}

public class StringNullableReadOnlySpanWhenPopulatedAddAllStringBearer(string?[]? value) : IStringBearer
{
    public string?[]? WhenPopulatedAddAllStringNullableReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable(nameof(WhenPopulatedAddAllStringNullableReadOnlySpan)
                                                      , (ReadOnlySpan<string?>)WhenPopulatedAddAllStringNullableReadOnlySpan.AsSpan())
           .Complete();
}

public class CharSequenceReadOnlySpanWhenPopulatedAddAllStringBearer<TCharSeq>(TCharSeq[]? value) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq[]? WhenPopulatedAddAllCharSequenceReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeq(nameof(WhenPopulatedAddAllCharSequenceReadOnlySpan)
                                                     , (ReadOnlySpan<TCharSeq>)WhenPopulatedAddAllCharSequenceReadOnlySpan.AsSpan())
           .Complete();
}

public class CharSequenceNullableReadOnlySpanWhenPopulatedAddAllStringBearer<TCharSeq>(TCharSeq?[]? value) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? WhenPopulatedAddAllCharSequenceNullableSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeqNullable(nameof(WhenPopulatedAddAllCharSequenceNullableSpan)
                                                             , (ReadOnlySpan<TCharSeq?>)WhenPopulatedAddAllCharSequenceNullableSpan.AsSpan())
           .Complete();
}

public class StringBuilderReadOnlySpanWhenPopulatedAddAllStringBearer(StringBuilder[] value) : IStringBearer
{
    public StringBuilder[] WhenPopulatedAddAllStringBuilderReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllStringBuilderReadOnlySpan)
                                              , (ReadOnlySpan<StringBuilder>)WhenPopulatedAddAllStringBuilderReadOnlySpan.AsSpan())
           .Complete();
}

public class StringBuilderNullableReadOnlySpanWhenPopulatedAddAllStringBearer(StringBuilder?[] value) : IStringBearer
{
    public StringBuilder?[] WhenPopulatedAddAllStringBuilderNullableReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllNullable(nameof(WhenPopulatedAddAllStringBuilderNullableReadOnlySpan)
                                                      , (ReadOnlySpan<StringBuilder?>)WhenPopulatedAddAllStringBuilderNullableReadOnlySpan.AsSpan())
           .Complete();
}

public class MatchReadOnlySpanWhenPopulatedAddAllStringBearer<T>(T[]? value) : IStringBearer
{
    public T[]? WhenPopulatedAddAllMatchReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch(nameof(WhenPopulatedAddAllMatchReadOnlySpan)
                                                   , (ReadOnlySpan<T>)WhenPopulatedAddAllMatchReadOnlySpan.AsSpan())
           .Complete();
}

public class MatchNullableReadOnlySpanWhenPopulatedAddAllStringBearer<T>(T?[]? value) : IStringBearer
{
    public T?[]? WhenPopulatedAddAllMatchNullableReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch(nameof(WhenPopulatedAddAllMatchNullableReadOnlySpan)
                                                   , (ReadOnlySpan<T?>)WhenPopulatedAddAllMatchNullableReadOnlySpan.AsSpan())
           .Complete();
}

public class ObjectReadOnlySpanWhenPopulatedAddAllStringBearer(object[]? value) : IStringBearer
{
    public object[]? WhenPopulatedAddAllObjectSpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObject(nameof(WhenPopulatedAddAllObjectSpan)
                                                    , (ReadOnlySpan<object>)WhenPopulatedAddAllObjectSpan.AsSpan())
           .Complete();
}

public class ObjectNullableReadOnlySpanWhenPopulatedAddAllStringBearer(object?[]? value) : IStringBearer
{
    public object?[]? WhenPopulatedAddAllObjectNullableReadOnlySpan { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObjectNullable(nameof(WhenPopulatedAddAllObjectNullableReadOnlySpan)
                                                            , (ReadOnlySpan<object?>)WhenPopulatedAddAllObjectNullableReadOnlySpan.AsSpan())
           .Complete();
}

public class BoolArrayWhenPopulatedAddAllStringBearer(bool[]? value) : IStringBearer
{
    public bool[]? WhenPopulatedAddAllBoolArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllBoolArray), WhenPopulatedAddAllBoolArray)
           .Complete();
}

public class NullableBoolArrayWhenPopulatedAddAllStringBearer(bool?[]? value) : IStringBearer
{
    public bool?[]? WhenPopulatedAddAllNullableBoolArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllNullableBoolArray), WhenPopulatedAddAllNullableBoolArray)
           .Complete();
}

public class SpanFormattableArrayWhenPopulatedAddAllStringBearer<TFmt>(TFmt[]? value) : IStringBearer
    where TFmt : ISpanFormattable
{
    public TFmt[]? WhenPopulatedAddAllSpanFormattableArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllSpanFormattableArray), WhenPopulatedAddAllSpanFormattableArray)
           .Complete();
}

public class NullableSpanFormattableArrayWhenPopulatedAddAllStringBearer<TStructFmt>(TStructFmt?[]? value) : IStringBearer
    where TStructFmt : struct, ISpanFormattable
{
    public TStructFmt?[]? WhenPopulatedAddAllNullableSpanFormattableArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllNullableSpanFormattableArray)
                                              , WhenPopulatedAddAllNullableSpanFormattableArray)
           .Complete();
}

public class CustomBearerArrayWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase>
(
    TCloaked[]? value
  , PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedBase
{
    public TCloaked[]? WhenPopulatedAddAllCustomBearerArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll(nameof(WhenPopulatedAddAllCustomBearerArray), WhenPopulatedAddAllCustomBearerArray, palantírReveal)
           .Complete();
}

public class NullableCustomBearerArrayWhenPopulatedAddAllStringBearer<TCloakedStruct>
(
    TCloakedStruct?[]? value
  , PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? WhenPopulatedAddAllNullableCustomBearerArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll(nameof(WhenPopulatedAddAllNullableCustomBearerArray), WhenPopulatedAddAllNullableCustomBearerArray
                                                 , palantírReveal)
           .Complete();
}

public class StringBearerArrayWhenPopulatedAddAllStringBearer<TBearer>(TBearer[]? value) : IStringBearer
    where TBearer : IStringBearer
{
    public TBearer[]? WhenPopulatedAddAllStringBearerArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll(nameof(WhenPopulatedAddAllStringBearerArray), WhenPopulatedAddAllStringBearerArray)
           .Complete();
}

public class NullableStringBearerArrayWhenPopulatedAddAllStringBearer<TBearerStruct>(TBearerStruct?[]? value) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? WhenPopulatedAddAllNullableStringBearerArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll(nameof(WhenPopulatedAddAllNullableStringBearerArray), WhenPopulatedAddAllNullableStringBearerArray)
           .Complete();
}

public class StringArrayWhenPopulatedAddAllStringBearer(string?[]? value) : IStringBearer
{
    public string?[]? WhenPopulatedAddAllStringArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllStringArray), WhenPopulatedAddAllStringArray)
           .Complete();
}

public class CharSequenceArrayWhenPopulatedAddAllStringBearer<TCharSeq>(TCharSeq?[]? value) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? WhenPopulatedAddAllCharSequenceArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeq(nameof(WhenPopulatedAddAllCharSequenceArray), WhenPopulatedAddAllCharSequenceArray)
           .Complete();
}

public class StringBuilderArrayWhenPopulatedAddAllStringBearer(StringBuilder?[]? value) : IStringBearer
{
    public StringBuilder?[]? WhenPopulatedAddAllStringBuilderArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllStringBuilderArray), WhenPopulatedAddAllStringBuilderArray)
           .Complete();
}

public class MatchArrayWhenPopulatedAddAllStringBearer<T>(T[]? value) : IStringBearer
{
    public T[]? WhenPopulatedAddAllMatchArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch(nameof(WhenPopulatedAddAllMatchArray), WhenPopulatedAddAllMatchArray)
           .Complete();
}

public class ObjectArrayWhenPopulatedAddFilteredStringBearer(object?[]? value) : IStringBearer
{
    public object?[]? WhenPopulatedAddObjectArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObject(nameof(WhenPopulatedAddObjectArray), WhenPopulatedAddObjectArray);
}

public class BoolListWhenPopulatedAddAllStringBearer(List<bool>? value) : IStringBearer
{
    public IReadOnlyList<bool>? WhenPopulatedAddAllBoolList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllBoolList), WhenPopulatedAddAllBoolList)
           .Complete();
}

public class NullableBoolListWhenPopulatedAddAllStringBearer(List<bool?>? value) : IStringBearer
{
    public IReadOnlyList<bool?>? WhenPopulatedAddAllNullableBoolList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllNullableBoolList), WhenPopulatedAddAllNullableBoolList)
           .Complete();
}

public class SpanFormattableListWhenPopulatedAddAllStringBearer<TFmt>(List<TFmt>? value) : IStringBearer
    where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt>? WhenPopulatedAddAllSpanFormattableList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllSpanFormattableList), WhenPopulatedAddAllSpanFormattableList)
           .Complete();
}

public class NullableSpanFormattableListWhenPopulatedAddAllStringBearer<TStructFmt>(List<TStructFmt?>? value) : IStringBearer
    where TStructFmt : struct, ISpanFormattable
{
    public IReadOnlyList<TStructFmt?>? WhenPopulatedAddAllNullableSpanFormattableList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllNullableSpanFormattableList)
                                              , WhenPopulatedAddAllNullableSpanFormattableList)
           .Complete();
}

public class CustomBearerListWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase>
(
    List<TCloaked>? value
  , PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedBase
{
    public IReadOnlyList<TCloaked>? WhenPopulatedAddAllCustomBearerList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll(nameof(WhenPopulatedAddAllCustomBearerList), WhenPopulatedAddAllCustomBearerList, palantírReveal)
           .Complete();
}

public class NullableCustomBearerListWhenPopulatedAddAllStringBearer<TCloakedStruct>
(
    List<TCloakedStruct?>? value
  , PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? WhenPopulatedAddAllNullableCustomBearerList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll(nameof(WhenPopulatedAddAllNullableCustomBearerList), WhenPopulatedAddAllNullableCustomBearerList
                                                 , palantírReveal)
           .Complete();
}

public class StringBearerListWhenPopulatedAddAllStringBearer<TBearer>(List<TBearer>? value) : IStringBearer
    where TBearer : IStringBearer
{
    public IReadOnlyList<TBearer>? WhenPopulatedAddAllStringBearerList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll(nameof(WhenPopulatedAddAllStringBearerList), WhenPopulatedAddAllStringBearerList)
           .Complete();
}

public class NullableStringBearerListWhenPopulatedAddAllStringBearer<TBearerStruct>(List<TBearerStruct?>? value) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? WhenPopulatedAddAllNullableStringBearerList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll(nameof(WhenPopulatedAddAllNullableStringBearerList), WhenPopulatedAddAllNullableStringBearerList)
           .Complete();
}

public class StringListWhenPopulatedAddAllStringBearer(List<string?>? value) : IStringBearer
{
    public IReadOnlyList<string?>? WhenPopulatedAddAllStringList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllStringList), WhenPopulatedAddAllStringList)
           .Complete();
}

public class CharSequenceListWhenPopulatedAddAllStringBearer<TCharSeq>(List<TCharSeq?>? value) : IStringBearer
    where TCharSeq : ICharSequence
{
    public IReadOnlyList<TCharSeq?>? WhenPopulatedAddAllCharSequenceList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllCharSeq(nameof(WhenPopulatedAddAllCharSequenceList), WhenPopulatedAddAllCharSequenceList)
           .Complete();
}

public class StringBuilderListWhenPopulatedAddAllStringBearer(List<StringBuilder?>? value) : IStringBearer
{
    public IReadOnlyList<StringBuilder?>? WhenPopulatedAddAllStringBuilderList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllStringBuilderList), WhenPopulatedAddAllStringBuilderList)
           .Complete();
}

public class MatchListWhenPopulatedAddAllStringBearer<T>(List<T>? value) : IStringBearer
{
    public IReadOnlyList<T>? WhenPopulatedAddAllMatchList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllMatch(nameof(WhenPopulatedAddAllMatchList), WhenPopulatedAddAllMatchList)
           .Complete();
}

public class ObjectListWhenPopulatedAddFilteredStringBearer(List<object?>? value) : IStringBearer
{
    public IReadOnlyList<object?>? WhenPopulatedAddObjectList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObject(nameof(WhenPopulatedAddObjectList), WhenPopulatedAddObjectList);
}
