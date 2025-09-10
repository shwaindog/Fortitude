using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt AlwaysAddFiltered(string fieldName, bool[]? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(bool);
        var matchCount  = 0;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart(elementType, matchCount++);
            }
        }
        if (found)
        {
            stb.EndCollection(elementType, matchCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.Settings.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(bool?);
        var matchCount  = 0;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart(elementType, matchCount++);
            }
        }
        if (found)
        {
            stb.EndCollection(elementType, matchCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.Settings.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered<TFmt>
    (string fieldName, TFmt[]? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TFmt);
        var matchCount  = 0;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, matchCount++);
            }
        }
        if (found)
        {
            stb.EndCollection(elementType, matchCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.Settings.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered<TFmtStruct>
    (string fieldName, TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TFmtStruct?);
        var matchCount  = 0;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, matchCount++);
            }
        }
        if (found)
        {
            stb.EndCollection(elementType, matchCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.Settings.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered<TToStyle, TToStyleBase, TStylerType>
    (string fieldName, TToStyle[]? value, OrderedCollectionPredicate<TToStyleBase> filterPredicate
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TToStyle);
        var matchCount  = 0;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart(elementType, matchCount++);
            }
        }
        if (found)
        {
            stb.EndCollection(elementType, matchCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.Settings.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered
    (string fieldName, string?[]? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(string);
        var matchCount  = 0;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, matchCount++);
            }
        }
        if (found)
        {
            stb.EndCollection(elementType, matchCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.Settings.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFilteredCharSequence<TCharSeq>
    (string fieldName, TCharSeq?[]? value, OrderedCollectionPredicate<ICharSequence?> filterPredicate)
    where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TCharSeq);
        var matchCount  = 0;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, matchCount++);
            }
        }
        if (found)
        {
            stb.EndCollection(elementType, matchCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.Settings.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered(string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(StringBuilder);
        var matchCount  = 0;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, matchCount++);
            }
        }
        if (found)
        {
            stb.EndCollection(elementType, matchCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.Settings.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered<TStyledObj, TBase>(string fieldName, TStyledObj[]? value, OrderedCollectionPredicate<TBase> filterPredicate)
        where TStyledObj : class, IStyledToStringObject, TBase
        where TBase : class
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(StringBuilder);
        var matchCount  = 0;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart(elementType, matchCount++);
            }
        }
        if (found)
        {
            stb.EndCollection(elementType, matchCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.Settings.NullStyle).AddGoToNext(stb);
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredMatch<T, TBase>(string fieldName, T[]? value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(T);
        var matchCount  = 0;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendObjectOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, matchCount++);
            }
        }
        if (found)
        {
            stb.EndCollection(elementType, matchCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.Settings.NullStyle).AddGoToNext(stb);
    }

    
    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(bool);
        var matchCount  = 0;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart(elementType, matchCount++);
            }
        }
        if (found)
        {
            stb.EndCollection(elementType, matchCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.Settings.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(bool?);
        var matchCount  = 0;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart(elementType, matchCount++);
            }
        }
        if (found)
        {
            stb.EndCollection(elementType, matchCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.Settings.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered<TFmt>(string fieldName, IReadOnlyList<TFmt>? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TFmt);
        var matchCount  = 0;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, matchCount++);
            }
        }
        if (found)
        {
            stb.EndCollection(elementType, matchCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.Settings.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TFmtStruct?);
        var matchCount  = 0;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, matchCount++);
            }
        }
        if (found)
        {
            stb.EndCollection(elementType, matchCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.Settings.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered<TToStyle, TToStyleBase, TStylerType>
        (string fieldName, IReadOnlyList<TToStyle>? value, OrderedCollectionPredicate<TToStyleBase> filterPredicate
          , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TToStyle);
        var matchCount  = 0;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart(elementType, matchCount++);
            }
        }
        if (found)
        {
            stb.EndCollection(elementType, matchCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.Settings.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(string);
        var matchCount  = 0;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, matchCount++);
            }
        }
        if (found)
        {
            stb.EndCollection(elementType, matchCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.Settings.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFilteredCharSequence<TCharSeq>(string fieldName, IReadOnlyList<TCharSeq?>? value
      , OrderedCollectionPredicate<ICharSequence?> filterPredicate)
    where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(TCharSeq);
        var matchCount  = 0;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, matchCount++);
            }
        }
        if (found)
        {
            stb.EndCollection(elementType, matchCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.Settings.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(StringBuilder);
        var matchCount  = 0;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, matchCount++);
            }
        }
        if (found)
        {
            stb.EndCollection(elementType, matchCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.Settings.NullStyle).AddGoToNext(stb);
    }


    [CallsObjectToString]
    public TExt AlwaysAddFilteredMatch<T, TBase>(string fieldName, IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class, TBase
        where TBase : class
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        stb.FieldNameJoin(fieldName);
        var elementType = typeof(T);
        var matchCount  = 0;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendObjectOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, matchCount++);
            }
        }
        if (found)
        {
            stb.EndCollection(elementType, matchCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.Settings.NullStyle).AddGoToNext(stb);
    }
}
