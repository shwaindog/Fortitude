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
        var collectionType = typeof(Span<bool>);
        var elementType    = typeof(bool);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<bool>(collectionType, formatFlags).AddAll(value, formatString, formatFlags).Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, Span<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<bool?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<bool?>);
        var elementType    = typeof(bool?);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionTypeOfNullable<bool>(collectionType, formatFlags).AddAll(value, formatString, formatFlags).Complete();
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
        var collectionType = typeof(Span<TFmt>);
        var elementType    = typeof(TFmt);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<TFmt>(collectionType, formatFlags).AddAll(value, formatString, formatFlags).Complete();
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
        var collectionType = typeof(Span<TFmtStruct?>);
        var elementType    = typeof(TFmtStruct?);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionTypeOfNullable<TFmtStruct>(collectionType, formatFlags).AddAll(value, formatString, formatFlags).Complete();
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
        var collectionType = typeof(Span<TRevealBase>);
        var elementType    = typeof(TRevealBase);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, 0, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<TCloaked>(collectionType, formatFlags).RevealAll(value, palantírReveal, formatString, formatFlags)
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
        var collectionType = typeof(Span<TCloakedStruct?>);
        var elementType    = typeof(TCloakedStruct?);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionTypeOfNullable<TCloakedStruct>(collectionType, formatFlags)
           .RevealAll(value, palantírReveal, formatString, formatFlags).Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TBearer>(ReadOnlySpan<char> fieldName, Span<TBearer> value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = typeof(Span<TBearer>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TBearer>);
        var elementType    = typeof(TBearer);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<TBearer>(collectionType, formatFlags).RevealAll(value, formatString, formatFlags).Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, Span<TBearerStruct?> value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(Span<TBearerStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TBearerStruct?>);
        var elementType    = typeof(TBearerStruct?);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionTypeOfNullable<TBearerStruct>(collectionType, formatFlags).RevealAll(value, formatString, formatFlags)
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
        var collectionType = typeof(Span<string>);
        var elementType    = typeof(string);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<string>(collectionType, formatFlags).AddAll(value, formatString, formatFlags).Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllNullable(ReadOnlySpan<char> fieldName, Span<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<string?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<string>);
        var elementType    = typeof(string);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<string?>(collectionType, formatFlags).AddAllNullable(value, formatString, formatFlags).Complete();
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
        var collectionType = typeof(Span<TCharSeq>);
        var elementType    = typeof(TCharSeq);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<TCharSeq>(collectionType, formatFlags).AddAllCharSeq(value, formatString, formatFlags).Complete();
        return stb.AddGoToNext(true);
    }


    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, Span<StringBuilder> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<StringBuilder>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<StringBuilder>);
        var elementType    = typeof(StringBuilder);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<StringBuilder>(collectionType, formatFlags).AddAll(value, formatString, formatFlags).Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllNullable(ReadOnlySpan<char> fieldName, Span<StringBuilder?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<StringBuilder?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<StringBuilder>);
        var elementType    = typeof(StringBuilder);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<StringBuilder?>(collectionType, formatFlags).AddAllNullable(value, formatString, formatFlags)
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
        var collectionType = typeof(Span<TAny>);
        var elementType    = typeof(TAny);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<TAny>(collectionType, formatFlags).AddAllMatch(value, formatString, formatFlags).Complete();
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
        var collectionType = typeof(Span<object>);
        var elementType    = typeof(object);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<object>(collectionType, formatFlags).AddAllObject(value, formatString, formatFlags).Complete();
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
        var collectionType = typeof(Span<object>);
        var elementType    = typeof(object);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<object?>(collectionType, formatFlags).AddAllObjectNullable(value, formatString, formatFlags)
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
        var collectionType = typeof(ReadOnlySpan<bool>);
        var elementType    = typeof(bool);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<bool>(collectionType, formatFlags).AddAll(value, formatString, formatFlags).Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<bool?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<bool?>);
        var elementType    = typeof(bool?);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionTypeOfNullable<bool>(collectionType, formatFlags).AddAll(value, formatString, formatFlags).Complete();
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
        var collectionType = typeof(ReadOnlySpan<TFmt>);
        var elementType    = typeof(TFmt);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<TFmt>(collectionType, formatFlags).AddAll(value, formatString, formatFlags).Complete();
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
        var collectionType = typeof(ReadOnlySpan<TFmtStruct?>);
        var elementType    = typeof(TFmtStruct?);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionTypeOfNullable<TFmtStruct>(collectionType, formatFlags)
           .AddAll(value, formatString, formatFlags).Complete();
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
        var collectionType = typeof(ReadOnlySpan<TRevealBase>);
        var elementType    = typeof(TRevealBase);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<TCloaked>(collectionType, formatFlags)
           .RevealAll(value, palantírReveal, formatString, formatFlags).Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TCloakedStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloakedStruct?> value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = typeof(ReadOnlySpan<TCloakedStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TCloakedStruct?>);
        var elementType    = typeof(TCloakedStruct?);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionTypeOfNullable<TCloakedStruct>(collectionType, formatFlags)
           .RevealAll(value, palantírReveal, formatString, formatFlags).Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TBearer>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer> value, string? formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = typeof(ReadOnlySpan<TBearer>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TBearer>);
        var elementType    = typeof(TBearer);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<TBearer>(collectionType, formatFlags).RevealAll(value, formatString, formatFlags).Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TBearerStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearerStruct?> value, string? formatString
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(ReadOnlySpan<TBearerStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName
                                , formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TBearerStruct?>);
        var elementType    = typeof(TBearerStruct?);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionTypeOfNullable<TBearerStruct>(collectionType, formatFlags)
           .RevealAll(value, formatString, formatFlags).Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll(ReadOnlySpan<char> fieldName, ReadOnlySpan<string> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<string>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<string>);
        var elementType    = typeof(string);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<string>(collectionType, formatFlags).AddAll(value, formatString, formatFlags).Complete();
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<string?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<string?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<string>);
        var elementType    = typeof(string);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<string?>(collectionType, formatFlags).AddAllNullable(value, formatString, formatFlags).Complete();
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
        var collectionType = typeof(ReadOnlySpan<TCharSeq>);
        var elementType    = typeof(TCharSeq);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<TCharSeq>(collectionType, formatFlags).AddAllCharSeq(value, formatString, formatFlags).Complete();
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
        var collectionType = typeof(ReadOnlySpan<StringBuilder>);
        var elementType    = typeof(StringBuilder);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<StringBuilder>(collectionType, formatFlags).AddAll(value, formatString, formatFlags).Complete();
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
        var collectionType = typeof(ReadOnlySpan<StringBuilder>);
        var elementType    = typeof(StringBuilder);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, null, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<StringBuilder?>(collectionType, formatFlags).AddAllNullable(value, formatString, formatFlags)
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
        var collectionType = typeof(ReadOnlySpan<TAny>);
        var elementType    = typeof(TAny);
        if (value.Length == 0)
        {
            return stb.AppendEmptyCollectionOrNullAndGoToNext(elementType, collectionType, 0, 0, formatString, formatFlags, false);
        }
        stb.Master.StartExplicitCollectionType<TAny>(collectionType, formatFlags).AddAllMatch(value, formatString, formatFlags).Complete();
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

        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value).AddAll(value, formatString, formatFlags).Complete();
        else { stb.AppendEmptyCollectionOrNull(typeof(bool), value?.GetType() ?? typeof(bool[]), null, null, formatString, formatFlags, false); }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll(string fieldName, bool?[]? value, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(bool?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAll(value, formatString, formatFlags).Complete();
        else { stb.AppendEmptyCollectionOrNull(typeof(bool?), value?.GetType() ?? typeof(bool?[]), null, null, formatString, formatFlags, false); }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll<TFmt>(string fieldName, TFmt[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(TFmt[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var collectionType = stb.Master.StartSimpleCollectionType(value, null, formatFlags | AsCollection);
            collectionType.AddAll(value, formatString, formatFlags);
            collectionType.Complete();
        }
        else { stb.AppendEmptyCollectionOrNull(typeof(TFmt), value?.GetType() ?? typeof(TFmt[]), null, null, formatString, formatFlags, false); }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll<TFmtStruct>(string fieldName, TFmtStruct?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(TFmtStruct?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TFmtStruct?), value?.GetType() ?? typeof(TFmtStruct?[]), null, null, formatString, formatFlags
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
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, null, formatFlags)
               .RevealAll(value, palantírReveal, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TCloaked), value?.GetType() ?? typeof(TCloaked[]), null, null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TCloakedStruct>
    (string fieldName, TCloakedStruct?[]? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(TCloakedStruct?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).RevealAll(value, palantírReveal, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TCloakedStruct?), value?.GetType() ?? typeof(TCloakedStruct?[]), null, null, formatString
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
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).RevealAll(value, formatString, formatFlags).Complete();
        else { stb.AppendEmptyCollectionOrNull(typeof(string), value?.GetType() ?? typeof(TBearer[]), null, null, formatString, formatFlags, false); }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TBearerStruct>(string fieldName, TBearerStruct?[]? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(TBearerStruct?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).RevealAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(string), value?.GetType() ?? typeof(TBearerStruct?[]), null, null, formatString, formatFlags
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
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAll(value, formatString, formatFlags).Complete();
        else { stb.AppendEmptyCollectionOrNull(typeof(string), value?.GetType() ?? typeof(string[]), null, null, formatString, formatFlags, false); }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllCharSeq<TCharSeq>(string fieldName, TCharSeq?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllCharSeq(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TCharSeq?), value?.GetType() ?? typeof(TCharSeq[]), null, null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll(string fieldName, StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(StringBuilder?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(StringBuilder), value?.GetType() ?? typeof(StringBuilder[]), null, null, formatString, formatFlags
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
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllMatch(value, formatString, formatFlags).Complete();
        else { stb.AppendEmptyCollectionOrNull(typeof(TAny), value?.GetType() ?? typeof(TAny[]), null, null, formatString, formatFlags, false); }
        return stb.AddGoToNext(true);
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObject(string fieldName, object?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(object?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllMatch(value, formatString, formatFlags).Complete();
        else { stb.AppendEmptyCollectionOrNull(typeof(object), value?.GetType() ?? typeof(object[]), null, null, formatString, formatFlags, false); }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<bool>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(bool), value?.GetType() ?? typeof(IReadOnlyList<bool>), null, null, formatString, formatFlags
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
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(bool?), value?.GetType() ?? typeof(IReadOnlyList<bool?>), null, null, formatString, formatFlags
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
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TFmt?), value?.GetType() ?? typeof(IReadOnlyList<TFmt?>), null, null, formatString, formatFlags
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
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TFmtStruct?), value?.GetType() ?? typeof(IReadOnlyList<TFmtStruct?>), null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TCloaked, TRevealBase>
    (string fieldName, IReadOnlyList<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TCloaked?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).RevealAll(value, palantírReveal, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TRevealBase), value?.GetType() ?? typeof(IReadOnlyList<TRevealBase>), null, null, formatString
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
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).RevealAll(value, palantírReveal, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TCloakedStruct), value?.GetType() ?? typeof(IReadOnlyList<TCloakedStruct>), null, null
                                          , formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAll<TBearer>(string fieldName
      , IReadOnlyList<TBearer?>? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TBearer?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).RevealAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TBearer?), value?.GetType() ?? typeof(IReadOnlyList<TBearer?>), null, null, formatString
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
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).RevealAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TBearerStruct?), value?.GetType() ?? typeof(IReadOnlyList<TBearerStruct?>), null, null
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
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(string), value?.GetType() ?? typeof(IReadOnlyList<string>), null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllCharSeq<TCharSeq>(string fieldName, IReadOnlyList<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TCharSeq?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllCharSeq(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TCharSeq?), value?.GetType() ?? typeof(IReadOnlyList<TCharSeq?>), null, null, formatString
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
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAll(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(StringBuilder), value?.GetType() ?? typeof(IReadOnlyList<StringBuilder>), null, null, formatString
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
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllMatch(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TAny), value?.GetType() ?? typeof(IReadOnlyList<TAny>), null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObject(string fieldName, IReadOnlyList<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddAllMatch(fieldName, value, formatString, formatFlags);

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerable<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<bool>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllEnumerate(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(bool), value?.GetType() ?? typeof(IEnumerable<bool>), null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerable<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<bool?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllEnumerate(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(bool?), value?.GetType() ?? typeof(IEnumerable<bool?>), null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerate<TFmt>(string fieldName, IEnumerable<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TFmt?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllEnumerate(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TFmt?), value?.GetType() ?? typeof(IEnumerable<TFmt?>), null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerable<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TFmtStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllEnumerate(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TFmtStruct?), value?.GetType() ?? typeof(IEnumerable<TFmtStruct?>), null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllEnumerate<TCloaked, TRevealBase>
    (string fieldName, IEnumerable<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TCloaked?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).RevealAllEnumerate(value, palantírReveal, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TRevealBase), value?.GetType() ?? typeof(IEnumerable<TRevealBase>), null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllEnumerate<TCloakedStruct>
    (string fieldName, IEnumerable<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TCloakedStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).RevealAllEnumerate(value, palantírReveal, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TCloakedStruct?), value?.GetType() ?? typeof(IEnumerable<TCloakedStruct?>), null, null
                                          , formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllEnumerate<TBearer>(string fieldName, IEnumerable<TBearer?>? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TBearer?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).RevealAllEnumerate(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TBearer?), value?.GetType() ?? typeof(IEnumerable<TBearer?>), null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllEnumerate<TBearerStruct>(string fieldName, IEnumerable<TBearerStruct?>? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TBearerStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).RevealAllEnumerate(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TBearerStruct?), value?.GetType() ?? typeof(IEnumerable<TBearerStruct?>), null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerable<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<string?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllEnumerate(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(string), value?.GetType() ?? typeof(IEnumerable<string>), null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllCharSeqEnumerate<TCharSeq>(string fieldName, IEnumerable<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TCharSeq?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllCharSeqEnumerate(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TCharSeq), value?.GetType() ?? typeof(IEnumerable<TCharSeq>), null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<StringBuilder?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllEnumerate(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(StringBuilder), value?.GetType() ?? typeof(IEnumerable<StringBuilder>), null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllMatchEnumerate<TAny>(string fieldName, IEnumerable<TAny>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TAny>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllMatchEnumerate(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TAny), value?.GetType() ?? typeof(IEnumerable<TAny>), null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObjectEnumerate(string fieldName, IEnumerable<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<object?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllMatchEnumerate(value, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(bool), value?.GetType() ?? typeof(IEnumerable<object?>), null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerator<bool>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<bool>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllEnumerate(value, formatString, formatFlags).Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(bool), value?.GetType() ?? typeof(IEnumerator<bool>), value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerator<bool?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<bool?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllEnumerate(value, formatString, formatFlags).Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(bool?), value?.GetType() ?? typeof(IEnumerator<bool?>), value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerate<TFmt>(string fieldName, IEnumerator<TFmt?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TFmt?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllEnumerate(value, formatString, formatFlags).Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TFmt?), value?.GetType() ?? typeof(IEnumerator<TFmt?>), value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerator<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TFmtStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllEnumerate(value, formatString, formatFlags).Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TFmtStruct?), value?.GetType() ?? typeof(IEnumerator<TFmtStruct?>), value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllEnumerate<TCloaked, TRevealBase>
    (string fieldName, IEnumerator<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCloaked?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).RevealAllEnumerate(value, palantírReveal, formatString, formatFlags).Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TCloaked), value?.GetType() ?? typeof(IEnumerator<TCloaked>), value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllEnumerate<TCloakedStruct>
    (string fieldName, IEnumerator<TCloakedStruct?>? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCloakedStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).RevealAllEnumerate(value, palantírReveal, formatString, formatFlags).Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TCloakedStruct), value?.GetType() ?? typeof(IEnumerator<TCloakedStruct>), value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllEnumerate<TBearer>(string fieldName, IEnumerator<TBearer?>? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TBearer?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).RevealAllEnumerate(value, formatString, formatFlags).Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TBearer), value?.GetType() ?? typeof(IEnumerator<TBearer>), value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllEnumerate<TBearerStruct>(string fieldName, IEnumerator<TBearerStruct?>? value
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TBearerStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).RevealAllEnumerate(value, formatString, formatFlags).Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TBearerStruct?), value?.GetType() ?? typeof(IEnumerator<TBearerStruct?>), value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerator<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<string?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllEnumerate(value, formatString, formatFlags).Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(string), value?.GetType() ?? typeof(IEnumerator<string?>), value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllCharSeqEnumerate<TCharSeq>(string fieldName, IEnumerator<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCharSeq?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllCharSeqEnumerate(value, formatString, formatFlags).Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TCharSeq?), value?.GetType() ?? typeof(IEnumerator<TCharSeq?>), value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerator<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<StringBuilder?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllEnumerate(value, formatString, formatFlags).Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(StringBuilder), value?.GetType() ?? typeof(IEnumerator<StringBuilder>), value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllMatchEnumerate<TAny>(string fieldName, IEnumerator<TAny>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TAny>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllMatchEnumerate(value, formatString, formatFlags).Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(TAny), value?.GetType() ?? typeof(IEnumerator<TAny>), value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllObjectEnumerate(string fieldName, IEnumerator<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<object?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            value!.Reset();
            stb.Master.StartSimpleCollectionType(value, null, formatFlags).AddAllMatchEnumerate(value, formatString, formatFlags).Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(typeof(object), value?.GetType() ?? typeof(IEnumerator<object>), value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }
}
