using System.Net;
using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexFields.OrderedCollections;

public class BoolSpanWhenPopulatedWithFilterStringBearer(bool[]? value, OrderedCollectionPredicate<bool> filterPredicate) : IStringBearer
{
    public bool[]? WhenPopulatedWithFilterBoolSpan { get; } = value;

    public OrderedCollectionPredicate<bool> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterBoolSpan), WhenPopulatedWithFilterBoolSpan.AsSpan(), Filter)
           .Complete();
}

public class NullableBoolSpanWhenPopulatedWithFilterStringBearer(bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate) : IStringBearer
{
    public bool?[]? WhenPopulatedWithFilterNullableBoolSpan { get; } = value;

    public OrderedCollectionPredicate<bool?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterNullableBoolSpan), WhenPopulatedWithFilterNullableBoolSpan.AsSpan(), Filter)
           .Complete();
}

public class SpanFormattableSpanWhenPopulatedWithFilterStringBearer<TFmt>(TFmt[]? value, OrderedCollectionPredicate<TFmt> filterPredicate) : IStringBearer
    where TFmt : struct, ISpanFormattable
{
    public TFmt[]? WhenPopulatedWithFilterSpanFormattableSpan { get; } = value;

    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterSpanFormattableSpan), WhenPopulatedWithFilterSpanFormattableSpan.AsSpan(), Filter)
           .Complete();
}

public class SpanFormattableNullableSpanWhenPopulatedWithFilterStringBearer<TFmt>
    (TFmt?[]? value, OrderedCollectionPredicate<TFmt> filterPredicate) : IStringBearer
    where TFmt : class, ISpanFormattable
{
    public TFmt?[]? WhenPopulatedWithFilterSpanFormattableNullableSpan { get; } = value;

    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterNullable(nameof(WhenPopulatedWithFilterSpanFormattableNullableSpan)
                                                    , WhenPopulatedWithFilterSpanFormattableNullableSpan.AsSpan(), Filter)
           .Complete();
}

public class NullableSpanFormattableSpanWhenPopulatedWithFilterStringBearer<TFmtStruct>
    (TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? WhenPopulatedWithFilterNullableSpanFormattableSpan { get; } = value;

    public OrderedCollectionPredicate<TFmtStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterNullableSpanFormattableSpan)
                                            , WhenPopulatedWithFilterNullableSpanFormattableSpan.AsSpan(), Filter)
           .Complete();
}

public class IpAddressSpanWhenPopulatedWithFilterStringBearer(IPAddress?[]? value, OrderedCollectionPredicate<IPAddress?> filterPredicate) : IStringBearer
{
    public IPAddress?[]? WhenPopulatedWithFilterIpAddressSpan { get; } = value;

    public OrderedCollectionPredicate<IPAddress?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterNullable(nameof(WhenPopulatedWithFilterIpAddressSpan), WhenPopulatedWithFilterIpAddressSpan.AsSpan(), Filter)
           .Complete();
}

public class CustomBearerSpanWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
(
    TCloaked[]? value
  , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
  , PalantírReveal<TCloakedRevealBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? WhenPopulatedWithFilterCloakedBearerSpan { get; } = value;

    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(WhenPopulatedWithFilterCloakedBearerSpan), WhenPopulatedWithFilterCloakedBearerSpan.AsSpan(), Filter
                                               , palantírReveal)
           .Complete();
}

public class CustomBearerNullableSpanWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
(
    TCloaked?[]? value
  , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
  , PalantírReveal<TCloakedRevealBase> palantírReveal) : IStringBearer
    where TCloaked : class, TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked?[]? WhenPopulatedWithFilterCloakedBearerNullableSpan { get; } = value;

    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredNullable(nameof(WhenPopulatedWithFilterCloakedBearerNullableSpan)
                                                       , WhenPopulatedWithFilterCloakedBearerNullableSpan.AsSpan(), Filter, palantírReveal)
           .Complete();
}

public class NullableCustomBearerSpanWhenPopulatedWithFilterStringBearer<TCloakedStruct>
(
    TCloakedStruct?[]? value
  , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
  , PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? WhenPopulatedWithFilterNullableCloakedBearerSpan { get; } = value;

    public OrderedCollectionPredicate<TCloakedStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(WhenPopulatedWithFilterNullableCloakedBearerSpan)
                                               , WhenPopulatedWithFilterNullableCloakedBearerSpan.AsSpan(), Filter, palantírReveal)
           .Complete();
}

public class StringBearerSpanWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    (TBearer[]? value, OrderedCollectionPredicate<TBearerBase> filterPredicate) : IStringBearer
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? WhenPopulatedWithFilterStringBearerSpan { get; } = value;

    public OrderedCollectionPredicate<TBearerBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(WhenPopulatedWithFilterStringBearerSpan), WhenPopulatedWithFilterStringBearerSpan.AsSpan(), Filter)
           .Complete();
}

public class StringBearerNullableSpanWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    (TBearer?[]? value, OrderedCollectionPredicate<TBearerBase?> filterPredicate) : IStringBearer
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? WhenPopulatedWithFilterStringBearerNullableSpan { get; } = value;

    public OrderedCollectionPredicate<TBearerBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredNullable(nameof(WhenPopulatedWithFilterStringBearerNullableSpan)
                                                       , WhenPopulatedWithFilterStringBearerNullableSpan.AsSpan(), Filter)
           .Complete();
}

public class NullableStringBearerSpanWhenPopulatedWithFilterStringBearer<TBearerStruct>
    (TBearerStruct?[]? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? WhenPopulatedWithFilterNullableStringBearerSpan { get; } = value;

    public OrderedCollectionPredicate<TBearerStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(WhenPopulatedWithFilterNullableStringBearerSpan), WhenPopulatedWithFilterNullableStringBearerSpan.AsSpan()
                                               , Filter)
           .Complete();
}

public class StringSpanWhenPopulatedWithFilterStringBearer(string[]? value, OrderedCollectionPredicate<string?> filterPredicate) : IStringBearer
{
    public string[]? WhenPopulatedWithFilterStringSpan { get; } = value;

    public OrderedCollectionPredicate<string?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterStringSpan), WhenPopulatedWithFilterStringSpan.AsSpan(), Filter)
           .Complete();
}

public class StringNullableSpanWhenPopulatedWithFilterStringBearer(string?[]? value, OrderedCollectionPredicate<string?> filterPredicate) : IStringBearer
{
    public string?[]? WhenPopulatedWithFilterNullableStringSpan { get; } = value;

    public OrderedCollectionPredicate<string?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenPopulatedWithFilterNullable(nameof(WhenPopulatedWithFilterNullableStringSpan), WhenPopulatedWithFilterNullableStringSpan.AsSpan(), Filter)
           .Complete();
}

public class CharSequenceSpanWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    (TCharSeq[]? value, OrderedCollectionPredicate<TCharSeqFilterBase?> filterPredicate) : IStringBearer
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq[]? WhenPopulatedWithFilterCharSequenceSpan { get; } = value;

    public OrderedCollectionPredicate<TCharSeqFilterBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeq(nameof(WhenPopulatedWithFilterCharSequenceSpan), WhenPopulatedWithFilterCharSequenceSpan.AsSpan(), Filter)
           .Complete();
}

public class CharSequenceNullableSpanWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    (TCharSeq?[]? value, OrderedCollectionPredicate<TCharSeqFilterBase?> filterPredicate) : IStringBearer
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? WhenPopulatedWithFilterNullableCharSequenceSpan { get; } = value;

    public OrderedCollectionPredicate<TCharSeqFilterBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeqNullable(nameof(WhenPopulatedWithFilterNullableCharSequenceSpan)
                                                           , WhenPopulatedWithFilterNullableCharSequenceSpan.AsSpan(), Filter)
           .Complete();
}

public class StringBuilderSpanWhenPopulatedWithFilterStringBearer
    (StringBuilder[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate) : IStringBearer
{
    public StringBuilder[]? WhenPopulatedWithFilterStringBuilderSpan { get; } = value;

    public OrderedCollectionPredicate<StringBuilder?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterStringBuilderSpan), WhenPopulatedWithFilterStringBuilderSpan.AsSpan(), Filter)
           .Complete();
}

public class StringBuilderNullableSpanWhenPopulatedWithFilterStringBearer
    (StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate) : IStringBearer
{
    public StringBuilder?[]? WhenPopulatedWithFilterNullableStringBuilderSpan { get; } = value;

    public OrderedCollectionPredicate<StringBuilder?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterNullable(nameof(WhenPopulatedWithFilterNullableStringBuilderSpan)
                                                    , WhenPopulatedWithFilterNullableStringBuilderSpan.AsSpan(), Filter)
           .Complete();
}

public class MatchSpanWhenPopulatedWithFilterStringBearer<T, TFilterBase>
    (T[]? value, OrderedCollectionPredicate<TFilterBase> filterPredicate) : IStringBearer
    where T : TFilterBase
{
    public T[]? WhenPopulatedWithFilterMatchSpan { get; } = value;

    public OrderedCollectionPredicate<TFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterMatch(nameof(WhenPopulatedWithFilterMatchSpan), WhenPopulatedWithFilterMatchSpan.AsSpan(), Filter)
           .Complete();
}

public class MatchNullableSpanWhenPopulatedWithFilterStringBearer<T, TFilterBase>
    (T?[]? value, OrderedCollectionPredicate<TFilterBase> filterPredicate) : IStringBearer
    where T : TFilterBase
{
    public T?[]? WhenPopulatedWithFilterNullableMatchSpan { get; } = value;

    public OrderedCollectionPredicate<TFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField
           .WhenPopulatedWithFilterMatchNullable(nameof(WhenPopulatedWithFilterNullableMatchSpan), WhenPopulatedWithFilterNullableMatchSpan.AsSpan(), Filter)
           .Complete();
}

public class ObjectSpanWhenPopulatedWithFilterStringBearer(object[]? value, OrderedCollectionPredicate<object> filterPredicate) : IStringBearer
{
    public object[]? WhenPopulatedWithFilterObjectSpan { get; } = value;

    public OrderedCollectionPredicate<object> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObject(nameof(WhenPopulatedWithFilterObjectSpan), WhenPopulatedWithFilterObjectSpan.AsSpan(), Filter);
}

public class NullableObjectSpanWhenPopulatedWithFilterStringBearer(object?[]? value, OrderedCollectionPredicate<object?> filterPredicate) : IStringBearer
{
    public object?[]? WhenPopulatedWithFilterNullableObjectSpan { get; } = value;

    public OrderedCollectionPredicate<object?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObjectNullable(nameof(WhenPopulatedWithFilterNullableObjectSpan), WhenPopulatedWithFilterNullableObjectSpan.AsSpan()
                                                          , Filter);
}

public class BoolReadOnlySpanWhenPopulatedWithFilterStringBearer(bool[]? value, OrderedCollectionPredicate<bool> filterPredicate) : IStringBearer
{
    public bool[]? WhenPopulatedWithFilterBoolReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<bool> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterBoolReadOnlySpan)
                                            , (ReadOnlySpan<bool>)WhenPopulatedWithFilterBoolReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class NullableBoolReadOnlySpanSpanWhenPopulatedWithFilterStringBearer
    (bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate) : IStringBearer
{
    public bool?[]? WhenPopulatedWithFilterNullableBoolReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<bool?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterNullableBoolReadOnlySpan)
                                            , (ReadOnlySpan<bool?>)WhenPopulatedWithFilterNullableBoolReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class SpanFormattableReadOnlySpanSpanWhenPopulatedWithFilterStringBearer<TFmt>
    (TFmt[]? value, OrderedCollectionPredicate<TFmt> filterPredicate) : IStringBearer
    where TFmt : struct, ISpanFormattable
{
    public TFmt[]? WhenPopulatedWithFilterSpanFormattableReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterSpanFormattableReadOnlySpan)
                                            , (ReadOnlySpan<TFmt>)WhenPopulatedWithFilterSpanFormattableReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class SpanFormattableNullableReadOnlySpanWhenPopulatedWithFilterStringBearer<TFmt>
    (TFmt?[]? value, OrderedCollectionPredicate<TFmt> filterPredicate) : IStringBearer
    where TFmt : class, ISpanFormattable
{
    public TFmt?[]? WhenPopulatedWithFilterSpanFormattableNullableReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterNullable(nameof(WhenPopulatedWithFilterSpanFormattableNullableReadOnlySpan)
                                                    , (ReadOnlySpan<TFmt?>)WhenPopulatedWithFilterSpanFormattableNullableReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class NullableSpanFormattableReadOnlySpanWhenPopulatedWithFilterStringBearer<TFmtStruct>
    (TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public TFmtStruct?[]? WhenPopulatedWithFilterNullableSpanFormattableReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TFmtStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterNullableSpanFormattableReadOnlySpan)
                                            , (ReadOnlySpan<TFmtStruct?>)WhenPopulatedWithFilterNullableSpanFormattableReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class IpAddressReadOnlySpanWhenPopulatedWithFilterStringBearer
    (IPAddress?[]? value, OrderedCollectionPredicate<IPAddress?> filterPredicate) : IStringBearer
{
    public IPAddress?[]? WhenPopulatedWithFilterIpAddressReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<IPAddress?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterNullable(nameof(WhenPopulatedWithFilterIpAddressReadOnlySpan)
                                                    , (ReadOnlySpan<IPAddress?>)WhenPopulatedWithFilterIpAddressReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class CustomBearerReadOnlySpanWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
(
    TCloaked[]? value
  , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
  , PalantírReveal<TCloakedRevealBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? WhenPopulatedWithFilterCloakedBearerReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(WhenPopulatedWithFilterCloakedBearerReadOnlySpan)
                                               , (ReadOnlySpan<TCloaked>)WhenPopulatedWithFilterCloakedBearerReadOnlySpan.AsSpan(), Filter, palantírReveal)
           .Complete();
}

public class CustomBearerNullableReadOnlySpanWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>
(
    TCloaked?[]? value
  , OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate
  , PalantírReveal<TCloakedRevealBase> palantírReveal) : IStringBearer
    where TCloaked : class, TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked?[]? WhenPopulatedWithFilterCloakedBearerNullableReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredNullable(nameof(WhenPopulatedWithFilterCloakedBearerNullableReadOnlySpan)
                                                       , (ReadOnlySpan<TCloaked?>)WhenPopulatedWithFilterCloakedBearerNullableReadOnlySpan.AsSpan(), Filter
                                                       , palantírReveal)
           .Complete();
}

public class NullableCustomBearerReadOnlySpanWhenPopulatedWithFilterStringBearer<TCloakedStruct>
(
    TCloakedStruct?[]? value
  , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
  , PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? WhenPopulatedWithFilterNullableCloakedBearerReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TCloakedStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(WhenPopulatedWithFilterNullableCloakedBearerReadOnlySpan)
                                               , (ReadOnlySpan<TCloakedStruct?>)WhenPopulatedWithFilterNullableCloakedBearerReadOnlySpan.AsSpan(), Filter
                                               , palantírReveal)
           .Complete();
}

public class StringBearerReadOnlySpanWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    (TBearer[]? value, OrderedCollectionPredicate<TBearerBase> filterPredicate) : IStringBearer
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer[]? WhenPopulatedWithFilterStringBearerReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TBearerBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(WhenPopulatedWithFilterStringBearerReadOnlySpan)
                                               , (ReadOnlySpan<TBearer>)WhenPopulatedWithFilterStringBearerReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class StringBearerNullableReadOnlySpanWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>
    (TBearer?[]? value, OrderedCollectionPredicate<TBearerBase?> filterPredicate) : IStringBearer
    where TBearer : class, IStringBearer, TBearerBase
{
    public TBearer?[]? WhenPopulatedWithFilterStringBearerNullableReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TBearerBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFilteredNullable(nameof(WhenPopulatedWithFilterStringBearerNullableReadOnlySpan)
                                                       , (ReadOnlySpan<TBearer?>)WhenPopulatedWithFilterStringBearerNullableReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class NullableStringBearerReadOnlySpanWhenPopulatedWithFilterStringBearer<TBearerStruct>
    (TBearerStruct?[]? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? WhenPopulatedWithFilterNullableStringBearerReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TBearerStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(WhenPopulatedWithFilterNullableStringBearerReadOnlySpan),
                                                 (ReadOnlySpan<TBearerStruct?>)WhenPopulatedWithFilterNullableStringBearerReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class StringReadOnlySpanWhenPopulatedWithFilterStringBearer(string[]? value, OrderedCollectionPredicate<string?> filterPredicate) : IStringBearer
{
    public string[]? WhenPopulatedWithFilterStringReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<string?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterStringReadOnlySpan)
                                            , (ReadOnlySpan<string>)WhenPopulatedWithFilterStringReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class StringNullableReadOnlySpanWhenPopulatedWithFilterStringBearer
    (string?[]? value, OrderedCollectionPredicate<string?> filterPredicate) : IStringBearer
{
    public string?[]? WhenPopulatedWithFilterNullableStringReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<string?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterNullable(nameof(WhenPopulatedWithFilterNullableStringReadOnlySpan),
                                                      (ReadOnlySpan<string?>)WhenPopulatedWithFilterNullableStringReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class CharSequenceReadOnlySpanWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    (TCharSeq[]? value, OrderedCollectionPredicate<TCharSeqFilterBase?> filterPredicate) : IStringBearer
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq[]? WhenPopulatedWithFilterCharSequenceReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TCharSeqFilterBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeq(nameof(WhenPopulatedWithFilterCharSequenceReadOnlySpan)
                                                   , (ReadOnlySpan<TCharSeq>)WhenPopulatedWithFilterCharSequenceReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class CharSequenceNullableReadOnlySpanWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>
    (TCharSeq?[]? value, OrderedCollectionPredicate<TCharSeqFilterBase?> filterPredicate) : IStringBearer
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? WhenPopulatedWithFilterNullableCharSequenceReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TCharSeqFilterBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeqNullable(nameof(WhenPopulatedWithFilterNullableCharSequenceReadOnlySpan),
                                                             (ReadOnlySpan<TCharSeq?>)WhenPopulatedWithFilterNullableCharSequenceReadOnlySpan.AsSpan()
                                                           , Filter)
           .Complete();
}

public class StringBuilderReadOnlySpanWhenPopulatedWithFilterStringBearer
    (StringBuilder[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate) : IStringBearer
{
    public StringBuilder[]? WhenPopulatedWithFilterStringBuilderReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<StringBuilder?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterStringBuilderReadOnlySpan)
                                            , (ReadOnlySpan<StringBuilder>)WhenPopulatedWithFilterStringBuilderReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class StringBuilderNullableReadOnlySpanWhenPopulatedWithFilterStringBearer
    (StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate) : IStringBearer
{
    public StringBuilder?[]? WhenPopulatedWithFilterNullableStringBuilderReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<StringBuilder?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterNullable(nameof(WhenPopulatedWithFilterNullableStringBuilderReadOnlySpan)
                                                    , (ReadOnlySpan<StringBuilder?>)WhenPopulatedWithFilterNullableStringBuilderReadOnlySpan.AsSpan()
                                                    , Filter)
           .Complete();
}

public class MatchReadOnlySpanWhenPopulatedWithFilterStringBearer<T, TFilterBase>
    (T[]? value, OrderedCollectionPredicate<TFilterBase> filterPredicate) : IStringBearer
    where T : TFilterBase
{
    public T[]? WhenPopulatedWithFilterMatchReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterMatch(nameof(WhenPopulatedWithFilterMatchReadOnlySpan)
                                                 , (ReadOnlySpan<T>)WhenPopulatedWithFilterMatchReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class MatchNullableReadOnlySpanWhenPopulatedWithFilterStringBearer<T, TFilterBase>
    (T?[]? value, OrderedCollectionPredicate<TFilterBase> filterPredicate) : IStringBearer
    where T : TFilterBase
{
    public T?[]? WhenPopulatedWithFilterNullableMatchReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<TFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterMatchNullable(nameof(WhenPopulatedWithFilterNullableMatchReadOnlySpan)
                                                         , (ReadOnlySpan<T?>)WhenPopulatedWithFilterNullableMatchReadOnlySpan.AsSpan(), Filter)
           .Complete();
}

public class ObjectReadOnlySpanWhenPopulatedWithFilterStringBearer(object[]? value, OrderedCollectionPredicate<object> filterPredicate) : IStringBearer
{
    public object[]? WhenPopulatedWithFilterObjectReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<object> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObject(nameof(WhenPopulatedWithFilterObjectReadOnlySpan)
                                                  , (ReadOnlySpan<object>)WhenPopulatedWithFilterObjectReadOnlySpan.AsSpan(), Filter);
}

public class NullableObjecReadOnlytSpanWhenPopulatedWithFilterStringBearer
    (object?[]? value, OrderedCollectionPredicate<object?> filterPredicate) : IStringBearer
{
    public object?[]? WhenPopulatedWithFilterNullableObjectReadOnlySpan { get; } = value;

    public OrderedCollectionPredicate<object?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObjectNullable(nameof(WhenPopulatedWithFilterNullableObjectReadOnlySpan)
                                                          , (ReadOnlySpan<object?>)WhenPopulatedWithFilterNullableObjectReadOnlySpan.AsSpan(), Filter);
}

public class BoolArrayWhenPopulatedWithFilterStringBearer(bool[]? value, OrderedCollectionPredicate<bool> filterPredicate) : IStringBearer
{
    public bool[]? WhenPopulatedWithFilterBoolArray { get; } = value;

    public OrderedCollectionPredicate<bool> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterBoolArray), WhenPopulatedWithFilterBoolArray, Filter)
           .Complete();
}

public class NullableBoolArrayWhenPopulatedWithFilterStringBearer(bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate) : IStringBearer
{
    public bool?[]? WhenPopulatedWithFilterNullableBoolArray { get; } = value;

    public OrderedCollectionPredicate<bool?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterNullableBoolArray), WhenPopulatedWithFilterNullableBoolArray, Filter)
           .Complete();
}

public class SpanFormattableArrayWhenPopulatedWithFilterStringBearer<TFmt>(TFmt[]? value, OrderedCollectionPredicate<TFmt> filterPredicate) : IStringBearer
    where TFmt : ISpanFormattable
{
    public TFmt[]? WhenPopulatedWithFilterSpanFormattableArray { get; } = value;

    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterSpanFormattableArray), WhenPopulatedWithFilterSpanFormattableArray, Filter)
           .Complete();
}

public class NullableSpanFormattableArrayWhenPopulatedWithFilterStringBearer<TStructFmt>(TStructFmt?[]? value, OrderedCollectionPredicate<TStructFmt?> filterPredicate) : IStringBearer
    where TStructFmt : struct, ISpanFormattable
{
    public TStructFmt?[]? WhenPopulatedWithFilterNullableSpanFormattableArray { get; } = value;

    public OrderedCollectionPredicate<TStructFmt?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterNullableSpanFormattableArray), WhenPopulatedWithFilterNullableSpanFormattableArray, Filter)
           .Complete();
}

public class CustomBearerArrayWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>(
    TCloaked[]? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedRevealBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? WhenPopulatedWithFilterCustomBearerArray { get; } = value;

    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal(nameof(WhenPopulatedWithFilterCustomBearerArray), WhenPopulatedWithFilterCustomBearerArray, Filter, palantírReveal)
           .Complete();
}

public class NullableCustomBearerArrayWhenPopulatedWithFilterStringBearer<TCloakedStruct>(
    TCloakedStruct?[]? value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? WhenPopulatedWithFilterNullableCustomBearerArray { get; } = value;

    public OrderedCollectionPredicate<TCloakedStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal(nameof(WhenPopulatedWithFilterNullableCustomBearerArray), WhenPopulatedWithFilterNullableCustomBearerArray, Filter, palantírReveal)
           .Complete();
}

public class StringBearerArrayWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>(TBearer?[]? value, OrderedCollectionPredicate<TBearerBase?> filterPredicate) : IStringBearer
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer?[]? WhenPopulatedWithFilterStringBearerArray { get; } = value;

    public OrderedCollectionPredicate<TBearerBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal(nameof(WhenPopulatedWithFilterStringBearerArray), WhenPopulatedWithFilterStringBearerArray, Filter)
           .Complete();
}

public class NullableStringBearerArrayWhenPopulatedWithFilterStringBearer<TBearerStruct>(TBearerStruct?[]? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? WhenPopulatedWithFilterNullableStringBearerArray { get; } = value;

    public OrderedCollectionPredicate<TBearerStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal(nameof(WhenPopulatedWithFilterNullableStringBearerArray), WhenPopulatedWithFilterNullableStringBearerArray, Filter)
           .Complete();
}

public class StringArrayWhenPopulatedWithFilterStringBearer(string?[]? value, OrderedCollectionPredicate<string?> filterPredicate) : IStringBearer
{
    public string?[]? WhenPopulatedWithFilterStringArray { get; } = value;

    public OrderedCollectionPredicate<string?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterStringArray), WhenPopulatedWithFilterStringArray, Filter)
           .Complete();
}

public class CharSequenceArrayWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>(TCharSeq?[]? value, OrderedCollectionPredicate<TCharSeqFilterBase?> filterPredicate) : IStringBearer
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? WhenPopulatedWithFilterCharSequenceArray { get; } = value;

    public OrderedCollectionPredicate<TCharSeqFilterBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeq(nameof(WhenPopulatedWithFilterCharSequenceArray), WhenPopulatedWithFilterCharSequenceArray, Filter)
           .Complete();
}

public class StringBuilderArrayWhenPopulatedWithFilterStringBearer(StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate) : IStringBearer
{
    public StringBuilder?[]? WhenPopulatedWithFilterStringBuilderArray { get; } = value;

    public OrderedCollectionPredicate<StringBuilder?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterStringBuilderArray), WhenPopulatedWithFilterStringBuilderArray, Filter)
           .Complete();
}

public class MatchArrayWhenPopulatedWithFilterStringBearer<T, TFilterBase>(T[]? value, OrderedCollectionPredicate<TFilterBase?> filterPredicate) : IStringBearer
    where T : TFilterBase
{
    public T[]? WhenPopulatedWithFilterStringBuilderArray { get; } = value;

    public OrderedCollectionPredicate<TFilterBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterMatch(nameof(WhenPopulatedWithFilterStringBuilderArray), WhenPopulatedWithFilterStringBuilderArray, Filter)
           .Complete();
}

public class ObjectArrayWhenPopulatedWithFilterStringBearer(object[]? value, OrderedCollectionPredicate<object?> filterPredicate) : IStringBearer
{
    public object?[]? WhenPopulatedWithFilterStringBuilderArray { get; } = value;

    public OrderedCollectionPredicate<object?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObject(nameof(WhenPopulatedWithFilterStringBuilderArray), WhenPopulatedWithFilterStringBuilderArray, Filter);
}

public class BoolListWhenPopulatedWithFilterStringBearer(List<bool>? value, OrderedCollectionPredicate<bool> filterPredicate) : IStringBearer
{
    public IReadOnlyList<bool>? WhenPopulatedWithFilterBoolList { get; } = value;

    public OrderedCollectionPredicate<bool> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterBoolList), WhenPopulatedWithFilterBoolList, Filter)
           .Complete();
}

public class NullableBoolListWhenPopulatedWithFilterStringBearer(List<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate) : IStringBearer
{
    public IReadOnlyList<bool?>? WhenPopulatedWithFilterNullableBoolList { get; } = value;

    public OrderedCollectionPredicate<bool?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterNullableBoolList), WhenPopulatedWithFilterNullableBoolList, Filter)
           .Complete();
}

public class SpanFormattableListWhenPopulatedWithFilterStringBearer<TFmt>(List<TFmt>? value, OrderedCollectionPredicate<TFmt> filterPredicate) : IStringBearer
    where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt>? WhenPopulatedWithFilterSpanFormattableList { get; } = value;

    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterSpanFormattableList), WhenPopulatedWithFilterSpanFormattableList, Filter)
           .Complete();
}

public class NullableSpanFormattableListWhenPopulatedWithFilterStringBearer<TStructFmt>(List<TStructFmt?>? value, OrderedCollectionPredicate<TStructFmt?> filterPredicate) : IStringBearer
    where TStructFmt : struct, ISpanFormattable
{
    public IReadOnlyList<TStructFmt?>? WhenPopulatedWithFilterNullableSpanFormattableList { get; } = value;

    public OrderedCollectionPredicate<TStructFmt?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterNullableSpanFormattableList), WhenPopulatedWithFilterNullableSpanFormattableList, Filter)
           .Complete();
}

public class CustomBearerListWhenPopulatedWithFilterStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>(
    List<TCloaked>? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedRevealBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public IReadOnlyList<TCloaked>? WhenPopulatedWithFilterCustomBearerList { get; } = value;

    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal(nameof(WhenPopulatedWithFilterCustomBearerList), WhenPopulatedWithFilterCustomBearerList, Filter, palantírReveal)
           .Complete();
}

public class NullableCustomBearerListWhenPopulatedWithFilterStringBearer<TCloakedStruct>(
    List<TCloakedStruct?>? value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? WhenPopulatedWithFilterNullableCustomBearerList { get; } = value;

    public OrderedCollectionPredicate<TCloakedStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal(nameof(WhenPopulatedWithFilterNullableCustomBearerList), WhenPopulatedWithFilterNullableCustomBearerList, Filter, palantírReveal)
           .Complete();
}

public class StringBearerListWhenPopulatedWithFilterStringBearer<TBearer, TBearerBase>(List<TBearer?>? value, OrderedCollectionPredicate<TBearerBase?> filterPredicate) : IStringBearer
    where TBearer : IStringBearer, TBearerBase
{
    public IReadOnlyList<TBearer?>? WhenPopulatedWithFilterStringBearerList { get; } = value;

    public OrderedCollectionPredicate<TBearerBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal(nameof(WhenPopulatedWithFilterStringBearerList), WhenPopulatedWithFilterStringBearerList, Filter)
           .Complete();
}

public class NullableStringBearerListWhenPopulatedWithFilterStringBearer<TBearerStruct>(List<TBearerStruct?>? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? WhenPopulatedWithFilterNullableStringBearerList { get; } = value;

    public OrderedCollectionPredicate<TBearerStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterReveal(nameof(WhenPopulatedWithFilterNullableStringBearerList), WhenPopulatedWithFilterNullableStringBearerList, Filter)
           .Complete();
}

public class StringListWhenPopulatedWithFilterStringBearer(List<string?>? value, OrderedCollectionPredicate<string?> filterPredicate) : IStringBearer
{
    public IReadOnlyList<string?>? WhenPopulatedWithFilterStringList { get; } = value;

    public OrderedCollectionPredicate<string?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterStringList), WhenPopulatedWithFilterStringList, Filter)
           .Complete();
}

public class CharSequenceListWhenPopulatedWithFilterStringBearer<TCharSeq, TCharSeqFilterBase>(List<TCharSeq?>? value, OrderedCollectionPredicate<TCharSeqFilterBase?> filterPredicate) : IStringBearer
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IReadOnlyList<TCharSeq?>? WhenPopulatedWithFilterCharSequenceList { get; } = value;

    public OrderedCollectionPredicate<TCharSeqFilterBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterCharSeq(nameof(WhenPopulatedWithFilterCharSequenceList), WhenPopulatedWithFilterCharSequenceList, Filter)
           .Complete();
}

public class StringBuilderListWhenPopulatedWithFilterStringBearer(StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate) : IStringBearer
{
    public StringBuilder?[]? WhenPopulatedWithFilterStringBuilderList { get; } = value;

    public OrderedCollectionPredicate<StringBuilder?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterStringBuilderList), WhenPopulatedWithFilterStringBuilderList, Filter)
           .Complete();
}

public class MatchListWhenPopulatedWithFilterStringBearer<T, TFilterBase>(List<T?>? value, OrderedCollectionPredicate<TFilterBase?> filterPredicate) : IStringBearer
    where T : TFilterBase
{
    public IReadOnlyList<T?>? WhenPopulatedWithFilterMatchList { get; } = value;

    public OrderedCollectionPredicate<TFilterBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterMatch(nameof(WhenPopulatedWithFilterMatchList), WhenPopulatedWithFilterMatchList, Filter)
           .Complete();
}

public class ObjectListWhenPopulatedWithFilterStringBearer(List<object?>? value, OrderedCollectionPredicate<object> filterPredicate) : IStringBearer
{
    public List<object?>? WhenPopulatedWithFilterStringBuilderList { get; } = value;

    public OrderedCollectionPredicate<object> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObject(nameof(WhenPopulatedWithFilterStringBuilderList), WhenPopulatedWithFilterStringBuilderList, Filter);
}