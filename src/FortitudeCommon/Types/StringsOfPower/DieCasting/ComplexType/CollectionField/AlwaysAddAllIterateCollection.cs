// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.CollectionField;

public partial class SelectTypeCollectionField<TMold> where TMold : TypeMolder
{
    public TMold AlwaysAddAllIterateBool<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<bool>
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(bool);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateBool(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }
    
    public TMold AlwaysAddAllIterateBool<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<bool>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(bool);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateBool(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterateNullableBool<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<bool?>
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(bool?);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateNullableBool(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterateNullableBool<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<bool?>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(bool?);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateNullableBool(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterate<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterate(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            var elementType = typeof(ISpanFormattable);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterate<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterate(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            var elementType = typeof(ISpanFormattable);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterate<TEnumtr, TFmt>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TFmt>
        where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TFmt);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterate(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterate<TEnumtr, TFmt>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TFmt>?
        where TFmt : ISpanFormattable?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TFmt);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterate<TEnumtr, TFmt>(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterateNullable<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateNullable(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            var elementType = typeof(ISpanFormattable);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterateNullable<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateNullable(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            var elementType = typeof(ISpanFormattable);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterateNullable<TEnumtr, TFmtStruct>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TFmtStruct?>
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TFmtStruct?);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateNullable<TEnumtr, TFmtStruct>(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterateNullable<TEnumtr, TFmtStruct>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TFmtStruct?>?
        where TFmtStruct : struct, ISpanFormattable
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TFmtStruct?);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateNullable<TEnumtr, TFmtStruct>(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysRevealAllIterate<TEnumtr, TRevealBase>(string fieldName, TEnumtr? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator 
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllIterate(value, palantírReveal, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            var elementType = typeof(TRevealBase);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysRevealAllIterate<TEnumtr, TRevealBase>(string fieldName, TEnumtr? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator? 
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllIterate(value, palantírReveal, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            var elementType = typeof(TRevealBase);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysRevealAllIterate<TEnumtr, TCloaked, TRevealBase>(string fieldName, TEnumtr? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TCloaked> 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCloaked);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllIterate<TEnumtr, TCloaked, TRevealBase>(value, palantírReveal, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysRevealAllIterate<TEnumtr, TCloaked, TRevealBase>(string fieldName, TEnumtr? value, PalantírReveal<TRevealBase> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCloaked>? 
        where TCloaked : TRevealBase?
        where TRevealBase : notnull
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCloaked);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllIterate<TEnumtr, TCloaked, TRevealBase>(value, palantírReveal, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysRevealAllIterateNullable<TEnumtr, TCloakedStruct>(string fieldName, TEnumtr? value, PalantírReveal<TCloakedStruct> palantírReveal
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TCloakedStruct?> 
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
               .RevealAllIterateNullable(value, palantírReveal, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysRevealAllIterateNullable<TEnumtr, TCloakedStruct>(string fieldName, TEnumtr? value, PalantírReveal<TCloakedStruct> palantírReveal
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
               .RevealAllIterateNullable(value, palantírReveal, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysRevealAllIterate<TEnumtr>(string fieldName, TEnumtr? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator 
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllIterate(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            var elementType = typeof(IStringBearer);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysRevealAllIterate<TEnumtr>(string fieldName, TEnumtr? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator? 
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllIterate(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            var elementType = typeof(IStringBearer);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysRevealAllIterate<TEnumtr, TBearer>(string fieldName, TEnumtr? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TBearer> 
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TBearer);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllIterate<TEnumtr, TBearer>(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysRevealAllIterate<TEnumtr, TBearer>(string fieldName, TEnumtr? value, string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TBearer>? 
        where TBearer : IStringBearer?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TBearer);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllIterate<TEnumtr, TBearer>(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysRevealAllIterateNullable<TEnumtr>(string fieldName, TEnumtr? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator 
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllIterateNullable(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            var elementType = typeof(IStringBearer);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysRevealAllIterateNullable<TEnumtr>(string fieldName, TEnumtr? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator? 
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllIterateNullable(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            var elementType = typeof(IStringBearer);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysRevealAllIterateNullable<TEnumtr, TBearerStruct>(string fieldName, TEnumtr? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TBearerStruct?> 
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TBearerStruct?);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllIterateNullable<TEnumtr, TBearerStruct>(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysRevealAllIterateNullable<TEnumtr, TBearerStruct>(string fieldName, TEnumtr? value
      , string? formatString = null, FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TBearerStruct?>? 
        where TBearerStruct : struct, IStringBearer
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TBearerStruct?);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .RevealAllIterateNullable<TEnumtr, TBearerStruct>(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, "", formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterateString<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<string?> 
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(string);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateString(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterateString<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<string?>? 
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(string);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateString(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterateCharSeq<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator 
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateCharSeq(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            var elementType = typeof(ICharSequence);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterateCharSeq<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator? 
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateCharSeq(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            var elementType = typeof(ICharSequence);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterateCharSeq<TEnumtr, TCharSeq>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TCharSeq> 
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCharSeq);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateCharSeq<TEnumtr, TCharSeq>(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterateCharSeq<TEnumtr, TCharSeq>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TCharSeq>? 
        where TCharSeq : ICharSequence?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TCharSeq);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateCharSeq<TEnumtr, TCharSeq>(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterateStringBuilder<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<StringBuilder?>
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(StringBuilder);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateStringBuilder(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterateStringBuilder<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<StringBuilder?>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(StringBuilder);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateStringBuilder(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterateMatch<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateMatch(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            var elementType = typeof(object);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterateMatch<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateMatch(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            var elementType = typeof(object);
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterateMatch<TEnumtr, TAny>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<TAny>
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TAny);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateMatch<TEnumtr, TAny>(value, formatString, formatFlags, hasValue)
               .Complete();
        }
        else
        {
            stb.AppendEmptyCollectionOrNull(elementType, actualType, value != null ? 0 : null
                                          , value != null ? 0 : null, formatString, formatFlags, false);
        }
        return stb.AddGoToNext(true);
    }

    public TMold AlwaysAddAllIterateMatch<TEnumtr, TAny>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<TAny>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(TAny);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateMatch<TEnumtr, TAny>(value, formatString, formatFlags, hasValue)
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
    public TMold AlwaysAddAllIterateObject<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : struct, IEnumerator<object?>
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(object);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateMatch<TEnumtr, object?>(value, formatString, formatFlags, hasValue)
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
    public TMold AlwaysAddAllIterateObject<TEnumtr>(string fieldName, TEnumtr? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags)
        where TEnumtr : IEnumerator<object?>?
    {
        var actualType = value?.GetType() ?? typeof(TEnumtr);
        if (stb.HasSkipField(actualType, fieldName, formatFlags)) return stb.WasSkipped(actualType, fieldName, formatFlags);
        var elementType = typeof(object);
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.Master
               .StartSimpleCollectionType(value, formatFlags)
               .AddAllIterateMatch<TEnumtr, object?>(value, formatString, formatFlags, hasValue)
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
