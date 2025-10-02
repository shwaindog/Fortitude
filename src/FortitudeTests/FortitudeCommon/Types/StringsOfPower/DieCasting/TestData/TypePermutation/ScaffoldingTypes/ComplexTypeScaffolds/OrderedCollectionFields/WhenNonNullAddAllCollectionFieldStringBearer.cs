using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.OrderedCollectionFields;

public class BoolArrayWhenNonNullAddAllStringBearer(bool[]? value) : IStringBearer
{
    public bool[]? WhenNonNullAddAllBoolArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll(nameof(WhenNonNullAddAllBoolArray), WhenNonNullAddAllBoolArray)
           .Complete();
}

public class NullableBoolArrayWhenNonNullAddAllStringBearer(bool?[]? value) : IStringBearer
{
    public bool?[]? WhenNonNullAddAllNullableBoolArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll(nameof(WhenNonNullAddAllNullableBoolArray), WhenNonNullAddAllNullableBoolArray)
           .Complete();
}

public class SpanFormattableArrayWhenNonNullAddAllStringBearer<TFmt>(TFmt[]? value) : IStringBearer
    where TFmt : ISpanFormattable
{
    public TFmt[]? WhenNonNullAddAllSpanFormattableArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll(nameof(WhenNonNullAddAllSpanFormattableArray), WhenNonNullAddAllSpanFormattableArray)
           .Complete();
}

public class NullableSpanFormattableArrayWhenNonNullAddAllStringBearer<TStructFmt>(TStructFmt?[]? value) : IStringBearer
    where TStructFmt : struct, ISpanFormattable
{
    public TStructFmt?[]? WhenNonNullAddAllNullableSpanFormattableArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll(nameof(WhenNonNullAddAllNullableSpanFormattableArray), WhenNonNullAddAllNullableSpanFormattableArray)
           .Complete();
}

public class CustomBearerArrayWhenNonNullAddAllStringBearer<TCloaked, TCloakedBase>(
    TCloaked[]? value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedBase
{
    public TCloaked[]? WhenNonNullAddAllCustomBearerArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll(nameof(WhenNonNullAddAllCustomBearerArray), WhenNonNullAddAllCustomBearerArray, palantírReveal)
           .Complete();
}

public class NullableCustomBearerArrayWhenNonNullAddAllStringBearer<TCloakedStruct>(
    TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? WhenNonNullAddAllNullableCustomBearerArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll(nameof(WhenNonNullAddAllNullableCustomBearerArray), WhenNonNullAddAllNullableCustomBearerArray, palantírReveal)
           .Complete();
}

public class StringBearerArrayWhenNonNullAddAllStringBearer<TBearer>(TBearer[]? value) : IStringBearer
    where TBearer : IStringBearer
{
    public TBearer[]? WhenNonNullAddAllStringBearerArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll(nameof(WhenNonNullAddAllStringBearerArray), WhenNonNullAddAllStringBearerArray)
           .Complete();
}

public class NullableStringBearerArrayWhenNonNullAddAllStringBearer<TBearerStruct>(TBearerStruct?[]? value) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public TBearerStruct?[]? WhenNonNullAddAllNullableStringBearerArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll(nameof(WhenNonNullAddAllNullableStringBearerArray), WhenNonNullAddAllNullableStringBearerArray)
           .Complete();
}

public class StringArrayWhenNonNullAddAllStringBearer(string?[]? value) : IStringBearer
{
    public string?[]? WhenNonNullAddAllStringArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll(nameof(WhenNonNullAddAllStringArray), WhenNonNullAddAllStringArray)
           .Complete();
}

public class CharSequenceArrayWhenNonNullAddAllStringBearer<TCharSeq>(TCharSeq?[]? value) : IStringBearer
    where TCharSeq : ICharSequence
{
    public TCharSeq?[]? WhenNonNullAddAllCharSequenceArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeq(nameof(WhenNonNullAddAllCharSequenceArray), WhenNonNullAddAllCharSequenceArray)
           .Complete();
}

public class StringBuilderArrayWhenNonNullAddAllStringBearer(StringBuilder?[]? value) : IStringBearer
{
    public StringBuilder?[]? WhenNonNullAddAllStringBuilderArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll(nameof(WhenNonNullAddAllStringBuilderArray), WhenNonNullAddAllStringBuilderArray)
           .Complete();
}

public class MatchArrayWhenNonNullAddAllStringBearer<T>(T[]? value) : IStringBearer
{
    public T[]? WhenNonNullAddAllStringBuilderArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatch(nameof(WhenNonNullAddAllStringBuilderArray), WhenNonNullAddAllStringBuilderArray)
           .Complete();
}

public class ObjectArrayWhenNonNullAddAllStringBearer(object?[]? value) : IStringBearer
{
    public object?[]? WhenNonNullAddAllStringBuilderArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllObject(nameof(WhenNonNullAddAllStringBuilderArray), WhenNonNullAddAllStringBuilderArray)
           .Complete();
}

public class BoolListWhenNonNullAddAllStringBearer(List<bool>? value) : IStringBearer
{
    public IReadOnlyList<bool>? WhenNonNullAddAllBoolList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll(nameof(WhenNonNullAddAllBoolList), WhenNonNullAddAllBoolList)
           .Complete();
}

public class NullableBoolListWhenNonNullAddAllStringBearer(List<bool?>? value) : IStringBearer
{
    public IReadOnlyList<bool?>? WhenNonNullAddAllNullableBoolList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll(nameof(WhenNonNullAddAllNullableBoolList), WhenNonNullAddAllNullableBoolList)
           .Complete();
}

public class SpanFormattableListWhenNonNullAddAllStringBearer<TFmt>(List<TFmt>? value) : IStringBearer
    where TFmt : ISpanFormattable
{
    public IReadOnlyList<TFmt>? WhenNonNullAddAllSpanFormattableList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll(nameof(WhenNonNullAddAllSpanFormattableList), WhenNonNullAddAllSpanFormattableList)
           .Complete();
}

public class NullableSpanFormattableListWhenNonNullAddAllStringBearer<TFmtStruct>(List<TFmtStruct?>? value) : IStringBearer
    where TFmtStruct : struct, ISpanFormattable
{
    public IReadOnlyList<TFmtStruct?>? WhenNonNullAddAllNullableSpanFormattableList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll(nameof(WhenNonNullAddAllNullableSpanFormattableList), WhenNonNullAddAllNullableSpanFormattableList)
           .Complete();
}

public class CustomBearerListWhenNonNullAddAllStringBearer<TCloaked, TCloakedBase>(
    List<TCloaked>? value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedBase
{
    public IReadOnlyList<TCloaked>? WhenNonNullAddAllCustomBearerList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll(nameof(WhenNonNullAddAllCustomBearerList), WhenNonNullAddAllCustomBearerList, palantírReveal)
           .Complete();
}

public class NullableCustomBearerListWhenNonNullAddAllStringBearer<TCloakedStruct>(
    List<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? WhenNonNullAddAllNullableCustomBearerList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll(nameof(WhenNonNullAddAllNullableCustomBearerList), WhenNonNullAddAllNullableCustomBearerList, palantírReveal)
           .Complete();
}

public class StringBearerListWhenNonNullAddAllStringBearer<TBearer>(List<TBearer>? value) : IStringBearer
    where TBearer : IStringBearer
{
    public IReadOnlyList<TBearer>? WhenNonNullAddAllStringBearerList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll(nameof(WhenNonNullAddAllStringBearerList), WhenNonNullAddAllStringBearerList)
           .Complete();
}

public class NullableStringBearerListWhenNonNullAddAllStringBearer<TBearerStruct>(List<TBearerStruct?>? value) : IStringBearer
    where TBearerStruct : struct, IStringBearer
{
    public IReadOnlyList<TBearerStruct?>? WhenNonNullAddAllNullableStringBearerList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullRevealAll(nameof(WhenNonNullAddAllNullableStringBearerList), WhenNonNullAddAllNullableStringBearerList)
           .Complete();
}

public class StringListWhenNonNullAddAllStringBearer(List<string?>? value) : IStringBearer
{
    public IReadOnlyList<string?>? WhenNonNullAddAllStringList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll(nameof(WhenNonNullAddAllStringList), WhenNonNullAddAllStringList)
           .Complete();
}

public class CharSequenceListWhenNonNullAddAllStringBearer<TCharSeq>(List<TCharSeq?>? value) : IStringBearer
    where TCharSeq : ICharSequence
{
    public IReadOnlyList<TCharSeq?>? WhenNonNullAddAllCharSequenceList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllCharSeq(nameof(WhenNonNullAddAllCharSequenceList), WhenNonNullAddAllCharSequenceList)
           .Complete();
}

public class StringBuilderListWhenNonNullAddAllStringBearer(List<StringBuilder?>? value) : IStringBearer
{
    public IReadOnlyList<StringBuilder?>? WhenNonNullAddAllStringBuilderList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAll(nameof(WhenNonNullAddAllStringBuilderList), WhenNonNullAddAllStringBuilderList)
           .Complete();
}

public class MatchListWhenNonNullAddAllStringBearer<T>(List<T>? value) : IStringBearer
{
    public IReadOnlyList<T>? WhenNonNullAddAllMatchList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllMatch(nameof(WhenNonNullAddAllMatchList), WhenNonNullAddAllMatchList)
           .Complete();
}

public class ObjectListWhenNonNullAddAllStringBearer(List<object?>? value) : IStringBearer
{
    public IReadOnlyList<object?>? WhenNonNullAddAllMatchList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenNonNullAddAllObject(nameof(WhenNonNullAddAllMatchList), WhenNonNullAddAllMatchList)
           .Complete();
}