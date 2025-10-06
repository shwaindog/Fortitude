// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;

// ReSharper disable PossibleMultipleEnumeration

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenPopulatedAddAll(string fieldName, bool[]? value) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, bool?[]? value) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TFmt>(string fieldName, TFmt?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TFmtStruct>(string fieldName, TFmtStruct?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TCloaked, TCloakedBase>
        (string fieldName, TCloaked?[]? value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TCloakedStruct>
        (string fieldName, TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TBearer>(string fieldName, TBearer?[]? value) where TBearer : IStringBearer =>
        !stb.SkipFields & value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TBearerStruct>(string fieldName, TBearerStruct?[]? value) where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysRevealAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllCharSeq<TCharSeq>(string fieldName, TCharSeq?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllCharSeq(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    public TExt WhenPopulatedAddAllMatch<T>(string fieldName, T?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObject<T>(string fieldName, T?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where T : class  =>
        !stb.SkipFields && value is { Length: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<bool>? value) =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<bool?>? value) =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysAddAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TFmt>(string fieldName, IReadOnlyList<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TCloaked, TCloakedBase>
        (string fieldName, IReadOnlyList<TCloaked?>? value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysRevealAll(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TCloakedStruct>
        (string fieldName, IReadOnlyList<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysRevealAll(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TBearer>(string fieldName, IReadOnlyList<TBearer?>? value)
        where TBearer : IStringBearer =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysRevealAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAll<TBearerStruct>(string fieldName, IReadOnlyList<TBearerStruct?>? value)
        where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysRevealAll(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllCharSeq<TCharSeq>(string fieldName, IReadOnlyList<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysAddAllCharSeq(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAll(string fieldName, IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysAddAll(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllMatch<T>(string fieldName, IReadOnlyList<T?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObject<T>(string fieldName, IReadOnlyList<T?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where T : class  =>
        !stb.SkipFields && value is { Count: > 0 } ? AlwaysAddAllMatch(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<bool>? value) =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<bool?>? value) =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TFmt>(string fieldName, IEnumerable<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerable<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAllEnumerate<TCloaked, TCloakedBase>
        (string fieldName, IEnumerable<TCloaked?>? value, PalantírReveal<TCloakedBase> palantírReveal) where TCloaked : TCloakedBase =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysRevealAllEnumerate(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAllEnumerate<TCloakedStruct>
        (string fieldName, IEnumerable<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal) where TCloakedStruct : struct =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysRevealAllEnumerate(fieldName, value, palantírReveal) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAllEnumerate<TBearer>(string fieldName, IEnumerable<TBearer?>? value)
        where TBearer : IStringBearer =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysRevealAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedRevealAllEnumerate<TBearerStruct>(string fieldName, IEnumerable<TBearerStruct?>? value)
        where TBearerStruct : struct, IStringBearer =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysRevealAllEnumerate(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllCharSeqEnumerate<TCharSeq>(string fieldName, IEnumerable<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAllCharSeqEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAllEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    public TExt WhenPopulatedAddAllMatchEnumerate<T>(string fieldName, IEnumerable<T?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)  =>
        !stb.SkipFields && (value?.Any() ?? false) ? AlwaysAddAllMatchEnumerate(fieldName, value, formatString) : stb.StyleTypeBuilder;
    
    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObjectEnumerate<T>(string fieldName, IEnumerable<T?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class =>
        WhenPopulatedAddAllMatchEnumerate(fieldName, value, formatString);

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<bool>? value)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<IEnumerator<bool>, bool>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<bool?>? value)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<IEnumerator<bool?>, bool?>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate<TFmt>(string fieldName, IEnumerator<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TFmt>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, formatString);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerator<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TFmtStruct?>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, formatString);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedRevealAllEnumerate<TCloaked, TCloakedBase>
        (string fieldName, IEnumerator<TCloaked?>? value, PalantírReveal<TCloakedBase> palantírReveal)
        where TCloaked : TCloakedBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TCloaked>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, palantírReveal);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedRevealAllEnumerate<TCloakedStruct>
        (string fieldName, IEnumerator<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal)
        where TCloakedStruct : struct
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TCloakedStruct>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, palantírReveal);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }
    
    public TExt WhenPopulatedRevealAllEnumerate<TBearer>(string fieldName, IEnumerator<TBearer?>? value)
        where TBearer : IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TBearer>(value!);
            while (hasValue)
            {
                eoctb.AddBearerElementAndGoToNextElement(value!.Current);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }
    
    public TExt WhenPopulatedRevealAllEnumerate<TBearerStruct>(string fieldName, IEnumerator<TBearerStruct?>? value)
        where TBearerStruct : struct, IStringBearer
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TBearerStruct>(value!);
            while (hasValue)
            {
                eoctb.AddBearerElementAndGoToNextElement(value!.Current);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }


    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<string>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, formatString);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllCharSeqEnumerate<TCharSeq>(string fieldName, IEnumerator<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<TCharSeq?>(value!);
            while (hasValue)
            {
                eoctb.AddCharSequenceElementAndGoToNextElement(value!.Current, formatString);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllEnumerate(string fieldName, IEnumerator<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<StringBuilder>(value!);
            while (hasValue)
            {
                eoctb.AddElementAndGoToNextElement(value!.Current, formatString);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedAddAllMatchEnumerate<T>(string fieldName, IEnumerator<T?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);

        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.FieldNameJoin(fieldName);
            var eoctb = stb.Master.StartExplicitCollectionType<T>(value!);
            while (hasValue)
            {
                eoctb.AddMatchElementAndGoToNextElement(value!.Current, formatString);
                hasValue = value.MoveNext();
            }
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }
    
    [CallsObjectToString]
    public TExt WhenPopulatedAddAllObjectEnumerate<T>(string fieldName, IEnumerator<T?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where T : class =>
        WhenPopulatedAddAllMatchEnumerate(fieldName, value, formatString);
}
