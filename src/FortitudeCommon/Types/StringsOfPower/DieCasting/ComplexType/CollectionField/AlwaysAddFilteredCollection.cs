using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, Span<bool> value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<bool>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<bool>);
        var elementType    = typeof(bool);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionType<bool>(collectionType, formatFlags).AddFiltered(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, Span<bool?> value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<bool?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<bool?>);
        var elementType    = typeof(bool?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionTypeOfNullable<bool>(collectionType, formatFlags).AddFiltered(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmt, TFmtBase>
    (ReadOnlySpan<char> fieldName, Span<TFmt> value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase?
    {
        var actualType = typeof(Span<TFmt>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TFmt>);
        var elementType    = typeof(TFmt);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionType<TFmt>(collectionType, formatFlags).AddFiltered(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmtStruct>
    (ReadOnlySpan<char> fieldName, Span<TFmtStruct?> value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(Span<TFmtStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TFmtStruct?>);
        var elementType    = typeof(TFmtStruct?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionTypeOfNullable<TFmtStruct>(collectionType, formatFlags).AddFiltered(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloaked, TFilterBase, TRevealBase>
    (ReadOnlySpan<char> fieldName, Span<TCloaked> value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = typeof(Span<TCloaked>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TCloaked>);
        var elementType    = typeof(TCloaked);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionType<TCloaked>(collectionType, formatFlags).RevealFiltered(value, filterPredicate, palantírReveal, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloakedStruct>(ReadOnlySpan<char> fieldName, Span<TCloakedStruct?> value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct
    {
        var actualType = typeof(Span<TCloakedStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TCloakedStruct?>);
        var elementType    = typeof(TCloakedStruct?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionTypeOfNullable<TCloakedStruct>(collectionType, formatFlags).RevealFiltered(value, filterPredicate, palantírReveal, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearer, TBearerBase>(ReadOnlySpan<char> fieldName, Span<TBearer> value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?, TBearerBase?
    {
        var actualType = typeof(Span<TBearer>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TBearer>);
        var elementType    = typeof(TBearer);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionType<TBearer>(collectionType, formatFlags).RevealFiltered(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearerStruct>(ReadOnlySpan<char> fieldName, Span<TBearerStruct?> value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer
    {
        var actualType = typeof(Span<TBearerStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TBearerStruct?>);
        var elementType    = typeof(TBearerStruct?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionTypeOfNullable<TBearerStruct>(collectionType, formatFlags).RevealFiltered(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, Span<string> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<string>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<string>);
        var elementType    = typeof(string);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionType<string>(collectionType, formatFlags).AddFiltered(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredNullable(ReadOnlySpan<char> fieldName, Span<string?> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<string?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<string>);
        var elementType    = typeof(string);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionType<string?>(collectionType, formatFlags).AddFilteredNullable(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSeq<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, Span<TCharSeq> value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?, TCharSeqBase?
    {
        var actualType = typeof(Span<TCharSeq>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TCharSeq>);
        var elementType    = typeof(TCharSeq);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionType<TCharSeq>(collectionType, formatFlags).AddFilteredCharSeq(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, Span<StringBuilder> value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<StringBuilder>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<StringBuilder>);
        var elementType    = typeof(StringBuilder);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionType<StringBuilder>(collectionType, formatFlags).AddFiltered(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredNullable(ReadOnlySpan<char> fieldName, Span<StringBuilder?> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(Span<StringBuilder?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<StringBuilder>);
        var elementType    = typeof(StringBuilder);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionType<StringBuilder?>(collectionType, formatFlags).AddFilteredNullable(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredMatch<TAny, TAnyBase>(ReadOnlySpan<char> fieldName, Span<TAny> value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase?
    {
        var actualType = typeof(Span<TAny>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(Span<TAny>);
        var elementType    = typeof(TAny);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionType<TAny>(collectionType, formatFlags).AddFilteredMatch(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObject(ReadOnlySpan<char> fieldName, Span<object> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObjectNullable(ReadOnlySpan<char> fieldName, Span<object?> value, OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool> value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<bool>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<bool>);
        var elementType    = typeof(bool);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionType<bool>(collectionType, formatFlags).AddFiltered(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<bool?> value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<bool?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<bool?>);
        var elementType    = typeof(bool?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionTypeOfNullable<bool>(collectionType, formatFlags).AddFiltered(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmt, TFmtBase>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmt> value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase?
    {
        var actualType = typeof(ReadOnlySpan<TFmt>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TFmt>);
        var elementType    = typeof(TFmt);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionType<TFmt>(collectionType, formatFlags).AddFiltered(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmtStruct>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TFmtStruct?> value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = typeof(ReadOnlySpan<TFmtStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TFmtStruct?>);
        var elementType    = typeof(TFmtStruct?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionTypeOfNullable<TFmtStruct>(collectionType, formatFlags).AddFiltered(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloaked, TFilterBase, TRevealBase>
    (ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloaked> value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = typeof(ReadOnlySpan<TCloaked>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TCloaked>);
        var elementType    = typeof(TCloaked);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionType<TCloaked>(collectionType, formatFlags).RevealFiltered(value, filterPredicate, palantírReveal, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloakedStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCloakedStruct?> value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct
    {
        var actualType = typeof(ReadOnlySpan<TCloakedStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName
                                                                 , formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TCloakedStruct?>);
        var elementType    = typeof(TCloakedStruct?);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionTypeOfNullable<TCloakedStruct>(collectionType, formatFlags).RevealFiltered(value, filterPredicate, palantírReveal, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearer, TBearerBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearer> value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearer : IStringBearer?, TBearerBase?
    {
        var actualType = typeof(ReadOnlySpan<TBearer>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TBearer>);
        var elementType    = typeof(TBearer);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionType<TBearer>(collectionType, formatFlags).RevealFiltered(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearerStruct>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TBearerStruct?> value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer
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
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, "", formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionTypeOfNullable<TBearerStruct>(collectionType, formatFlags).RevealFiltered(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<string> value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<string>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<string>);
        var elementType    = typeof(string);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionType<string>(collectionType, formatFlags).AddFiltered(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<string?> value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = typeof(ReadOnlySpan<string?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<string>);
        var elementType    = typeof(string);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionType<string?>(collectionType, formatFlags).AddFilteredNullable(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSeq<TCharSeq, TCharSeqBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TCharSeq> value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?, TCharSeqBase?
    {
        var actualType = typeof(ReadOnlySpan<TCharSeq>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TCharSeq>);
        var elementType    = typeof(TCharSeq);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionType<TCharSeq>(collectionType, formatFlags).AddFilteredCharSeq(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
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
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionType<StringBuilder>(collectionType, formatFlags).AddFiltered(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<StringBuilder?> value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
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
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionType<StringBuilder?>(collectionType, formatFlags).AddFilteredNullable(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredMatch<TAny, TAnyBase>(ReadOnlySpan<char> fieldName, ReadOnlySpan<TAny> value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase?
    {
        var actualType = typeof(ReadOnlySpan<TAny>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var collectionType = typeof(ReadOnlySpan<TAny>);
        var elementType    = typeof(TAny);
        if (value.Length == 0)
        {
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, collectionType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }

        stb.Master.StartExplicitCollectionType<TAny>(collectionType, formatFlags).AddFilteredMatch(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObject(ReadOnlySpan<char> fieldName, ReadOnlySpan<object> value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObjectNullable(ReadOnlySpan<char> fieldName, ReadOnlySpan<object?> value
      , OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt AlwaysAddFiltered(string fieldName, bool[]? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(bool[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Length ?? 0) == 0)
        {
            var elementType    = typeof(bool);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, typeof(bool[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Length, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<bool[], bool>(value!).AddFiltered(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(bool?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Length ?? 0) == 0)
        {
            var elementType    = typeof(bool?);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, typeof(bool?[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Length, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<bool?[], bool?>(value!).AddFiltered(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmt, TFmtBase>
    (string fieldName, TFmt[]? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase?
    {
        var actualType = value?.GetType() ?? typeof(TFmt[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Length ?? 0) == 0)
        {
            var elementType    = typeof(TFmt);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, typeof(TFmt[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Length, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<TFmt[], TFmt>(value!).AddFiltered(value, filterPredicate, formatString, formatFlags).Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmtStruct>
    (string fieldName, TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(TFmtStruct?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Length ?? 0) == 0)
        {
            var elementType    = typeof(TFmtStruct?);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, typeof(TFmtStruct?[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Length, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<TFmtStruct?[], TFmtStruct?>(value!)
           .AddFiltered(value, filterPredicate, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloaked, TFilterBase, TRevealBase>
    (string fieldName, TCloaked[]? value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TCloaked[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Length ?? 0) == 0)
        {
            var elementType    = typeof(TCloaked);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, typeof(TCloaked[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Length, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<TCloaked[], TCloaked>(value!)
           .RevealFiltered(value, filterPredicate, palantírReveal, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloakedStruct>(string fieldName, TCloakedStruct?[]? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(TCloakedStruct?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Length ?? 0) == 0)
        {
            var elementType    = typeof(TCloakedStruct?);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, typeof(TCloakedStruct?[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Length, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<TCloakedStruct?[], TCloakedStruct?>(value!)
           .RevealFiltered(value, filterPredicate, palantírReveal, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearer, TBearerBase>(string fieldName, TBearer[]? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?, TBearerBase?
    {
        var actualType = value?.GetType() ?? typeof(TBearer[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Length ?? 0) == 0)
        {
            var elementType    = typeof(TBearer);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, typeof(TBearer[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Length, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<TBearer[], TBearer>(value!)
           .RevealFiltered(value, filterPredicate, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearerStruct>(string fieldName, TBearerStruct?[]? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(TBearerStruct?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Length ?? 0) == 0)
        {
            var elementType    = typeof(TBearerStruct?);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, typeof(TBearerStruct?[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Length, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<TBearerStruct?[], TBearerStruct?>(value!)
           .RevealFiltered(value, filterPredicate, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered
    (string fieldName
      , string?[]? value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(string?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Length ?? 0) == 0)
        {
            var elementType    = typeof(string);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, typeof(string?[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Length, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<string?[], string?>(value!)
           .AddFiltered(value, filterPredicate, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSeq<TCharSeq, TCharSeqBase>
    (string fieldName, TCharSeq[]? value, OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?, TCharSeqBase?
    {
        var actualType = value?.GetType() ?? typeof(TCharSeq?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Length ?? 0) == 0)
        {
            var elementType    = typeof(TCharSeq);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, typeof(TCharSeq[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Length, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<TCharSeq[], TCharSeq>(value!)
           .AddFilteredCharSeq(value, filterPredicate, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(StringBuilder?[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Length ?? 0) == 0)
        {
            var elementType    = typeof(StringBuilder);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, typeof(StringBuilder?[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Length, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<StringBuilder?[], StringBuilder>(value!)
           .AddFiltered(value, filterPredicate, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredMatch<TAny, TAnyBase>(string fieldName, TAny[]? value, OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase?
    {
        var actualType = value?.GetType() ?? typeof(TAny[]);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Length ?? 0) == 0)
        {
            var elementType    = typeof(TAny);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, typeof(TAny[]), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Length, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<TAny[], TAny>(value!)
           .AddFilteredMatch(value, filterPredicate, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObject(string fieldName, object?[]? value, OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<bool>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Count ?? 0) == 0)
        {
            var elementType    = typeof(bool);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, actualType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Count, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<bool>(value!, formatFlags)
           .AddFiltered(value, filterPredicate, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<bool?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Count ?? 0) == 0)
        {
            var elementType    = typeof(bool?);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, actualType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Count, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<bool?>(value!, formatFlags)
           .AddFiltered(value, filterPredicate, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmt, TFmtBase>(string fieldName, IReadOnlyList<TFmt>? value, OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase?
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TFmt>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Count ?? 0) == 0)
        {
            var elementType    = typeof(TFmt);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, actualType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Count, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<TFmt>(value!, formatFlags)
           .AddFiltered(value, filterPredicate, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TFmtStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Count ?? 0) == 0)
        {
            var elementType    = typeof(TFmtStruct?);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, actualType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Count, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<TFmtStruct?>(value!, formatFlags)
           .AddFiltered(value, filterPredicate, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloaked, TFilterBase, TRevealBase>
    (string fieldName, IReadOnlyList<TCloaked>? value, OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TCloaked>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Count ?? 0) == 0)
        {
            var elementType    = typeof(TCloaked);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, actualType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Count, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<TCloaked>(value!, formatFlags)
           .RevealFiltered(value, filterPredicate, palantírReveal, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TCloakedStruct>
    (string fieldName, IReadOnlyList<TCloakedStruct?>? value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TCloakedStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Count ?? 0) == 0)
        {
            var elementType    = typeof(TCloakedStruct?);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, actualType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Count, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<TCloakedStruct?>(value!, formatFlags)
           .RevealFiltered(value, filterPredicate, palantírReveal, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearer, TBearerBase>(string fieldName, IReadOnlyList<TBearer?>? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer, TBearerBase
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TBearer?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Count ?? 0) == 0)
        {
            var elementType    = typeof(TBearer);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, actualType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Count, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<TBearer>(value!, formatFlags)
           .RevealFiltered(value, filterPredicate, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFiltered<TBearerStruct>(string fieldName, IReadOnlyList<TBearerStruct?>? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TBearerStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Count ?? 0) == 0)
        {
            var elementType    = typeof(TBearerStruct?);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, actualType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Count, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<TBearerStruct?>(value!, formatFlags)
           .RevealFiltered(value, filterPredicate, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<string?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Count ?? 0) == 0)
        {
            var elementType    = typeof(string);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, actualType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Count, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<string?>(value!, formatFlags)
           .AddFiltered(value, filterPredicate, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSeq<TCharSeq, TCharSeqBase>(string fieldName, IReadOnlyList<TCharSeq>? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence?, TCharSeqBase?
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TCharSeq?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Count ?? 0) == 0)
        {
            var elementType    = typeof(TCharSeq);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, actualType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Count, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<TCharSeq>(value!, formatFlags)
           .AddFilteredCharSeq(value, filterPredicate, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<StringBuilder?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Count ?? 0) == 0)
        {
            var elementType    = typeof(StringBuilder);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, actualType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Count, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<StringBuilder?>(value!, formatFlags)
           .AddFiltered(value, filterPredicate, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredMatch<TAny, TAnyBase>(string fieldName
      , IReadOnlyList<TAny>? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase?
    {
        var actualType = value?.GetType() ?? typeof(IReadOnlyList<TAny>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if ((value?.Count ?? 0) == 0)
        {
            var elementType    = typeof(TAny);
            stb.StyleFormatter.FormatCollectionStart(stb, elementType, false, actualType, formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, value?.Count, elementType, 0, formatString, formatFlags);
            return stb.AddGoToNext();
        }
        
        stb.Master.StartExplicitCollectionType<TAny>(value!, formatFlags)
           .AddFilteredMatch(value, filterPredicate, formatString, formatFlags)
           .Complete();
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObject(string fieldName
      , IReadOnlyList<object?>? value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) =>
        AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString, formatFlags);

    public TExt AlwaysAddFilteredEnumerate(string fieldName, IEnumerable<bool>? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<bool>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(bool), null, value?.GetType() ?? typeof(IEnumerable<bool>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(bool), null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate(string fieldName
      , IEnumerable<bool?>? value
      , OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<bool?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(bool?), null, value?.GetType() ?? typeof(IEnumerable<bool?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(bool?), null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TFmt, TFmtBase>(string fieldName
      , IEnumerable<TFmt?>? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable, TFmtBase
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TFmt?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TFmt), null, value?.GetType() ?? typeof(IEnumerable<TFmt>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TFmt), null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TFmtStruct>(string fieldName
      , IEnumerable<TFmtStruct?>? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TFmtStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TFmtStruct?), null, value?.GetType() ?? typeof(IEnumerable<TFmtStruct?>)
                                                   , formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TFmtStruct?), null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredEnumerate<TCloaked, TFilterBase, TRevealBase>
    (string fieldName
      , IEnumerable<TCloaked?>? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TCloaked?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .RevealFilteredEnumerate(value, filterPredicate, palantírReveal, formatString, formatFlags)
               .Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TCloaked), null, value?.GetType() ?? typeof(IEnumerable<TCloaked?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TCloaked), null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredEnumerate<TCloakedStruct>
    (string fieldName
      , IEnumerable<TCloakedStruct?>? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TCloakedStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .RevealFilteredEnumerate(value, filterPredicate, palantírReveal, formatString, formatFlags)
               .Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TCloakedStruct), null, value?.GetType() ?? typeof(IEnumerable<TCloakedStruct?>)
                                                   , formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TCloakedStruct), null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredEnumerate<TBearer, TBearerBase>(string fieldName
      , IEnumerable<TBearer>? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer?, TBearerBase?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TBearer>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .RevealFilteredEnumerate(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TBearer), null, value?.GetType() ?? typeof(IEnumerable<TBearer?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TBearer), null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredEnumerate<TBearerStruct>(string fieldName
      , IEnumerable<TBearerStruct?>? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TBearerStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .RevealFilteredEnumerate(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TBearerStruct?), null, value?.GetType() ?? typeof(IEnumerable<TBearerStruct?>)
                                                   , formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TBearerStruct?), null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate(string fieldName
      , IEnumerable<string?>? value
      , OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<string?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(string), null, value?.GetType() ?? typeof(IEnumerable<string>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(string), null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSeqEnumerate<TCharSeq, TCharSeqBase>(string fieldName
      , IEnumerable<TCharSeq?>? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence, TCharSeqBase
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TCharSeq?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .AddFilteredCharSeqEnumerate(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TCharSeq), null, value?.GetType() ?? typeof(IEnumerable<TCharSeq>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TCharSeq), null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate(string fieldName
      , IEnumerable<StringBuilder?>? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<StringBuilder?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(StringBuilder), null, value?.GetType() ?? typeof(IEnumerable<StringBuilder>)
                                                   , formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(StringBuilder), null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredMatchEnumerate<TAny, TAnyBase>(string fieldName
      , IEnumerable<TAny?>? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<TAny>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .AddFilteredMatchEnumerate(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TAny?), null, value?.GetType() ?? typeof(IEnumerable<TAny?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TAny?), null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObjectEnumerate(string fieldName
      , IEnumerable<object?>? value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerable<object?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .AddFilteredObjectEnumerate(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(object), null, value?.GetType() ?? typeof(IEnumerable<object>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(object), null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate(string fieldName
      , IEnumerator<bool>? value
      , OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<bool>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(bool), null, value?.GetType() ?? typeof(IEnumerator<bool>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(bool), null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate(string fieldName
      , IEnumerator<bool?>? value
      , OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<bool?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(bool?), null, value?.GetType() ?? typeof(IEnumerator<bool?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(bool?), null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TFmt, TFmtBase>(string fieldName
      , IEnumerator<TFmt?>? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmt : ISpanFormattable?, TFmtBase?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TFmt?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TFmt), null, value?.GetType() ?? typeof(IEnumerator<TFmt>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TFmt), null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TFmtStruct>(string fieldName
      , IEnumerator<TFmtStruct?>? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TFmtStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TFmtStruct?), null, value?.GetType() ?? typeof(IEnumerator<TFmtStruct?>)
                                                   , formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TFmtStruct?), null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredEnumerate<TCloaked, TFilterBase, TRevealBase>
    (string fieldName, IEnumerator<TCloaked?>? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate
      , PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCloaked?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .RevealFilteredEnumerate(value, filterPredicate, palantírReveal, formatString, formatFlags)
               .Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TCloaked), null, value?.GetType() ?? typeof(IEnumerator<TCloaked>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TCloaked), null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredEnumerate<TCloakedStruct>
    (string fieldName, IEnumerator<TCloakedStruct?>? value, OrderedCollectionPredicate<TCloakedStruct?> filterPredicate
      , PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCloakedStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .RevealFilteredEnumerate(value, filterPredicate, palantírReveal, formatString, formatFlags)
               .Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TCloakedStruct?), null, value?.GetType() ?? typeof(IEnumerator<TCloakedStruct?>)
                                                   , formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TCloakedStruct?), null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredEnumerate<TBearer, TBearerBase>(string fieldName, IEnumerator<TBearer?>? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearer : IStringBearer, TBearerBase
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TBearer?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .RevealFilteredEnumerate(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TBearer), null, value?.GetType() ?? typeof(IEnumerator<TBearer>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TBearer), null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysRevealFilteredEnumerate<TBearerStruct>(string fieldName, IEnumerator<TBearerStruct?>? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TBearerStruct?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .RevealFilteredEnumerate(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TBearerStruct?), null, value?.GetType() ?? typeof(IEnumerator<TBearerStruct?>)
                                                   , formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TBearerStruct?), null, "", formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate(string fieldName, IEnumerator<string?>? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<string?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(string), null, value?.GetType() ?? typeof(IEnumerator<string?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(string), null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSeqEnumerate<TCharSeq, TCharSeqBase>(string fieldName, IEnumerator<TCharSeq?>? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TCharSeq : ICharSequence, TCharSeqBase
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TCharSeq?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .AddFilteredCharSeqEnumerate(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TCharSeq), null, value?.GetType() ?? typeof(IEnumerator<TCharSeq?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TCharSeq), null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate(string fieldName, IEnumerator<StringBuilder?>? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<StringBuilder?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(StringBuilder), null, value?.GetType()
                                                                                    ?? typeof(IEnumerator<StringBuilder>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(StringBuilder), null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredMatchEnumerate<TAny, TAnyBase>(string fieldName, IEnumerator<TAny?>? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TAny : TAnyBase?
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<TAny>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .AddFilteredMatchEnumerate(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(TAny), null, value?.GetType() ?? typeof(IEnumerator<TAny?>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(TAny), null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredObjectEnumerate(string fieldName, IEnumerator<object?>? value
      , OrderedCollectionPredicate<object> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
    {
        var actualType = value?.GetType() ?? typeof(IEnumerator<object?>);
        if (stb.HasSkipField(actualType, fieldName, formatFlags))
            return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .AddFilteredObjectEnumerate(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.StyleFormatter.FormatCollectionStart(stb, typeof(object), null, value?.GetType() ?? typeof(IEnumerator<object>), formatFlags);
            stb.StyleFormatter.FormatCollectionEnd(stb, null, typeof(object), null, formatString, formatFlags);
        }
        return stb.AddGoToNext();
    }
}
