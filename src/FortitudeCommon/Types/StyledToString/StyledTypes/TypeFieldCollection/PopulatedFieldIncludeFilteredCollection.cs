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
        var found = false;
        if (value != null)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        if (value != null)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                stb.Sb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        if (found)
        {
            stb.EndCollection();
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
        var found = false;
        if (value != null)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        if (found)
        {
            stb.EndCollection();
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
        var found = false;
        if (value != null)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TToStyle, TStylerType, TToStyleBase>
    (string fieldName, TToStyle[]? value, OrderedCollectionPredicate<TToStyleBase> filterPredicate
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType, TToStyleBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        if (value != null)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart();
            }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter
    (string fieldName, string?[]? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        if (value != null)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter
        (string fieldName, ICharSequence?[]? value, OrderedCollectionPredicate<ICharSequence?> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        if (value != null)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        if (value != null)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TStyledObj, TBase>(string fieldName, TStyledObj[]? value, OrderedCollectionPredicate<TBase> filterPredicate)
        where TStyledObj : class, IStyledToStringObject, TBase where TBase : class
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        if (value != null)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<T, TBase1, TBase2>(string fieldName, T[]? value, OrderedCollectionPredicate<TBase1> filterPredicate)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        if (value != null)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TExt WhenPopulatedWithFilterMatch<T, TBase>(string fieldName, T[]? value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class, TBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        if (value != null)
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendObjectOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }


    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        if (value != null)
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        if (value != null)
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                stb.Sb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmt>(string fieldName, IReadOnlyList<TFmt>? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        if (value != null)
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        if (value != null)
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TToStyle, TStylerType, TToStyleBase>
    (string fieldName, IReadOnlyList<TToStyle>? value, OrderedCollectionPredicate<TToStyleBase> filterPredicate
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType, TToStyleBase
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        if (value != null)
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart();
            }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        if (value != null)
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<ICharSequence?>? value
      , OrderedCollectionPredicate<ICharSequence?> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        if (value != null)
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<StringBuilder?>? value
      , OrderedCollectionPredicate<StringBuilder?> filterPredicate)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        if (value != null)
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TExt WhenPopulatedWithFilter<T, TBase1, TBase2>(string fieldName, IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase1> filterPredicate)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        if (value != null)
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterMatch<T>(string fieldName, IReadOnlyList<T>? value, OrderedCollectionPredicate<T> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        var found = false;
        if (value != null)
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendObjectOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }
}
