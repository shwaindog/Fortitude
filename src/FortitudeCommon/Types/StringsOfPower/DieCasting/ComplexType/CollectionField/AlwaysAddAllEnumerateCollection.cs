// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    
    public TExt AlwaysAddAllEnumerateBool<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<bool>
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(bool);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerateBool(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerateBool<TEnumbl>(string fieldName, TEnumbl? value
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
               .AddAllEnumerateBool(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerateNullableBool<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : struct, IEnumerable<bool?>
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(bool?);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerateNullableBool(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerateNullableBool<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags) 
        where TEnumbl : IEnumerable<bool?>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(bool?);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerateNullableBool(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerate<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerate(value, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(ISpanFormattable);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerate<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerate(value, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(ISpanFormattable);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerate<TEnumbl, TFmt>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TFmt>
        where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
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

    public TExt AlwaysAddAllEnumerate<TEnumbl, TFmt>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmt>?
        where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
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

    public TExt AlwaysAddAllEnumerateNullable<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerateNullable(value, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(ISpanFormattable);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerateNullable<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerateNullable(value, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(ISpanFormattable);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerateNullable<TEnumbl, TFmtStruct>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TFmtStruct?>
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
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

    public TExt AlwaysAddAllEnumerateNullable<TEnumbl, TFmtStruct>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
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

    public TExt AlwaysRevealAllEnumerate<TEnumbl, TRevealBase>(string fieldName, TEnumbl? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllEnumerate(value, palantírReveal, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(TRevealBase);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllEnumerate<TEnumbl, TRevealBase>(string fieldName, TEnumbl? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllEnumerate(value, palantírReveal, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(TRevealBase);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(string fieldName, TEnumbl? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCloaked>
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
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

    public TExt AlwaysRevealAllEnumerate<TEnumbl, TCloaked, TRevealBase>(string fieldName, TEnumbl? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloaked>?
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
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

    public TExt AlwaysRevealAllEnumerateNullable<TEnumbl, TCloakedStruct>(string fieldName, TEnumbl? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCloakedStruct?>
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
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

    public TExt AlwaysRevealAllEnumerateNullable<TEnumbl, TCloakedStruct>(string fieldName, TEnumbl? value
      , PalantírReveal<TCloakedStruct> palantírReveal, string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloakedStruct?>?
        where TCloakedStruct : struct
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
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

    public TExt AlwaysRevealAllEnumerate<TEnumbl>(string fieldName, TEnumbl? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllEnumerate(value, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(IStringBearer);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllEnumerate<TEnumbl>(string fieldName, TEnumbl? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllEnumerate(value, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(IStringBearer);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllEnumerate<TEnumbl, TBearer>(string fieldName, TEnumbl? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TBearer>
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
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

    public TExt AlwaysRevealAllEnumerate<TEnumbl, TBearer>(string fieldName, TEnumbl? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearer>?
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
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

    public TExt AlwaysRevealAllEnumerateNullable<TEnumbl>(string fieldName, TEnumbl? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllEnumerateNullable(value, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(IStringBearer);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllEnumerateNullable<TEnumbl>(string fieldName, TEnumbl? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllEnumerateNullable(value, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(IStringBearer);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealAllEnumerateNullable<TEnumbl, TBearerStruct>(string fieldName, TEnumbl? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TBearerStruct?>
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
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

    public TExt AlwaysRevealAllEnumerateNullable<TEnumbl, TBearerStruct>(string fieldName, TEnumbl? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearerStruct?>?
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
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

    public TExt AlwaysAddAllStringEnumerateString<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<string?>
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(string);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerateString(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllStringEnumerateString<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<string?>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(string);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerateString(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerateCharSeq<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerateCharSeq(value, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(ICharSequence);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerateCharSeq<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerateCharSeq(value, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(ICharSequence);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerateCharSeq<TEnumbl, TCharSeq>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCharSeq>
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCharSeq);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerateCharSeq<TEnumbl, TCharSeq>(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerateCharSeq<TEnumbl, TCharSeq>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCharSeq>?
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCharSeq);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerateCharSeq<TEnumbl, TCharSeq>(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerateStringBuilder<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<StringBuilder?>
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(StringBuilder);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerateStringBuilder(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerateStringBuilder<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<StringBuilder?>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(StringBuilder);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerateStringBuilder(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerateMatch<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerateMatch(value, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(object);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerateMatch<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerateMatch(value, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(object);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerateMatch<TEnumbl, TAny>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TAny>
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TAny);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerateMatch<TEnumbl, TAny>(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddAllEnumerateMatch<TEnumbl, TAny>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TAny>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TAny);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerateMatch<TEnumbl, TAny>(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllEnumerateObject<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<object?>
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(object);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerateMatch<TEnumbl, object?>(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllEnumerateObject<TEnumbl>(string fieldName, TEnumbl? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<object?>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(object);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllEnumerateMatch<TEnumbl, object?>(value, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }
}
