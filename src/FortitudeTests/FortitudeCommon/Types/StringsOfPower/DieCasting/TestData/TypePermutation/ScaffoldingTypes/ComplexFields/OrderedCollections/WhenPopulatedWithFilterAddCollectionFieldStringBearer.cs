using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;

// ReSharper disable MemberCanBePrivate.Global

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexFields.OrderedCollections;

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

public class ObjectArrayWhenPopulatedWithFilterStringBearer<T, TFilterBase>(T[]? value, OrderedCollectionPredicate<TFilterBase> filterPredicate) : IStringBearer
    where T : class, TFilterBase
{
    public T[]? WhenPopulatedWithFilterStringBuilderArray { get; } = value;

    public OrderedCollectionPredicate<TFilterBase> Filter { get; set; } = filterPredicate;

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

public class ObjectListWhenPopulatedWithFilterStringBearer<T, TFilterBase>(List<T>? value, OrderedCollectionPredicate<TFilterBase> filterPredicate) : IStringBearer
    where T : class, TFilterBase
{
    public List<T>? WhenPopulatedWithFilterStringBuilderList { get; } = value;

    public OrderedCollectionPredicate<TFilterBase> Filter { get; set; } = filterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedWithFilterObject(nameof(WhenPopulatedWithFilterStringBuilderList), WhenPopulatedWithFilterStringBuilderList, Filter);
}