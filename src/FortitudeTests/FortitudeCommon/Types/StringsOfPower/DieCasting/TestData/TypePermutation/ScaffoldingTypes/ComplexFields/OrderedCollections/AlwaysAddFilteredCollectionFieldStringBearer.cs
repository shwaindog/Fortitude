using System.Net;
using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexFields.OrderedCollections;

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

public class NullableSpanFormattableArrayAlwaysAddFilteredStringBearer<TStructFmt>(TStructFmt?[]? value, OrderedCollectionPredicate<TStructFmt?> filterPredicate) : IStringBearer
    where TStructFmt : struct, ISpanFormattable
{
    public TStructFmt?[]? AlwaysAddFilteredNullableSpanFormattableArray { get; } = value;

    public OrderedCollectionPredicate<TStructFmt?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredNullableSpanFormattableArray), AlwaysAddFilteredNullableSpanFormattableArray, Filter)
           .Complete();
}

public class SpanFormattableNullableArrayAlwaysAddFilteredStringBearer<TFmt>(TFmt?[]? value, OrderedCollectionPredicate<TFmt> filterPredicate) : IStringBearer
    where TFmt : ISpanFormattable
{
    public TFmt?[]? AlwaysAddAllSpanFormattableArray { get; } = value;

    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddAllSpanFormattableArray), AlwaysAddAllSpanFormattableArray, Filter)
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

public class CustomBearerArrayAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>(
    TCloaked[]? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedRevealBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? AlwaysAddFilteredCustomBearerArray { get; } = value;

    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredCustomBearerArray), AlwaysAddFilteredCustomBearerArray, Filter, palantírReveal)
           .Complete();
}

public class NullableCustomBearerArrayAlwaysAddFilteredStringBearer<TCloakedStruct>(
    TCloakedStruct?[]? value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? AlwaysAddFilteredNullableCustomBearerArray { get; } = value;

    public OrderedCollectionPredicate<TCloakedStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredNullableCustomBearerArray), AlwaysAddFilteredNullableCustomBearerArray, Filter, palantírReveal)
           .Complete();
}

public class StringBearerArrayAlwaysAddFilteredStringBearer<TBearer, TBearerBase>(TBearer?[]? value, OrderedCollectionPredicate<TBearerBase?> filterPredicate) : IStringBearer
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer?[]? AlwaysAddFilteredStringBearerArray { get; } = value;

    public OrderedCollectionPredicate<TBearerBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredStringBearerArray), AlwaysAddFilteredStringBearerArray, Filter)
           .Complete();
}

public class NullableStringBearerArrayAlwaysAddFilteredStringBearer<TBearerStruct>(TBearerStruct?[]? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? AlwaysAddFilteredNullableStringBearerArray { get; } = value;

    public OrderedCollectionPredicate<TBearerStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredNullableStringBearerArray), AlwaysAddFilteredNullableStringBearerArray, Filter)
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

public class CharSequenceArrayAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>(TCharSeq?[]? value, OrderedCollectionPredicate<TCharSeqFilterBase?> filterPredicate) : IStringBearer
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? AlwaysAddFilteredCharSequenceArray { get; } = value;

    public OrderedCollectionPredicate<TCharSeqFilterBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeq(nameof(AlwaysAddFilteredCharSequenceArray), AlwaysAddFilteredCharSequenceArray, Filter)
           .Complete();
}

public class StringBuilderArrayAlwaysAddFilteredStringBearer(StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate) : IStringBearer
{
    public StringBuilder?[]? AlwaysAddFilteredStringBuilderArray { get; } = value;

    public OrderedCollectionPredicate<StringBuilder?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredStringBuilderArray), AlwaysAddFilteredStringBuilderArray, Filter)
           .Complete();
}

public class MatchArrayAlwaysAddFilteredStringBearer<T, TFilterBase>(T[]? value, OrderedCollectionPredicate<TFilterBase> filterPredicate) : IStringBearer
    where T : TFilterBase
{
    public T[]? AlwaysAddFilteredStringBuilderArray { get; } = value;

    public OrderedCollectionPredicate<TFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatch(nameof(AlwaysAddFilteredStringBuilderArray), AlwaysAddFilteredStringBuilderArray, Filter)
           .Complete();
}

public class ObjectArrayAlwaysAddFilteredStringBearer<T, TFilterBase>(T[]? value, OrderedCollectionPredicate<TFilterBase> filterPredicate) : IStringBearer
    where T : class, TFilterBase
{
    public T[]? AlwaysAddFilteredStringBuilderArray { get; } = value;

    public OrderedCollectionPredicate<TFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObject(nameof(AlwaysAddFilteredStringBuilderArray), AlwaysAddFilteredStringBuilderArray, Filter);
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

public class SpanFormattableListAlwaysAddFilteredStringBearer<TFmt>(List<TFmt>? value, OrderedCollectionPredicate<TFmt> filterPredicate) : IStringBearer
    where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt>? AlwaysAddFilteredSpanFormattableList { get; } = value;

    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredSpanFormattableList), AlwaysAddFilteredSpanFormattableList, Filter)
           .Complete();
}

public class NullableSpanFormattableListAlwaysAddFilteredStringBearer<TStructFmt>(List<TStructFmt?>? value, OrderedCollectionPredicate<TStructFmt?> filterPredicate) : IStringBearer
    where TStructFmt : struct, ISpanFormattable
{
    public IReadOnlyList<TStructFmt?>? AlwaysAddFilteredNullableSpanFormattableList { get; } = value;

    public OrderedCollectionPredicate<TStructFmt?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredNullableSpanFormattableList), AlwaysAddFilteredNullableSpanFormattableList, Filter)
           .Complete();
}

public class CustomBearerListAlwaysAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>(
    List<TCloaked>? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedRevealBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public IReadOnlyList<TCloaked>? AlwaysAddFilteredCustomBearerList { get; } = value;

    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredCustomBearerList), AlwaysAddFilteredCustomBearerList, Filter, palantírReveal)
           .Complete();
}

public class NullableCustomBearerListAlwaysAddFilteredStringBearer<TCloakedStruct>(
    List<TCloakedStruct?>? value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? AlwaysAddFilteredNullableCustomBearerList { get; } = value;

    public OrderedCollectionPredicate<TCloakedStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredNullableCustomBearerList), AlwaysAddFilteredNullableCustomBearerList, Filter, palantírReveal)
           .Complete();
}

public class StringBearerListAlwaysAddFilteredStringBearer<TBearer, TBearerBase>(List<TBearer?>? value, OrderedCollectionPredicate<TBearerBase?> filterPredicate) : IStringBearer
    where TBearer : IStringBearer, TBearerBase
{
    public IReadOnlyList<TBearer?>? AlwaysAddFilteredStringBearerList { get; } = value;

    public OrderedCollectionPredicate<TBearerBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysRevealFiltered(nameof(AlwaysAddFilteredStringBearerList), AlwaysAddFilteredStringBearerList, Filter)
           .Complete();
}

public class NullableStringBearerListAlwaysAddFilteredStringBearer<TBearerStruct>(List<TBearerStruct?>? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate) : IStringBearer
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

public class CharSequenceListAlwaysAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>(List<TCharSeq?>? value, OrderedCollectionPredicate<TCharSeqFilterBase?> filterPredicate) : IStringBearer
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IReadOnlyList<TCharSeq?>? AlwaysAddFilteredCharSequenceList { get; } = value;

    public OrderedCollectionPredicate<TCharSeqFilterBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredCharSeq(nameof(AlwaysAddFilteredCharSequenceList), AlwaysAddFilteredCharSequenceList, Filter)
           .Complete();
}

public class StringBuilderListAlwaysAddFilteredStringBearer(StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate) : IStringBearer
{
    public StringBuilder?[]? AlwaysAddFilteredStringBuilderList { get; } = value;

    public OrderedCollectionPredicate<StringBuilder?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredStringBuilderList), AlwaysAddFilteredStringBuilderList, Filter)
           .Complete();
}

public class MatchListAlwaysAddFilteredStringBearer<T, TFilterBase>(List<T>? value, OrderedCollectionPredicate<TFilterBase> filterPredicate) : IStringBearer
    where T : TFilterBase
{
    public IReadOnlyList<T>? AlwaysAddFilteredMatchList { get; } = value;

    public OrderedCollectionPredicate<TFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredMatch(nameof(AlwaysAddFilteredMatchList), AlwaysAddFilteredMatchList, Filter)
           .Complete();
}

public class ObjectListAlwaysAddFilteredStringBearer<T, TFilterBase>(List<T>? value, OrderedCollectionPredicate<TFilterBase> filterPredicate) : IStringBearer
    where T : class, TFilterBase
{
    public List<T>? AlwaysAddFilteredStringBuilderArray { get; } = value;

    public OrderedCollectionPredicate<TFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddFilteredObject(nameof(AlwaysAddFilteredStringBuilderArray), AlwaysAddFilteredStringBuilderArray, Filter);
}