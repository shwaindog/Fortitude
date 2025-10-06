using System.Net;
using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexFields.OrderedCollections;

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
    public TFmt[]? AlwaysAddAllSpanFormattableArray { get; } = value;

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

public class NullableSpanFormattableArrayAlwaysAddAllStringBearer<TStructFmt>(TStructFmt?[]? value) : IStringBearer
    where TStructFmt : struct, ISpanFormattable
{
    public TStructFmt?[]? AlwaysAddAllNullableSpanFormattableArray { get; } = value;

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

public class ObjectArrayAlwaysAddAllStringBearer<T>(T[]? value) : IStringBearer
where T : class
{
    public T[]? AlwaysAddAllObjectArray { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .CollectionField.AlwaysAddAllObject(nameof(AlwaysAddAllObjectArray), AlwaysAddAllObjectArray)
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

public class NullableSpanFormattableListAlwaysAddAllStringBearer<TStructFmt>(List<TStructFmt?>? value) : IStringBearer
    where TStructFmt : struct, ISpanFormattable
{
    public IReadOnlyList<TStructFmt?>? AlwaysAddAllNullableSpanFormattableList { get; } = value;

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