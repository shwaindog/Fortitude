// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;

#pragma warning disable CS0618 // Type or member is obsolete
public partial class SelectTypeCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt WhenPopulatedWithFilter(string fieldName, bool[]? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found       = false;
        var elementType = typeof(bool);
        var itemCount   = 0;
        if (value != null)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
        if (found)
        {
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found       = false;
        var elementType = typeof(bool?);
        var itemCount   = 0;
        if (value != null)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
        if (found)
        {
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmt>
    (string fieldName, TFmt[]? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found       = false;
        var elementType = typeof(TFmt);
        var itemCount   = 0;
        if (value != null)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItem(value[i], itemCount, formatString);
                else
                    stb.AppendCollectionItem(value[i], itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
        if (found)
        {
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmtStruct>
    (string fieldName, TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found       = false;
        var elementType = typeof(TFmtStruct?);
        var itemCount   = 0;
        if (value != null)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItem(value[i], itemCount, formatString);
                else
                    stb.AppendCollectionItem(value[i], itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
        if (found)
        {
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TToStyle, TStylerType, TToStyleBase>
    (string fieldName, TToStyle[]? value, OrderedCollectionPredicate<TToStyleBase> filterPredicate
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType, TToStyleBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found       = false;
        var elementType = typeof(TToStyle);
        var itemCount   = 0;
        if (value != null)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
        if (found)
        {
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter
    (string fieldName, string?[]? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found       = false;
        var elementType = typeof(string);
        var itemCount   = 0;
        if (value != null)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
        if (found)
        {
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterCharSequence<TCharSeq>
        (string fieldName, TCharSeq?[]? value, OrderedCollectionPredicate<ICharSequence?> filterPredicate
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found       = false;
        var elementType = typeof(TCharSeq);
        var itemCount   = 0;
        if (value != null)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
        if (found)
        {
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found       = false;
        var elementType = typeof(StringBuilder);
        var itemCount   = 0;
        if (value != null)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
        if (found)
        {
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterMatch<T, TBase>(string fieldName, T[]? value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found       = false;
        var elementType = typeof(T);
        var itemCount   = 0;
        if (value != null)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
        if (found)
        {
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }


    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found       = false;
        var elementType = typeof(bool);
        var itemCount   = 0;
        if (value != null)
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
        if (found)
        {
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found       = false;
        var elementType = typeof(bool?);
        var itemCount   = 0;
        if (value != null)
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                stb.AppendCollectionItem(item, i);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
        if (found)
        {
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmt>(string fieldName, IReadOnlyList<TFmt>? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found       = false;
        var elementType = typeof(TFmt);
        var itemCount   = 0;
        if (value != null)
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItem(value[i], itemCount, formatString);
                else
                    stb.AppendCollectionItem(value[i], itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
        if (found)
        {
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found       = false;
        var elementType = typeof(TFmtStruct?);
        var itemCount   = 0;
        if (value != null)
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendNullableFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
        if (found)
        {
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TToStyle, TStylerType, TToStyleBase>
    (string fieldName, IReadOnlyList<TToStyle>? value, OrderedCollectionPredicate<TToStyleBase> filterPredicate
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType, TToStyleBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found       = false;
        var elementType = typeof(TToStyle);
        var itemCount   = 0;
        if (value != null)
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
        if (found)
        {
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found       = false;
        var elementType = typeof(string);
        var itemCount   = 0;
        if (value != null)
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString, 0)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
        if (found)
        {
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterCharSequence<TCharSeq>(string fieldName, IReadOnlyList<TCharSeq?>? value
      , OrderedCollectionPredicate<ICharSequence?> filterPredicate) where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found       = false;
        var elementType = typeof(TCharSeq);
        var itemCount   = 0;
        if (value != null)
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
        if (found)
        {
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<StringBuilder?>? value
      , OrderedCollectionPredicate<StringBuilder?> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found       = false;
        var elementType = typeof(StringBuilder);
        var itemCount   = 0;
        if (value != null)
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
        if (found)
        {
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterMatch<T, TBase>(string fieldName, IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found       = false;
        var elementType = typeof(T);
        var itemCount   = 0;
        if (value != null)
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection(elementType, true);
                    found = true;
                }
                if (formatString.IsNotNullOrEmpty())
                    stb.AppendFormattedCollectionItemMatchOrNull(item, itemCount, formatString);
                else
                    stb.AppendCollectionItemMatchOrNull(item, itemCount);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
        if (found)
        {
            stb.EndCollection(elementType, itemCount);
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }
}
