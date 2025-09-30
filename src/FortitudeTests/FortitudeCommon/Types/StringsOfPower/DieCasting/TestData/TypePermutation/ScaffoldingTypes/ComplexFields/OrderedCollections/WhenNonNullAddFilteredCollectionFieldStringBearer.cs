using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexFields.OrderedCollections;

public class BoolArrayWhenNonNullAddFilteredStringBearer(bool[]? value, OrderedCollectionPredicate<bool> filterPredicate) : IStringBearer
{
    public bool[]? WhenNonNullAddFilteredBoolArray { get; } = value;

    public OrderedCollectionPredicate<bool> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered(nameof(WhenNonNullAddFilteredBoolArray), WhenNonNullAddFilteredBoolArray, Filter)
           .Complete();
}

public class NullableBoolArrayWhenNonNullAddFilteredStringBearer(bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate) : IStringBearer
{
    public bool?[]? WhenNonNullAddFilteredNullableBoolArray { get; } = value;

    public OrderedCollectionPredicate<bool?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered(nameof(WhenNonNullAddFilteredNullableBoolArray), WhenNonNullAddFilteredNullableBoolArray, Filter)
           .Complete();
}

public class SpanFormattableArrayWhenNonNullAddFilteredStringBearer<TFmt>(TFmt[]? value, OrderedCollectionPredicate<TFmt> filterPredicate) : IStringBearer
    where TFmt : ISpanFormattable
{
    public TFmt[]? WhenNonNullAddFilteredSpanFormattableArray { get; } = value;

    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered(nameof(WhenNonNullAddFilteredSpanFormattableArray), WhenNonNullAddFilteredSpanFormattableArray, Filter)
           .Complete();
}

public class NullableSpanFormattableArrayWhenNonNullAddFilteredStringBearer<TStructFmt>(TStructFmt?[]? value, OrderedCollectionPredicate<TStructFmt?> filterPredicate) : IStringBearer
    where TStructFmt : struct, ISpanFormattable
{
    public TStructFmt?[]? WhenNonNullAddFilteredNullableSpanFormattableArray { get; } = value;

    public OrderedCollectionPredicate<TStructFmt?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered(nameof(WhenNonNullAddFilteredNullableSpanFormattableArray), WhenNonNullAddFilteredNullableSpanFormattableArray, Filter)
           .Complete();
}

public class CustomBearerArrayWhenNonNullAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>(
    TCloaked[]? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedRevealBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public TCloaked[]? WhenNonNullAddFilteredCustomBearerArray { get; } = value;

    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered(nameof(WhenNonNullAddFilteredCustomBearerArray), WhenNonNullAddFilteredCustomBearerArray, Filter, palantírReveal)
           .Complete();
}

public class NullableCustomBearerArrayWhenNonNullAddFilteredStringBearer<TCloakedStruct>(
    TCloakedStruct?[]? value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? WhenNonNullAddFilteredNullableCustomBearerArray { get; } = value;

    public OrderedCollectionPredicate<TCloakedStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered(nameof(WhenNonNullAddFilteredNullableCustomBearerArray), WhenNonNullAddFilteredNullableCustomBearerArray, Filter, palantírReveal)
           .Complete();
}

public class StringBearerArrayWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>(TBearer?[]? value, OrderedCollectionPredicate<TBearerBase?> filterPredicate) : IStringBearer
    where TBearer : IStringBearer, TBearerBase
{
    public TBearer?[]? WhenNonNullAddFilteredStringBearerArray { get; } = value;

    public OrderedCollectionPredicate<TBearerBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered(nameof(WhenNonNullAddFilteredStringBearerArray), WhenNonNullAddFilteredStringBearerArray, Filter)
           .Complete();
}

public class NullableStringBearerArrayWhenNonNullAddFilteredStringBearer<TBearerStruct>(TBearerStruct?[]? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? WhenNonNullAddFilteredNullableStringBearerArray { get; } = value;

    public OrderedCollectionPredicate<TBearerStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered(nameof(WhenNonNullAddFilteredNullableStringBearerArray), WhenNonNullAddFilteredNullableStringBearerArray, Filter)
           .Complete();
}

public class StringArrayWhenNonNullAddFilteredStringBearer(string?[]? value, OrderedCollectionPredicate<string?> filterPredicate) : IStringBearer
{
    public string?[]? WhenNonNullAddFilteredStringArray { get; } = value;

    public OrderedCollectionPredicate<string?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered(nameof(WhenNonNullAddFilteredStringArray), WhenNonNullAddFilteredStringArray, Filter)
           .Complete();
}

public class CharSequenceArrayWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>(TCharSeq?[]? value, OrderedCollectionPredicate<TCharSeqFilterBase?> filterPredicate) : IStringBearer
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public TCharSeq?[]? WhenNonNullAddFilteredCharSequenceArray { get; } = value;

    public OrderedCollectionPredicate<TCharSeqFilterBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeq(nameof(WhenNonNullAddFilteredCharSequenceArray), WhenNonNullAddFilteredCharSequenceArray, Filter)
           .Complete();
}

public class StringBuilderArrayWhenNonNullAddFilteredStringBearer(StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate) : IStringBearer
{
    public StringBuilder?[]? WhenNonNullAddFilteredStringBuilderArray { get; } = value;

    public OrderedCollectionPredicate<StringBuilder?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered(nameof(WhenNonNullAddFilteredStringBuilderArray), WhenNonNullAddFilteredStringBuilderArray, Filter)
           .Complete();
}

public class MatchArrayWhenNonNullAddFilteredStringBearer<T, TFilterBase>(T[]? value, OrderedCollectionPredicate<TFilterBase?> filterPredicate) : IStringBearer
    where T : TFilterBase
{
    public T[]? WhenNonNullAddFilteredStringBuilderArray { get; } = value;

    public OrderedCollectionPredicate<TFilterBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredMatch(nameof(WhenNonNullAddFilteredStringBuilderArray), WhenNonNullAddFilteredStringBuilderArray, Filter)
           .Complete();
}

public class ObjectArrayWhenNonNullAddFilteredStringBearer(object?[]? value, OrderedCollectionPredicate<object> filterPredicate) : IStringBearer
{
    public object?[]? WhenNonNullAddFilteredStringBuilderArray { get; } = value;

    public OrderedCollectionPredicate<object> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObject(nameof(WhenNonNullAddFilteredStringBuilderArray), WhenNonNullAddFilteredStringBuilderArray, Filter);
}

public class BoolListWhenNonNullAddFilteredStringBearer(List<bool>? value, OrderedCollectionPredicate<bool> filterPredicate) : IStringBearer
{
    public IReadOnlyList<bool>? WhenNonNullAddFilteredBoolList { get; } = value;

    public OrderedCollectionPredicate<bool> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered(nameof(WhenNonNullAddFilteredBoolList), WhenNonNullAddFilteredBoolList, Filter)
           .Complete();
}

public class NullableBoolListWhenNonNullAddFilteredStringBearer(List<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate) : IStringBearer
{
    public IReadOnlyList<bool?>? WhenNonNullAddFilteredNullableBoolList { get; } = value;

    public OrderedCollectionPredicate<bool?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered(nameof(WhenNonNullAddFilteredNullableBoolList), WhenNonNullAddFilteredNullableBoolList, Filter)
           .Complete();
}

public class SpanFormattableListWhenNonNullAddFilteredStringBearer<TFmt>(List<TFmt>? value, OrderedCollectionPredicate<TFmt> filterPredicate) : IStringBearer
    where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt>? WhenNonNullAddFilteredSpanFormattableList { get; } = value;

    public OrderedCollectionPredicate<TFmt> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered(nameof(WhenNonNullAddFilteredSpanFormattableList), WhenNonNullAddFilteredSpanFormattableList, Filter)
           .Complete();
}

public class NullableSpanFormattableListWhenNonNullAddFilteredStringBearer<TStructFmt>(List<TStructFmt?>? value, OrderedCollectionPredicate<TStructFmt?> filterPredicate) : IStringBearer
    where TStructFmt : struct, ISpanFormattable
{
    public IReadOnlyList<TStructFmt?>? WhenNonNullAddFilteredNullableSpanFormattableList { get; } = value;

    public OrderedCollectionPredicate<TStructFmt?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered(nameof(WhenNonNullAddFilteredNullableSpanFormattableList), WhenNonNullAddFilteredNullableSpanFormattableList, Filter)
           .Complete();
}

public class CustomBearerListWhenNonNullAddFilteredStringBearer<TCloaked, TCloakedFilterBase, TCloakedRevealBase>(
    List<TCloaked>? value, OrderedCollectionPredicate<TCloakedFilterBase> filterPredicate, PalantírReveal<TCloakedRevealBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedRevealBase, TCloakedFilterBase
{
    public IReadOnlyList<TCloaked>? WhenNonNullAddFilteredCustomBearerList { get; } = value;

    public OrderedCollectionPredicate<TCloakedFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered(nameof(WhenNonNullAddFilteredCustomBearerList), WhenNonNullAddFilteredCustomBearerList, Filter, palantírReveal)
           .Complete();
}

public class NullableCustomBearerListWhenNonNullAddFilteredStringBearer<TCloakedStruct>(
    List<TCloakedStruct?>? value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? WhenNonNullAddFilteredNullableCustomBearerList { get; } = value;

    public OrderedCollectionPredicate<TCloakedStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered(nameof(WhenNonNullAddFilteredNullableCustomBearerList), WhenNonNullAddFilteredNullableCustomBearerList, Filter, palantírReveal)
           .Complete();
}

public class StringBearerListWhenNonNullAddFilteredStringBearer<TBearer, TBearerBase>(List<TBearer?>? value, OrderedCollectionPredicate<TBearerBase?> filterPredicate) : IStringBearer
    where TBearer : IStringBearer, TBearerBase
{
    public IReadOnlyList<TBearer?>? WhenNonNullAddFilteredStringBearerList { get; } = value;

    public OrderedCollectionPredicate<TBearerBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered(nameof(WhenNonNullAddFilteredStringBearerList), WhenNonNullAddFilteredStringBearerList, Filter)
           .Complete();
}

public class NullableStringBearerListWhenNonNullAddFilteredStringBearer<TBearerStruct>(List<TBearerStruct?>? value, OrderedCollectionPredicate<TBearerStruct?> filterPredicate) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? WhenNonNullAddFilteredNullableStringBearerList { get; } = value;

    public OrderedCollectionPredicate<TBearerStruct?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealFiltered(nameof(WhenNonNullAddFilteredNullableStringBearerList), WhenNonNullAddFilteredNullableStringBearerList, Filter)
           .Complete();
}

public class StringListWhenNonNullAddFilteredStringBearer(List<string?>? value, OrderedCollectionPredicate<string?> filterPredicate) : IStringBearer
{
    public IReadOnlyList<string?>? WhenNonNullAddFilteredStringList { get; } = value;

    public OrderedCollectionPredicate<string?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered(nameof(WhenNonNullAddFilteredStringList), WhenNonNullAddFilteredStringList, Filter)
           .Complete();
}

public class CharSequenceListWhenNonNullAddFilteredStringBearer<TCharSeq, TCharSeqFilterBase>(List<TCharSeq?>? value, OrderedCollectionPredicate<TCharSeqFilterBase?> filterPredicate) : IStringBearer
    where TCharSeq : ICharSequence, TCharSeqFilterBase
{
    public IReadOnlyList<TCharSeq?>? WhenNonNullAddFilteredCharSequenceList { get; } = value;

    public OrderedCollectionPredicate<TCharSeqFilterBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredCharSeq(nameof(WhenNonNullAddFilteredCharSequenceList), WhenNonNullAddFilteredCharSequenceList, Filter)
           .Complete();
}

public class StringBuilderListWhenNonNullAddFilteredStringBearer(StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate) : IStringBearer
{
    public IReadOnlyList<StringBuilder?>? WhenNonNullAddFilteredStringBuilderList { get; } = value;

    public OrderedCollectionPredicate<StringBuilder?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFiltered(nameof(WhenNonNullAddFilteredStringBuilderList), WhenNonNullAddFilteredStringBuilderList, Filter)
           .Complete();
}

public class MatchListWhenNonNullAddFilteredStringBearer<T, TFilterBase>(List<T?>? value, OrderedCollectionPredicate<TFilterBase?> filterPredicate) : IStringBearer
    where T : TFilterBase
{
    public IReadOnlyList<T?>? WhenNonNullAddFilteredMatchList { get; } = value;

    public OrderedCollectionPredicate<TFilterBase?> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredMatch(nameof(WhenNonNullAddFilteredMatchList), WhenNonNullAddFilteredMatchList, Filter)
           .Complete();
}

public class ObjectListWhenNonNullAddFilteredStringBearer(List<object?>? value, OrderedCollectionPredicate<object> filterPredicate) : IStringBearer
{
    public IReadOnlyList<object?>? WhenNonNullAddFilteredObjectList { get; } = value;

    public OrderedCollectionPredicate<object> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddFilteredObject(nameof(WhenNonNullAddFilteredObjectList), WhenNonNullAddFilteredObjectList, Filter);
}