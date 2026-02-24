// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, Span<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<bool>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(bool);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionType<bool>(actualType, formatFlags)
           .AddAll(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, Span<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<bool?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(bool?);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionTypeOfNullable<bool>(actualType, formatFlags)
           .AddAll(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll<TFmt>(ReadOnlySpan<char> fieldName, Span<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?
    {
        var actualType = typeof(Span<TFmt>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TFmt);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionType<TFmt>(actualType, formatFlags)
           .AddAll(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll<TFmtStruct>(ReadOnlySpan<char> fieldName, Span<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(Span<TFmtStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TFmtStruct?);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionTypeOfNullable<TFmtStruct>(actualType, formatFlags)
           .AddAll(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TCloaked, TRevealBase>(ReadOnlySpan<char> fieldName, Span<TCloaked> value
      , PalantírReveal<TRevealBase> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = typeof(Span<TCloaked>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TRevealBase);
        if (value.Length == 0) { return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, 0, 0, formatString, formatFlags, false); }
        stb.Master
           .StartExplicitCollectionType<TCloaked>(actualType, formatFlags)
           .RevealAll(value, palantírReveal, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TCloakedStruct>(ReadOnlySpan<char> fieldName, Span<TCloakedStruct?> value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = typeof(Span<TCloakedStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TCloakedStruct?);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionTypeOfNullable<TCloakedStruct>(actualType, formatFlags)
           .RevealAll(value, palantírReveal, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TBearer>(ReadOnlySpan<char> fieldName, Span<TBearer> value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = typeof(Span<TBearer>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TBearer);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionType<TBearer>(actualType, formatFlags)
           .RevealAll(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, Span<TBearerStruct?> value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(Span<TBearerStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TBearerStruct?);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionTypeOfNullable<TBearerStruct>(actualType, formatFlags)
           .RevealAll(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, Span<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<string>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(string);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionType<string>(actualType, formatFlags)
           .AddAll(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllNullable(ReadOnlySpan<char> fieldName, Span<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<string?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(string);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionType<string?>(actualType, formatFlags)
           .AddAllNullable(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, Span<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType = typeof(Span<TCharSeq>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TCharSeq);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionType<TCharSeq>(actualType, formatFlags)
           .AddAllCharSeq(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }


    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, Span<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<StringBuilder>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(StringBuilder);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionType<StringBuilder>(actualType, formatFlags)
           .AddAll(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllNullable(ReadOnlySpan<char> fieldName, Span<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<StringBuilder?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(StringBuilder);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionType<StringBuilder?>(actualType, formatFlags)
           .AddAllNullable(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllMatch<TAny>(ReadOnlySpan<char> fieldName, Span<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<TAny>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TAny);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionType<TAny>(actualType, formatFlags)
           .AddAllMatch(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObject(ReadOnlySpan<char> fieldName, Span<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<object>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(object);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionType<object>(actualType, formatFlags)
           .AddAllObject(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObjectNullable(ReadOnlySpan<char> fieldName, Span<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<object?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(object);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionType<object?>(actualType, formatFlags)
           .AddAllObjectNullable(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<bool>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(bool);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionType<bool>(actualType, formatFlags)
           .AddAll(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<bool?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(bool?);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionTypeOfNullable<bool>(actualType, formatFlags)
           .AddAll(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll<TFmt>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?
    {
        var actualType = typeof(ReadOnlySpan<TFmt>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TFmt);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionType<TFmt>(actualType, formatFlags)
           .AddAll(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll<TFmtStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmtStruct?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(ReadOnlySpan<TFmtStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TFmtStruct?);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionTypeOfNullable<TFmtStruct>(actualType, formatFlags)
           .AddAll(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TCloaked, TRevealBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloaked> value
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = typeof(ReadOnlySpan<TCloaked>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TRevealBase);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionType<TCloaked>(actualType, formatFlags)
           .RevealAll(value, palantírReveal, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TCloakedStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloakedStruct?> value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = typeof(ReadOnlySpan<TCloakedStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TCloakedStruct?);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionTypeOfNullable<TCloakedStruct>(actualType, formatFlags)
           .RevealAll(value, palantírReveal, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TBearer>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer> value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = typeof(ReadOnlySpan<TBearer>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TBearer);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionType<TBearer>(actualType, formatFlags)
           .RevealAll(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearerStruct?> value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(ReadOnlySpan<TBearerStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName
                                , formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TBearerStruct?);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionTypeOfNullable<TBearerStruct>(actualType, formatFlags)
           .RevealAll(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<string>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(string);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionType<string>(actualType, formatFlags)
           .AddAll(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<string?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(string);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionType<string?>(actualType, formatFlags)
           .AddAllNullable(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllCharSeq<TCharSeq>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType = typeof(ReadOnlySpan<TCharSeq>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TCharSeq);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionType<TCharSeq>(actualType, formatFlags)
           .AddAllCharSeq(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<StringBuilder>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName
                                , formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(StringBuilder);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionType<StringBuilder>(actualType, formatFlags)
           .AddAll(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<StringBuilder?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName
                                , formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(StringBuilder);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, null, 0, formatString, formatFlags, false);
        }
        stb.Master
           .StartExplicitCollectionType<StringBuilder?>(actualType, formatFlags)
           .AddAllNullable(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllMatch<TAny>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<TAny>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TAny);
        if (value.Length == 0) { return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, actualType, 0, 0, formatString, formatFlags, false); }
        stb.Master
           .StartExplicitCollectionType<TAny>(actualType, formatFlags)
           .AddAllMatch(value, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext(true);
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObject(ReadOnlySpan<char> fieldName, ReadOnlySpan<object> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddAllMatch(fieldName, value, formatString, formatFlags);

    [CallsObjectToString]
    public TExt AlwaysAddAllObjectNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddAllMatch(fieldName, value, formatString, formatFlags);

    public TExt AlwaysAddAll(string fieldName, bool[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(bool[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(bool);

        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAll(value, formatString, formatFlags)
               .Complete();
        else { stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags, false); }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll(string fieldName, bool?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(bool?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(bool?);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAll(value, formatString, formatFlags)
               .Complete();
        else { stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags, false); }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll<TFmt>(string fieldName, TFmt[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(TFmt[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TFmt);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var collectionType = stb.Master.StartSimpleCollectionType(value, formatFlags | AsCollection);
            collectionType.AddAll(value, formatString, formatFlags);
            collectionType.Complete();
        }
        else { stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags, false); }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll<TFmtStruct>(string fieldName, TFmtStruct?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(TFmtStruct?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TFmtStruct?);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAll(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TCloaked, TRevealBase>
    (string fieldName, TCloaked?[]? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TCloaked?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCloaked?);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAll(value, palantírReveal, formatString, formatFlags)
               .Complete();
        else { stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags, false); }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TCloakedStruct>
    (string fieldName, TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(TCloakedStruct?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCloakedStruct?);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAll(value, palantírReveal, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TBearer>(string fieldName, TBearer[]? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TBearer[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TBearer);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAll(value, formatString, formatFlags)
               .Complete();
        else { stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags, false); }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TBearerStruct>(string fieldName, TBearerStruct?[]? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(TBearerStruct?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TBearerStruct?);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAll(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll(string fieldName, string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(string?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(string);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAll(value, formatString, formatFlags)
               .Complete();
        else { stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags, false); }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllCharSeq<TCharSeq>(string fieldName, TCharSeq?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCharSeq);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllCharSeq(value, formatString, formatFlags)
               .Complete();
        else { stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags, false); }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll(string fieldName, StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(StringBuilder?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(StringBuilder);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAll(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllMatch<TAny>(string fieldName, TAny?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(TAny?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TAny);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllMatch(value, formatString, formatFlags)
               .Complete();
        else { stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags, false); }
        return stb.AddGoToNext(true);
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObject(string fieldName, object?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(object?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(object);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllMatch(value, formatString, formatFlags)
               .Complete();
        else { stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags, false); }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<bool>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(bool);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAll(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<bool?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(bool?);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAll(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll<TFmt>(string fieldName, IReadOnlyList<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TFmt?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TFmt);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAll(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TFmtStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TFmtStruct?);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAll(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TCloaked, TRevealBase>
    (string fieldName, IReadOnlyList<TCloaked>? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TCloaked>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCloaked);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAll(value, palantírReveal, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TCloakedStruct>
    (string fieldName, IReadOnlyList<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TCloakedStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCloakedStruct?);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAll(value, palantírReveal, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null
                                          , formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TBearer>(string fieldName
      , IReadOnlyList<TBearer>? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TBearer>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TBearer);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAll(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TBearerStruct>(string fieldName
      , IReadOnlyList<TBearerStruct?>? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TBearerStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TBearerStruct?);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAll(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null
                                          , formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<string?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(string);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAll(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllCharSeq<TCharSeq>(string fieldName, IReadOnlyList<TCharSeq>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TCharSeq>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCharSeq);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllCharSeq(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<StringBuilder?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(StringBuilder);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAll(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllMatch<TAny>(string fieldName, IReadOnlyList<TAny>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TAny>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TAny);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllMatch(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObject(string fieldName, IReadOnlyList<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddAllMatch(fieldName, value, formatString, formatFlags);

    public TExt AlwaysAddAllEnumerate<TEnumbl>(string fieldName, TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : IEnumerable<bool>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(bool);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerate(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerateNullable<TEnumbl>(string fieldName, TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : IEnumerable<bool?>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<bool?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(bool?);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerateNullable(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerate<TEnumbl, TFmt>(string fieldName, TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmt>?
        where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TFmt>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TFmt);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerate<TEnumbl, TFmt>(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerateNullable<TEnumbl, TFmtStruct>(string fieldName, TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TFmtStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TFmtStruct?);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerateNullable<TEnumbl, TFmtStruct>(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>
    (string fieldName, TEnumbl value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloaked>?
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TCloaked>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCloaked);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(value, palantírReveal, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllEnumerateNullable<TEnumbl, TCloakedStruct>
    (string fieldName, TEnumbl value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloakedStruct?>?
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TCloakedStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCloakedStruct?);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllEnumerateNullable(value, palantírReveal, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null
                                          , formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllEnumerate<TEnumbl, TBearer>(string fieldName, TEnumbl value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearer>?
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TBearer>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TBearer);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllEnumerate<TEnumbl, TBearer>(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllEnumerateNullable<TEnumbl, TBearerStruct>(string fieldName, TEnumbl value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearerStruct?>?
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TBearerStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TBearerStruct?);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllEnumerateNullable<TEnumbl, TBearerStruct>(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllStringEnumerate<TEnumbl>(string fieldName, TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<string?>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<string?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(string);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllStringEnumerate(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllCharSeqEnumerate<TEnumbl, TCharSeq>(string fieldName, TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCharSeq>?
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TCharSeq>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCharSeq);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllCharSeqEnumerate<TEnumbl, TCharSeq>(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllStringBuilderEnumerate<TEnumbl>(string fieldName, TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<StringBuilder?>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<StringBuilder>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(StringBuilder);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllStringBuilderEnumerate(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllMatchEnumerate<TEnumbl, TAny>(string fieldName, TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TAny>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TAny>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TAny);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllMatchEnumerate<TEnumbl, TAny>(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObjectEnumerate<TEnumbl>(string fieldName, TEnumbl value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<object?>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<object?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(object);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllMatchEnumerate<TEnumbl, object?>(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllIterate<TEnumtr>(string fieldName, TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<bool>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<bool>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(bool);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterate(value, hasValue, formatString, formatFlags)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllIterateNullable<TEnumtr>(string fieldName, TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<bool?>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<bool?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(bool?);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateNullable(value, hasValue, formatString, formatFlags)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllIterate<TEnumtr, TFmt>(string fieldName, TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TFmt>?
        where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TFmt>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TFmt);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterate<TEnumtr, TFmt>(value, hasValue, formatString, formatFlags)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllIterateNullable<TEnumtr, TFmtStruct>(string fieldName, TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TFmtStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TFmtStruct?);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateNullable<TEnumtr, TFmtStruct>(value, hasValue, formatString, formatFlags)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllIterate<TEnumtr, TCloaked, TRevealBase>
    (string fieldName, TEnumtr value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCloaked>? 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCloaked?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCloaked);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllIterate<TEnumtr, TCloaked, TRevealBase>(value, palantírReveal, hasValue, formatString, formatFlags)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllIterateNullable<TEnumtr, TCloakedStruct>
    (string fieldName, TEnumtr value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCloakedStruct?>? 
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCloakedStruct?);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllIterateNullable(value, palantírReveal, hasValue, formatString, formatFlags)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllIterate<TEnumtr, TBearer>(string fieldName, TEnumtr value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TBearer>? 
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TBearer?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TBearer);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllIterate<TEnumtr, TBearer>(value, hasValue, formatString, formatFlags)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllIterateNullable<TEnumtr, TBearerStruct>(string fieldName, TEnumtr value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TBearerStruct?>? 
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TBearerStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TBearerStruct?);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllIterateNullable<TEnumtr, TBearerStruct>(value, hasValue, formatString, formatFlags)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllStringIterate<TEnumtr>(string fieldName, TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<string?>? 
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<string?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(string);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllStringIterate(value, hasValue, formatString, formatFlags)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllCharSeqIterate<TEnumtr, TCharSeq>(string fieldName, TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCharSeq>? 
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCharSeq>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCharSeq);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllCharSeqIterate<TEnumtr, TCharSeq>(value, hasValue, formatString, formatFlags)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllStringBuilderIterate<TEnumtr>(string fieldName, TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<StringBuilder?>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<StringBuilder?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(StringBuilder);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllStringBuilderIterate(value, hasValue, formatString, formatFlags)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllMatchIterate<TEnumtr, TAny>(string fieldName, TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TAny>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TAny>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TAny);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllMatchIterate<TEnumtr, TAny>(value, hasValue, formatString, formatFlags)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObjectIterate<TEnumtr>(string fieldName, TEnumtr value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<object?>?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<object?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(object);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllMatchIterate<TEnumtr, object?>(value, hasValue, formatString, formatFlags)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }
}
