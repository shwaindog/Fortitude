using System.Net;
using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexFields.OrderedCollections;

public class BoolSpanAlwaysAddFilteredStringBearer(bool[]? value, OrderedCollectionPredicate<bool> filterPredicate) : IStringBearer
{
    public bool[]? AlwaysAddFilteredBoolSpan { get; } = value;

    public OrderedCollectionPredicate<bool> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredBoolSpan), AlwaysAddFilteredBoolSpan.AsSpan(), Filter)
           .Complete();
}

public class NullableBoolSpanAlwaysAddFilteredStringBearer(bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate) : IStringBearer
{
    public bool?[]? AlwaysAddFilteredNullableBoolSpan { get; } = value;

    public OrderedCollectionPredicate<bool?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredNullableBoolSpan), AlwaysAddFilteredNullableBoolSpan.AsSpan(), Filter)
           .Complete();
}

public class SpanFormattableSpanAlwaysAddFilteredStringBearer<TFmt>(TFmt[]? value, OrderedCollectionPredicate<TFmt> filterPredicate) : IStringBearer
    where TFmt : struct, ISpanFormattable
{
    public TFmt[]? AlwaysAddFilteredSpanFormattableSpan { get; } = value;

    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredSpanFormattableSpan), AlwaysAddFilteredSpanFormattableSpan.AsSpan(), Filter)
           .Complete();
}

public class SpanFormattableNullableSpanAlwaysAddFilteredStringBearer<TFmt>
    (TFmt?[]? value, OrderedCollectionPredicate<TFmt> filterPredicate) : IStringBearer
    where TFmt : class, ISpanFormattable
{
    public TFmt?[]? AlwaysAddFilteredSpanFormattableNullableSpan { get; } = value;

    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredNullable(nameof(AlwaysAddFilteredSpanFormattableNullableSpan)
                                                    , AlwaysAddFilteredSpanFormattableNullableSpan.AsSpan(), Filter)
           .Complete();
}

public class NullableSpanFormattableSpanAlwaysAddFilteredStringBearer<TFmtStruct>
    (TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? AlwaysAddFilteredNullableSpanFormattableSpan { get; } = value;

    public OrderedCollectionPredicate<TFmtStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredNullableSpanFormattableSpan)
                                            , AlwaysAddFilteredNullableSpanFormattableSpan.AsSpan(), Filter)
           .Complete();
}

public class IpAddressSpanAlwaysAddFilteredStringBearer(IPAddress?[]? value, OrderedCollectionPredicate<IPAddress?> filterPredicate) : IStringBearer
{
    public IPAddress?[]? AlwaysAddAllIpAddressSpan { get; } = value;

    public OrderedCollectionPredicate<IPAddress?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredNullable(nameof(AlwaysAddAllIpAddressSpan), AlwaysAddAllIpAddressSpan.AsSpan(), Filter)
           .Complete();
}

public class CustomBearerSpanAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
(
    TCloaked[]? value
  , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
  , PalantírReveal<TCloakedRevealBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? AlwaysAddFilteredCloakedBearerSpan { get; } = value;

    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredCloakedBearerSpan), AlwaysAddFilteredCloakedBearerSpan.AsSpan(), Filter
                                               , palantírReveal)
           .Complete();
}

public class CustomBearerNullableSpanAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
(
    TCloaked?[]? value
  , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
  , PalantírReveal<TCloakedRevealBase> palantírReveal) : IStringBearer
    where TCloaked : class, TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked?[]? AlwaysAddFilteredCloakedBearerNullableSpan { get; } = value;

    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredNullable(nameof(AlwaysAddFilteredCloakedBearerNullableSpan)
                                                       , AlwaysAddFilteredCloakedBearerNullableSpan.AsSpan(), Filter, palantírReveal)
           .Complete();
}

public class NullableCustomBearerSpanAlwaysAddFilteredStringBearer<TCloakedStruct>
(
    TCloakedStruct?[]? value
  , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
  , PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? AlwaysAddFilteredNullableCloakedBearerSpan { get; } = value;

    public OrderedCollectionPredicate<TCloakedStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredNullableCloakedBearerSpan)
                                               , AlwaysAddFilteredNullableCloakedBearerSpan.AsSpan(), Filter, palantírReveal)
           .Complete();
}

public class StringBearerSpanAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    (TBearer[]? value, OrderedCollectionPredicate<TBearerBase> filterPredicate) : IStringBearer
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? AlwaysAddFilteredStringBearerSpan { get; } = value;

    public OrderedCollectionPredicate<TBearerBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredStringBearerSpan), AlwaysAddFilteredStringBearerSpan.AsSpan(), Filter)
           .Complete();
}

public class StringBearerNullableSpanAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    (TBearer?[]? value, OrderedCollectionPredicate<TBearerBase?> filterPredicate) : IStringBearer
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? AlwaysAddFilteredStringBearerNullableSpan { get; } = value;

    public OrderedCollectionPredicate<TBearerBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredNullable(nameof(AlwaysAddFilteredStringBearerNullableSpan)
                                                       , AlwaysAddFilteredStringBearerNullableSpan.AsSpan(), Filter)
           .Complete();
}

public class NullableStringBearerSpanAlwaysAddFilteredStringBearer<TBearerStruct>
    (TBearerStruct?[]? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? AlwaysAddFilteredNullableStringBearerSpan { get; } = value;

    public OrderedCollectionPredicate<TBearerStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredNullableStringBearerSpan), AlwaysAddFilteredNullableStringBearerSpan.AsSpan()
                                               , Filter)
           .Complete();
}

public class StringSpanAlwaysAddFilteredStringBearer(string[]? value, OrderedCollectionPredicate<string?> filterPredicate) : IStringBearer
{
    public string[]? AlwaysAddFilteredStringSpan { get; } = value;

    public OrderedCollectionPredicate<string?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredStringSpan), AlwaysAddFilteredStringSpan.AsSpan(), Filter)
           .Complete();
}

public class StringNullableSpanAlwaysAddFilteredStringBearer(string?[]? value, OrderedCollectionPredicate<string?> filterPredicate) : IStringBearer
{
    public string?[]? AlwaysAddFilteredNullableStringSpan { get; } = value;

    public OrderedCollectionPredicate<string?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .AlwaysAddFilteredNullable(nameof(AlwaysAddFilteredNullableStringSpan), AlwaysAddFilteredNullableStringSpan.AsSpan(), Filter)
           .Complete();
}

public class CharSequenceSpanAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    (TCharSeq[]? value, OrderedCollectionPredicate<TCharSeqFilterBase?> filterPredicate) : IStringBearer
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq[]? AlwaysAddFilteredCharSequenceSpan { get; } = value;

    public OrderedCollectionPredicate<TCharSeqFilterBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeq(nameof(AlwaysAddFilteredCharSequenceSpan), AlwaysAddFilteredCharSequenceSpan.AsSpan(), Filter)
           .Complete();
}

public class CharSequenceNullableSpanAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    (TCharSeq?[]? value, OrderedCollectionPredicate<TCharSeqFilterBase?> filterPredicate) : IStringBearer
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? AlwaysAddFilteredNullableCharSequenceSpan { get; } = value;

    public OrderedCollectionPredicate<TCharSeqFilterBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeqNullable(nameof(AlwaysAddFilteredNullableCharSequenceSpan)
                                                           , AlwaysAddFilteredNullableCharSequenceSpan.AsSpan(), Filter)
           .Complete();
}

public class StringBuilderSpanAlwaysAddFilteredStringBearer
    (StringBuilder[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate) : IStringBearer
{
    public StringBuilder[]? AlwaysAddFilteredStringBuilderSpan { get; } = value;

    public OrderedCollectionPredicate<StringBuilder?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredStringBuilderSpan), AlwaysAddFilteredStringBuilderSpan.AsSpan(), Filter)
           .Complete();
}

public class StringBuilderNullableSpanAlwaysAddFilteredStringBearer
    (StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate) : IStringBearer
{
    public StringBuilder?[]? AlwaysAddFilteredNullableStringBuilderSpan { get; } = value;

    public OrderedCollectionPredicate<StringBuilder?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredNullable(nameof(AlwaysAddFilteredNullableStringBuilderSpan)
                                                    , AlwaysAddFilteredNullableStringBuilderSpan.AsSpan(), Filter)
           .Complete();
}

public class MatchSpanAlwaysAddFilteredStringBearer<T, TFilterBase>
    (T[]? value, OrderedCollectionPredicate<TFilterBase> filterPredicate) : IStringBearer
    where T : TFilterBase
{
    public T[]? AlwaysAddFilteredMatchSpan { get; } = value;

    public OrderedCollectionPredicate<TFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatch(nameof(AlwaysAddFilteredMatchSpan), AlwaysAddFilteredMatchSpan.AsSpan(), Filter)
           .Complete();
}

public class MatchNullableSpanAlwaysAddFilteredStringBearer<T, TFilterBase>
    (T?[]? value, OrderedCollectionPredicate<TFilterBase> filterPredicate) : IStringBearer
    where T : TFilterBase
{
    public T?[]? AlwaysAddFilteredNullableMatchSpan { get; } = value;

    public OrderedCollectionPredicate<TFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .AlwaysAddFilteredMatchNullable(nameof(AlwaysAddFilteredNullableMatchSpan), AlwaysAddFilteredNullableMatchSpan.AsSpan(), Filter)
           .Complete();
}

public class ObjectSpanAlwaysAddFilteredStringBearer(object[]? value, OrderedCollectionPredicate<object> filterPredicate) : IStringBearer
{
    public object[]? AlwaysAddFilteredObjectSpan { get; } = value;

    public OrderedCollectionPredicate<object> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObject(nameof(AlwaysAddFilteredObjectSpan), AlwaysAddFilteredObjectSpan.AsSpan(), Filter);
}

public class NullableObjectSpanAlwaysAddFilteredStringBearer(object?[]? value, OrderedCollectionPredicate<object?> filterPredicate) : IStringBearer
{
    public object?[]? AlwaysAddFilteredNullableObjectSpan { get; } = value;

    public OrderedCollectionPredicate<object?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObjectNullable(nameof(AlwaysAddFilteredNullableObjectSpan), AlwaysAddFilteredNullableObjectSpan.AsSpan()
                                                          , Filter);
}

public class BoolReadOnlySpanAlwaysAddFilteredStringBearer(bool[]? value, OrderedCollectionPredicate<bool> filterPredicate) : IStringBearer
{
    public bool[]? AlwaysAddFilteredBoolReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<bool> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredBoolReadOnlySpan)
                                            , (ReadOnlySpan<bool>)AlwaysAddFilteredBoolReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class NullableBoolReadOnlySpanSpanAlwaysAddFilteredStringBearer
    (bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate) : IStringBearer
{
    public bool?[]? AlwaysAddFilteredNullableBoolReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<bool?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredNullableBoolReadOnlySpan)
                                            , (ReadOnlySpan<bool?>)AlwaysAddFilteredNullableBoolReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class SpanFormattableReadOnlySpanSpanAlwaysAddFilteredStringBearer<TFmt>
    (TFmt[]? value, OrderedCollectionPredicate<TFmt> filterPredicate) : IStringBearer
    where TFmt : struct, ISpanFormattable
{
    public TFmt[]? AlwaysAddFilteredSpanFormattableReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredSpanFormattableReadOnlySpan)
                                            , (ReadOnlySpan<TFmt>)AlwaysAddFilteredSpanFormattableReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class SpanFormattableNullableReadOnlySpanAlwaysAddFilteredStringBearer<TFmt>
    (TFmt?[]? value, OrderedCollectionPredicate<TFmt> filterPredicate) : IStringBearer
    where TFmt : class, ISpanFormattable
{
    public TFmt?[]? AlwaysAddFilteredSpanFormattableNullableReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredNullable(nameof(AlwaysAddFilteredSpanFormattableNullableReadOnlySpan)
                                                    , (ReadOnlySpan<TFmt?>)AlwaysAddFilteredSpanFormattableNullableReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class NullableSpanFormattableReadOnlySpanAlwaysAddFilteredStringBearer<TFmtStruct>
    (TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? AlwaysAddFilteredNullableSpanFormattableReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TFmtStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredNullableSpanFormattableReadOnlySpan)
                                            , (ReadOnlySpan<TFmtStruct?>)AlwaysAddFilteredNullableSpanFormattableReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class IpAddressReadOnlySpanAlwaysAddFilteredStringBearer
    (IPAddress?[]? value, OrderedCollectionPredicate<IPAddress?> filterPredicate) : IStringBearer
{
    public IPAddress?[]? AlwaysAddAllIpAddressReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<IPAddress?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredNullable(nameof(AlwaysAddAllIpAddressReadOnlySpan)
                                                    , (ReadOnlySpan<IPAddress?>)AlwaysAddAllIpAddressReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class CustomBearerReadOnlySpanAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
(
    TCloaked[]? value
  , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
  , PalantírReveal<TCloakedRevealBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? AlwaysAddFilteredCloakedBearerReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredCloakedBearerReadOnlySpan)
                                               , (ReadOnlySpan<TCloaked>)AlwaysAddFilteredCloakedBearerReadOnlySpan.AsSpan(), Filter, palantírReveal)
           .Complete();
}

public class CustomBearerNullableReadOnlySpanAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
(
    TCloaked?[]? value
  , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
  , PalantírReveal<TCloakedRevealBase> palantírReveal) : IStringBearer
    where TCloaked : class, TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked?[]? AlwaysAddFilteredCloakedBearerNullableReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredNullable(nameof(AlwaysAddFilteredCloakedBearerNullableReadOnlySpan)
                                                       , (ReadOnlySpan<TCloaked?>)AlwaysAddFilteredCloakedBearerNullableReadOnlySpan.AsSpan(), Filter
                                                       , palantírReveal)
           .Complete();
}

public class NullableCustomBearerReadOnlySpanAlwaysAddFilteredStringBearer<TCloakedStruct>
(
    TCloakedStruct?[]? value
  , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
  , PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? AlwaysAddFilteredNullableCloakedBearerReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TCloakedStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredNullableCloakedBearerReadOnlySpan)
                                               , (ReadOnlySpan<TCloakedStruct?>)AlwaysAddFilteredNullableCloakedBearerReadOnlySpan.AsSpan(), Filter
                                               , palantírReveal)
           .Complete();
}

public class StringBearerReadOnlySpanAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    (TBearer[]? value, OrderedCollectionPredicate<TBearerBase> filterPredicate) : IStringBearer
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? AlwaysAddFilteredStringBearerReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TBearerBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredStringBearerReadOnlySpan)
                                               , (ReadOnlySpan<TBearer>)AlwaysAddFilteredStringBearerReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class StringBearerNullableReadOnlySpanAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    (TBearer?[]? value, OrderedCollectionPredicate<TBearerBase?> filterPredicate) : IStringBearer
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? AlwaysAddFilteredStringBearerNullableReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TBearerBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredNullable(nameof(AlwaysAddFilteredStringBearerNullableReadOnlySpan)
                                                       , (ReadOnlySpan<TBearer?>)AlwaysAddFilteredStringBearerNullableReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class NullableStringBearerReadOnlySpanAlwaysAddFilteredStringBearer<TBearerStruct>
    (TBearerStruct?[]? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? AlwaysAddFilteredNullableStringBearerReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TBearerStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredNullableStringBearerReadOnlySpan),
                                                 (ReadOnlySpan<TBearerStruct?>)AlwaysAddFilteredNullableStringBearerReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class StringReadOnlySpanAlwaysAddFilteredStringBearer(string[]? value, OrderedCollectionPredicate<string?> filterPredicate) : IStringBearer
{
    public string[]? AlwaysAddFilteredStringReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<string?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredStringReadOnlySpan)
                                            , (ReadOnlySpan<string>)AlwaysAddFilteredStringReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class StringNullableReadOnlySpanAlwaysAddFilteredStringBearer
    (string?[]? value, OrderedCollectionPredicate<string?> filterPredicate) : IStringBearer
{
    public string?[]? AlwaysAddFilteredNullableStringReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<string?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredNullable(nameof(AlwaysAddFilteredNullableStringReadOnlySpan),
                                                      (ReadOnlySpan<string?>)AlwaysAddFilteredNullableStringReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class CharSequenceReadOnlySpanAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    (TCharSeq[]? value, OrderedCollectionPredicate<TCharSeqFilterBase?> filterPredicate) : IStringBearer
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq[]? AlwaysAddFilteredCharSequenceReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TCharSeqFilterBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeq(nameof(AlwaysAddFilteredCharSequenceReadOnlySpan)
                                                   , (ReadOnlySpan<TCharSeq>)AlwaysAddFilteredCharSequenceReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class CharSequenceNullableReadOnlySpanAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    (TCharSeq?[]? value, OrderedCollectionPredicate<TCharSeqFilterBase?> filterPredicate) : IStringBearer
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? AlwaysAddFilteredNullableCharSequenceReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TCharSeqFilterBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeqNullable(nameof(AlwaysAddFilteredNullableCharSequenceReadOnlySpan),
                                                             (ReadOnlySpan<TCharSeq?>)AlwaysAddFilteredNullableCharSequenceReadOnlySpan.AsSpan()
                                                           , Filter)
           .Complete();
}

public class StringBuilderReadOnlySpanAlwaysAddFilteredStringBearer
    (StringBuilder[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate) : IStringBearer
{
    public StringBuilder[]? AlwaysAddFilteredStringBuilderReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<StringBuilder?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredStringBuilderReadOnlySpan)
                                            , (ReadOnlySpan<StringBuilder>)AlwaysAddFilteredStringBuilderReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class StringBuilderNullableReadOnlySpanAlwaysAddFilteredStringBearer
    (StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate) : IStringBearer
{
    public StringBuilder?[]? AlwaysAddFilteredNullableStringBuilderReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<StringBuilder?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredNullable(nameof(AlwaysAddFilteredNullableStringBuilderReadOnlySpan)
                                                    , (ReadOnlySpan<StringBuilder?>)AlwaysAddFilteredNullableStringBuilderReadOnlySpan.AsSpan()
                                                    , Filter)
           .Complete();
}

public class MatchReadOnlySpanAlwaysAddFilteredStringBearer<T, TFilterBase>
    (T[]? value, OrderedCollectionPredicate<TFilterBase> filterPredicate) : IStringBearer
    where T : TFilterBase
{
    public T[]? AlwaysAddFilteredMatchReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatch(nameof(AlwaysAddFilteredMatchReadOnlySpan)
                                                 , (ReadOnlySpan<T>)AlwaysAddFilteredMatchReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class MatchNullableReadOnlySpanAlwaysAddFilteredStringBearer<T, TFilterBase>
    (T?[]? value, OrderedCollectionPredicate<TFilterBase> filterPredicate) : IStringBearer
    where T : TFilterBase
{
    public T?[]? AlwaysAddFilteredNullableMatchReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatchNullable(nameof(AlwaysAddFilteredNullableMatchReadOnlySpan)
                                                         , (ReadOnlySpan<T?>)AlwaysAddFilteredNullableMatchReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class ObjectReadOnlySpanAlwaysAddFilteredStringBearer(object[]? value, OrderedCollectionPredicate<object> filterPredicate) : IStringBearer
{
    public object[]? AlwaysAddFilteredObjectReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<object> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObject(nameof(AlwaysAddFilteredObjectReadOnlySpan)
                                                  , (ReadOnlySpan<object>)AlwaysAddFilteredObjectReadOnlySpan.AsSpan(), Filter);
}

public class NullableObjecReadOnlytSpanAlwaysAddFilteredStringBearer
    (object?[]? value, OrderedCollectionPredicate<object?> filterPredicate) : IStringBearer
{
    public object?[]? AlwaysAddFilteredNullableObjectReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<object?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObjectNullable(nameof(AlwaysAddFilteredNullableObjectReadOnlySpan)
                                                          , (ReadOnlySpan<object?>)AlwaysAddFilteredNullableObjectReadOnlySpan.AsSpan(), Filter);
}

public class BoolArrayAlwaysAddFilteredStringBearer(bool[]? value, OrderedCollectionPredicate<bool> filterPredicate) : IStringBearer
{
    public bool[]? AlwaysAddFilteredBoolArray { get; } = value;

    public OrderedCollectionPredicate<bool> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredBoolArray), AlwaysAddFilteredBoolArray, Filter)
           .Complete();
}

public class NullableBoolArrayAlwaysAddFilteredStringBearer(bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate) : IStringBearer
{
    public bool?[]? AlwaysAddFilteredNullableBoolArray { get; } = value;

    public OrderedCollectionPredicate<bool?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredNullableBoolArray), AlwaysAddFilteredNullableBoolArray, Filter)
           .Complete();
}

public class SpanFormattableArrayAlwaysAddFilteredStringBearer<TFmt>(TFmt[]? value, OrderedCollectionPredicate<TFmt> filterPredicate) : IStringBearer
    where TFmt : ISpanFormattable
{
    public TFmt[]? AlwaysAddFilteredSpanFormattableArray { get; } = value;

    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredSpanFormattableArray), AlwaysAddFilteredSpanFormattableArray, Filter)
           .Complete();
}

public class SpanFormattableNullableArrayAlwaysAddFilteredStringBearer<TFmt>
    (TFmt?[]? value, OrderedCollectionPredicate<TFmt> filterPredicate) : IStringBearer
    where TFmt : ISpanFormattable
{
    public TFmt?[]? AlwaysAddAllSpanFormattableArray { get; } = value;

    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddAllSpanFormattableArray), AlwaysAddAllSpanFormattableArray, Filter)
           .Complete();
}

public class NullableSpanFormattableArrayAlwaysAddFilteredStringBearer<TFmtStruct>
    (TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? AlwaysAddFilteredNullableSpanFormattableArray { get; } = value;

    public OrderedCollectionPredicate<TFmtStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredNullableSpanFormattableArray), AlwaysAddFilteredNullableSpanFormattableArray
                                            , Filter)
           .Complete();
}

public class IpAddressArrayAlwaysAddFilteredStringBearer(IPAddress?[]? value, OrderedCollectionPredicate<IPAddress?> filterPredicate) : IStringBearer
{
    public IPAddress?[]? AlwaysAddAllSpanFormattableArray { get; } = value;

    public OrderedCollectionPredicate<IPAddress?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddAllSpanFormattableArray), AlwaysAddAllSpanFormattableArray, Filter)
           .Complete();
}

public class CustomBearerArrayAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
(
    TCloaked[]? value
  , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
  , PalantírReveal<TCloakedRevealBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? AlwaysAddFilteredCloakedBearerArray { get; } = value;

    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredCloakedBearerArray), AlwaysAddFilteredCloakedBearerArray, Filter
                                               , palantírReveal)
           .Complete();
}

public class NullableCustomBearerArrayAlwaysAddFilteredStringBearer<TCloakedStruct>
(
    TCloakedStruct?[]? value
  , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
  , PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? AlwaysAddFilteredNullableCloakedBearerArray { get; } = value;

    public OrderedCollectionPredicate<TCloakedStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredNullableCloakedBearerArray), AlwaysAddFilteredNullableCloakedBearerArray
                                               , Filter, palantírReveal)
           .Complete();
}

public class StringBearerArrayAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    (TBearer?[]? value, OrderedCollectionPredicate<TBearerBase?> filterPredicate) : IStringBearer
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer?[]? AlwaysAddFilteredStringBearerArray { get; } = value;

    public OrderedCollectionPredicate<TBearerBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredStringBearerArray), AlwaysAddFilteredStringBearerArray, Filter)
           .Complete();
}

public class NullableStringBearerArrayAlwaysAddFilteredStringBearer<TBearerStruct>
    (TBearerStruct?[]? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? AlwaysAddFilteredNullableStringBearerArray { get; } = value;

    public OrderedCollectionPredicate<TBearerStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredNullableStringBearerArray), AlwaysAddFilteredNullableStringBearerArray
                                               , Filter)
           .Complete();
}

public class StringArrayAlwaysAddFilteredStringBearer(string?[]? value, OrderedCollectionPredicate<string?> filterPredicate) : IStringBearer
{
    public string?[]? AlwaysAddFilteredStringArray { get; } = value;

    public OrderedCollectionPredicate<string?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredStringArray), AlwaysAddFilteredStringArray, Filter)
           .Complete();
}

public class CharSequenceArrayAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    (TCharSeq?[]? value, OrderedCollectionPredicate<TCharSeqFilterBase?> filterPredicate) : IStringBearer
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? AlwaysAddFilteredCharSequenceArray { get; } = value;

    public OrderedCollectionPredicate<TCharSeqFilterBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeq(nameof(AlwaysAddFilteredCharSequenceArray), AlwaysAddFilteredCharSequenceArray, Filter)
           .Complete();
}

public class StringBuilderArrayAlwaysAddFilteredStringBearer
    (StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate) : IStringBearer
{
    public StringBuilder?[]? AlwaysAddFilteredStringBuilderArray { get; } = value;

    public OrderedCollectionPredicate<StringBuilder?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredStringBuilderArray), AlwaysAddFilteredStringBuilderArray, Filter)
           .Complete();
}

public class MatchArrayAlwaysAddFilteredStringBearer<T, TFilterBase>
    (T[]? value, OrderedCollectionPredicate<TFilterBase> filterPredicate) : IStringBearer
    where T : TFilterBase
{
    public T[]? AlwaysAddFilteredMatchArray { get; } = value;

    public OrderedCollectionPredicate<TFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatch(nameof(AlwaysAddFilteredMatchArray), AlwaysAddFilteredMatchArray, Filter)
           .Complete();
}

public class ObjectArrayAlwaysAddFilteredStringBearer
    (object?[]? value, OrderedCollectionPredicate<object> filterPredicate) : IStringBearer
{
    public object?[]? AlwaysAddFilteredObjectArray { get; } = value;

    public OrderedCollectionPredicate<object> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObject(nameof(AlwaysAddFilteredObjectArray), AlwaysAddFilteredObjectArray, Filter);
}

public class BoolListAlwaysAddFilteredStringBearer(List<bool>? value, OrderedCollectionPredicate<bool> filterPredicate) : IStringBearer
{
    public IReadOnlyList<bool>? AlwaysAddFilteredBoolList { get; } = value;

    public OrderedCollectionPredicate<bool> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredBoolList), AlwaysAddFilteredBoolList, Filter)
           .Complete();
}

public class NullableBoolListAlwaysAddFilteredStringBearer(List<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate) : IStringBearer
{
    public IReadOnlyList<bool?>? AlwaysAddFilteredNullableBoolList { get; } = value;

    public OrderedCollectionPredicate<bool?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredNullableBoolList), AlwaysAddFilteredNullableBoolList, Filter)
           .Complete();
}

public class SpanFormattableListAlwaysAddFilteredStringBearer<TFmt>
    (List<TFmt>? value, OrderedCollectionPredicate<TFmt> filterPredicate) : IStringBearer
    where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt>? AlwaysAddFilteredSpanFormattableList { get; } = value;

    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredSpanFormattableList), AlwaysAddFilteredSpanFormattableList, Filter)
           .Complete();
}

public class NullableSpanFormattableListAlwaysAddFilteredStringBearer<TFmtStruct>
    (List<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? AlwaysAddFilteredNullableSpanFormattableList { get; } = value;

    public OrderedCollectionPredicate<TFmtStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredNullableSpanFormattableList), AlwaysAddFilteredNullableSpanFormattableList
                                            , Filter)
           .Complete();
}

public class CustomBearerListAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
(
    List<TCloaked>? value
  , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
  , PalantírReveal<TCloakedRevealBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public IReadOnlyList<TCloaked>? AlwaysAddFilteredCloakedBearerList { get; } = value;

    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredCloakedBearerList), AlwaysAddFilteredCloakedBearerList, Filter
                                               , palantírReveal)
           .Complete();
}

public class NullableCustomBearerListAlwaysAddFilteredStringBearer<TCloakedStruct>
(
    List<TCloakedStruct?>? value
  , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
  , PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? AlwaysAddFilteredNullableCloakedBearerList { get; } = value;

    public OrderedCollectionPredicate<TCloakedStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredNullableCloakedBearerList), AlwaysAddFilteredNullableCloakedBearerList
                                               , Filter, palantírReveal)
           .Complete();
}

public class StringBearerListAlwaysAddFilteredStringBearer<TBearer, TBearerBase>
    (List<TBearer?>? value, OrderedCollectionPredicate<TBearerBase?> filterPredicate) : IStringBearer
    where TBearer : IStringBearer, TBearerBase
{
    public IReadOnlyList<TBearer?>? AlwaysAddFilteredStringBearerList { get; } = value;

    public OrderedCollectionPredicate<TBearerBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredStringBearerList), AlwaysAddFilteredStringBearerList, Filter)
           .Complete();
}

public class NullableStringBearerListAlwaysAddFilteredStringBearer<TBearerStruct>
    (List<TBearerStruct?>? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? AlwaysAddFilteredNullableStringBearerList { get; } = value;

    public OrderedCollectionPredicate<TBearerStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredNullableStringBearerList), AlwaysAddFilteredNullableStringBearerList, Filter)
           .Complete();
}

public class StringListAlwaysAddFilteredStringBearer(List<string?>? value, OrderedCollectionPredicate<string?> filterPredicate) : IStringBearer
{
    public IReadOnlyList<string?>? AlwaysAddFilteredStringList { get; } = value;

    public OrderedCollectionPredicate<string?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredStringList), AlwaysAddFilteredStringList, Filter)
           .Complete();
}

public class CharSequenceListAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>
    (List<TCharSeq?>? value, OrderedCollectionPredicate<TCharSeqFilterBase?> filterPredicate) : IStringBearer
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IReadOnlyList<TCharSeq?>? AlwaysAddFilteredCharSequenceList { get; } = value;

    public OrderedCollectionPredicate<TCharSeqFilterBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeq(nameof(AlwaysAddFilteredCharSequenceList), AlwaysAddFilteredCharSequenceList, Filter)
           .Complete();
}

public class StringBuilderListAlwaysAddFilteredStringBearer
    (StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate) : IStringBearer
{
    public StringBuilder?[]? AlwaysAddFilteredStringBuilderList { get; } = value;

    public OrderedCollectionPredicate<StringBuilder?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredStringBuilderList), AlwaysAddFilteredStringBuilderList, Filter)
           .Complete();
}

public class MatchListAlwaysAddFilteredStringBearer<T, TFilterBase>
    (List<T>? value, OrderedCollectionPredicate<TFilterBase> filterPredicate) : IStringBearer
    where T : TFilterBase
{
    public IReadOnlyList<T>? AlwaysAddFilteredMatchList { get; } = value;

    public OrderedCollectionPredicate<TFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatch(nameof(AlwaysAddFilteredMatchList), AlwaysAddFilteredMatchList, Filter)
           .Complete();
}

public class ObjectListAlwaysAddFilteredStringBearer(List<object?>? value, OrderedCollectionPredicate<object> filterPredicate) : IStringBearer
{
    public List<object?>? AlwaysAddFilteredObjectList { get; } = value;

    public OrderedCollectionPredicate<object> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObject(nameof(AlwaysAddFilteredObjectList), AlwaysAddFilteredObjectList, Filter);
}
