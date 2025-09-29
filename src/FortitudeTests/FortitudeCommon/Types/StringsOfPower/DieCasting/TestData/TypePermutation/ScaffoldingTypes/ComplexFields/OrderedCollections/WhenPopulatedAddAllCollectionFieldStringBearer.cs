using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexFields.OrderedCollections;

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
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllNullableSpanFormattableArray), WhenPopulatedAddAllNullableSpanFormattableArray)
           .Complete();
}

public class CustomBearerArrayWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase>(
    TCloaked[]? value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedBase
{
    public TCloaked[]? WhenPopulatedAddAllCustomBearerArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll(nameof(WhenPopulatedAddAllCustomBearerArray), WhenPopulatedAddAllCustomBearerArray, palantírReveal)
           .Complete();
}

public class NullableCustomBearerArrayWhenPopulatedAddAllStringBearer<TCloakedStruct>(
    TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public TCloakedStruct?[]? WhenPopulatedAddAllNullableCustomBearerArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll(nameof(WhenPopulatedAddAllNullableCustomBearerArray), WhenPopulatedAddAllNullableCustomBearerArray, palantírReveal)
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

public class ObjectArrayWhenPopulatedAddFilteredStringBearer<T>(T[]? value) : IStringBearer
    where T : class
{
    public T[]? WhenPopulatedAddObjectArray { get; } = value;

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
           .CollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllNullableSpanFormattableList), WhenPopulatedAddAllNullableSpanFormattableList)
           .Complete();
}

public class CustomBearerListWhenPopulatedAddAllStringBearer<TCloaked, TCloakedBase>(
    List<TCloaked>? value, PalantírReveal<TCloakedBase> palantírReveal) : IStringBearer
    where TCloaked : TCloakedBase
{
    public IReadOnlyList<TCloaked>? WhenPopulatedAddAllCustomBearerList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll(nameof(WhenPopulatedAddAllCustomBearerList), WhenPopulatedAddAllCustomBearerList, palantírReveal)
           .Complete();
}

public class NullableCustomBearerListWhenPopulatedAddAllStringBearer<TCloakedStruct>(
    List<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal) : IStringBearer
    where TCloakedStruct : struct
{
    public IReadOnlyList<TCloakedStruct?>? WhenPopulatedAddAllNullableCustomBearerList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedRevealAll(nameof(WhenPopulatedAddAllNullableCustomBearerList), WhenPopulatedAddAllNullableCustomBearerList, palantírReveal)
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

public class ObjectListWhenPopulatedAddFilteredStringBearer<T>(List<T>? value) : IStringBearer
    where T : class
{
    public IReadOnlyList<T>? WhenPopulatedAddObjectList { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.WhenPopulatedAddAllObject(nameof(WhenPopulatedAddObjectList), WhenPopulatedAddObjectList);
}