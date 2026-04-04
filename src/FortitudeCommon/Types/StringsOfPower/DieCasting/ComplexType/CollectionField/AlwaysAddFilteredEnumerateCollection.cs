// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt AlwaysAddFilteredEnumerate<TEnumbl>(string fieldName, TEnumbl? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<bool>
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(bool);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerateBool(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }
    
    public TExt AlwaysAddFilteredEnumerate<TEnumbl>(string fieldName, TEnumbl? value, OrderedCollectionPredicate<bool> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<bool>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(bool);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master.StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerateBool(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddFilteredEnumerateNullable<TEnumbl>(string fieldName, TEnumbl? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<bool?>
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(bool?);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerateNullableBool(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddFilteredEnumerateNullable<TEnumbl>(string fieldName, TEnumbl? value, OrderedCollectionPredicate<bool?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<bool?>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(bool?);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerateNullableBool(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddFilteredEnumerate<TEnumbl, TFmtBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(TFmtBase);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddFilteredEnumerate<TEnumbl, TFmtBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerate(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(TFmtBase);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TFmt>
        where TFmt : ISpanFormattable?, TFmtBase?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TFmt);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TFmtBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TFmt>?
        where TFmt : ISpanFormattable?, TFmtBase?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TFmt);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerate<TEnumbl, TFmt, TFmtBase>(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddFilteredEnumerateNullable<TEnumbl, TFmtStruct>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
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
               .AddFilteredEnumerateNullable(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddFilteredEnumerateNullable<TEnumbl, TFmtStruct>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
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
               .AddFilteredEnumerateNullable(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealFilteredEnumerate<TEnumbl, TFilterBase, TRevealBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate, PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealFilteredEnumerate(value, filterPredicate, palantírReveal, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(TRevealBase);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealFilteredEnumerate<TEnumbl, TFilterBase, TRevealBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate, PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealFilteredEnumerate(value, filterPredicate, palantírReveal, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(TRevealBase);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate, PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCloaked>
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCloaked);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>
                   (value, filterPredicate, palantírReveal, formatString, formatFlags)
               .Complete();
        else { stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, "", formatFlags, false); }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TFilterBase> filterPredicate, PalantírReveal<TRevealBase> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCloaked>?
        where TCloaked : TFilterBase?, TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCloaked);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealFilteredEnumerate<TEnumbl, TCloaked, TFilterBase, TRevealBase>
                   (value, filterPredicate, palantírReveal, formatString, formatFlags)
               .Complete();
        else { stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, "", formatFlags, false); }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealFilteredEnumerateNullable<TEnumbl, TCloakedStruct>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
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
               .RevealFilteredEnumerateNullable(value, filterPredicate, palantírReveal, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, ""
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealFilteredEnumerateNullable<TEnumbl, TCloakedStruct>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TCloakedStruct?> filterPredicate, PalantírReveal<TCloakedStruct> palantírReveal
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
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
               .RevealFilteredEnumerateNullable(value, filterPredicate, palantírReveal, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, ""
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealFilteredEnumerate<TEnumbl, TBearerBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealFilteredEnumerate(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(IStringBearer);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealFilteredEnumerate<TEnumbl, TBearerBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealFilteredEnumerate(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(IStringBearer);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealFilteredEnumerate<TEnumbl, TBearer, TBearerBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TBearer>
        where TBearer : IStringBearer?, TBearerBase?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TBearer);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealFilteredEnumerate<TEnumbl, TBearer, TBearerBase>
                   (value, filterPredicate, formatString, formatFlags)
               .Complete();
        else { stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, "", formatFlags, false); }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealFilteredEnumerate<TEnumbl, TBearer, TBearerBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TBearerBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TBearer>?
        where TBearer : IStringBearer?, TBearerBase?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TBearer);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealFilteredEnumerate<TEnumbl, TBearer, TBearerBase>
                   (value, filterPredicate, formatString, formatFlags)
               .Complete();
        else { stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, "", formatFlags, false); }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealFilteredEnumerateNullable<TEnumbl, TBearerStruct>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
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
               .RevealFilteredEnumerateNullable(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, ""
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysRevealFilteredEnumerateNullable<TEnumbl, TBearerStruct>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TBearerStruct?> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
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
               .RevealFilteredEnumerateNullable(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, ""
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddFilteredEnumerateString<TEnumbl>(string fieldName, TEnumbl? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<string?>
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(string);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerateString(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddFilteredEnumerateString<TEnumbl>(string fieldName, TEnumbl? value, OrderedCollectionPredicate<string> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<string?>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(string);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerateString(value, filterPredicate, formatString, formatFlags).Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddFilteredEnumerateCharSeq<TEnumbl, TCharSeqBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerateCharSeq(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(ICharSequence);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddFilteredEnumerateCharSeq<TEnumbl, TCharSeqBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerateCharSeq(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(ICharSequence);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddFilteredEnumerateCharSeq<TEnumbl, TCharSeq, TCharSeqBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TCharSeq>
        where TCharSeq : ICharSequence?, TCharSeqBase?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCharSeq);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerateCharSeq<TEnumbl, TCharSeq, TCharSeqBase>(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddFilteredEnumerateCharSeq<TEnumbl, TCharSeq, TCharSeqBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TCharSeqBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TCharSeq>?
        where TCharSeq : ICharSequence?, TCharSeqBase?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCharSeq);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerateCharSeq<TEnumbl, TCharSeq, TCharSeqBase>(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddFilteredEnumerateStringBuilder<TEnumbl>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
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
               .AddFilteredEnumerateStringBuilder(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddFilteredEnumerateStringBuilder<TEnumbl>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<StringBuilder> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
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
               .AddFilteredEnumerateStringBuilder(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString
                                          , formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddFilteredEnumerateMatch<TEnumbl, TAnyBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerateMatch(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(object);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddFilteredEnumerateMatch<TEnumbl, TAnyBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerateMatch(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            var elementType = typeof(object);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddFilteredEnumerateMatch<TEnumbl, TAny, TAnyBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : struct, IEnumerable<TAny>
        where TAny : TAnyBase?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TAny);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerateMatch<TEnumbl, TAny, TAnyBase>(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    public TExt AlwaysAddFilteredEnumerateMatch<TEnumbl, TAny, TAnyBase>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<TAnyBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumbl : IEnumerable<TAny>?
        where TAny : TAnyBase?
    {
        var actualType = value?.GetType() ?? typeof(TEnumbl);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TAny);
        stb.FieldNameJoin(fieldName);
        if (value != null)
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddFilteredEnumerateMatch<TEnumbl, TAny, TAnyBase>(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredEnumerateObject<TEnumbl>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<object> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
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
               .AddFilteredEnumerateObject(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredEnumerateObject<TEnumbl>(string fieldName, TEnumbl? value
      , OrderedCollectionPredicate<object> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
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
               .AddFilteredEnumerateObject(value, filterPredicate, formatString, formatFlags)
               .Complete();
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, null, null, formatString, formatFlags
                                          , false);
        }
        return stb.AddGoToNext(true);
    }

}
