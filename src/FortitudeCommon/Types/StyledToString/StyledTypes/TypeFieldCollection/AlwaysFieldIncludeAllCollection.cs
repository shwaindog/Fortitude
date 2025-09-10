// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt AlwaysAddAll(string fieldName, bool[]? value)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;

        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(bool);
            stb.StartCollection(elementType, value.Length > 0);
            for (var i = 0; i < value.Length; i++)
            {
                stb.Sb.Append(value[i]);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.EndCollection(elementType, value.Length);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll(string fieldName, bool?[]? value)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(bool?);
            stb.StartCollection(elementType, value.Length > 0);
            for (var i = 0; i < value.Length; i++)
            {
                var boolItem = value[i];
                if (boolItem != null)
                {
                    stb.Sb.Append(boolItem);
                }
                else
                {
                    stb.Sb.Append(stb.Settings.NullStyle);
                }
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.EndCollection(elementType, value.Length);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TFmt>(string fieldName, TFmt[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(TFmt);
            stb.StartCollection(elementType, value.Length > 0);
            for (var i = 0; i < value.Length; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.Sb.Append(value[i]);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.EndCollection(elementType, value.Length);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TToStyle, TStylerType>
        (string fieldName, TToStyle?[]? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(TToStyle);
            stb.StartCollection(elementType, value.Length > 0);
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (item != null)
                {
                    customTypeStyler(item, stb.OwningAppender);
                }
                else
                {
                    stb.Sb.Append(stb.Settings.NullStyle);
                }
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.EndCollection(elementType, value.Length);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll(string fieldName, string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(string);
            stb.StartCollection(elementType, value.Length > 0);
            for (var i = 0; i < value.Length; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.Sb.Append(value[i] ?? stb.Settings.NullStyle);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.EndCollection(elementType, value.Length);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllCharSequence<TCharSeq>(string fieldName, TCharSeq?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            Type elementType = typeof(TCharSeq);
            stb.StartCollection(elementType, value.Length > 0);
            for (var i = 0; i < value.Length; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.AppendOrNull(value[i]);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.EndCollection(elementType, value.Length);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll(string fieldName, StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            Type elementType = typeof(StringBuilder);
            stb.StartCollection(elementType, value.Length > 0);
            for (var i = 0; i < value.Length; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.AppendOrNull(value[i]);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.EndCollection(elementType, value.Length);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TStyledObj>(string fieldName, TStyledObj[]? value)
        where TStyledObj : class, IStyledToStringObject
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            Type elementType = typeof(TStyledObj);
            stb.StartCollection(elementType, value.Length > 0);
            for (var i = 0; i < value.Length; i++)
            {
                stb.AppendOrNull(value[i]);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.EndCollection(elementType, value.Length);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllMatch<T>(string fieldName, T[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            Type elementType = typeof(T);
            stb.StartCollection(elementType, value.Length > 0);
            for (var i = 0; i < value.Length; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.AppendObjectOrNull(value[i]);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.EndCollection(elementType, value.Length);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<bool>? value)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(bool);
            stb.StartCollection(elementType, value.Count > 0);
            for (var i = 0; i < value.Count; i++)
            {
                stb.Sb.Append(value[i]);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.EndCollection(elementType, value.Count);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<bool?>? value)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(bool?);
            stb.StartCollection(elementType, value.Count > 0);
            for (var i = 0; i < value.Count; i++)
            {
                stb.Sb.Append(value[i]);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.EndCollection(elementType, value.Count);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TFmt>(string fieldName, IReadOnlyList<TFmt>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(TFmt);
            stb.StartCollection(elementType, value.Count > 0);
            for (var i = 0; i < value.Count; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.Sb.Append(value[i]);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.EndCollection(elementType, value.Count);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(TFmtStruct);
            stb.StartCollection(elementType, value.Count > 0);
            for (var i = 0; i < value.Count; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.Sb.Append(value[i]);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.EndCollection(elementType, value.Count);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TToStyle, TStylerType>
        (string fieldName, IReadOnlyList<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler)
        where TToStyle : TStylerType
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(TToStyle);
            stb.StartCollection(elementType, value.Count > 0);
            for (var i = 0; i < value.Count; i++)
            {
                customTypeStyler(value[i], stb.OwningAppender);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.EndCollection(elementType, value.Count);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(string);
            stb.StartCollection(elementType, value.Count > 0);
            for (var i = 0; i < value.Count; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.Sb.Append(value[i] ?? stb.Settings.NullStyle);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.EndCollection(elementType, value.Count);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllCharSequence<TCharSeq>(string fieldName, IReadOnlyList<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(TCharSeq);
            stb.StartCollection(elementType, value.Count > 0);
            for (var i = 0; i < value.Count; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.AppendOrNull(value[i]);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.EndCollection(elementType, value.Count);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(StringBuilder);
            stb.StartCollection(elementType, value.Count > 0);
            for (var i = 0; i < value.Count; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.AppendOrNull(value[i]);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.EndCollection(elementType, value.Count);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TStyledObj>(string fieldName, IReadOnlyList<TStyledObj>? value)
        where TStyledObj : class, IStyledToStringObject
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(TStyledObj);
            stb.StartCollection(elementType, value.Count > 0);
            for (var i = 0; i < value.Count; i++)
            {
                stb.AppendOrNull(value[i]);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.EndCollection(elementType, value.Count);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }


    [CallsObjectToString]
    public TExt AlwaysAddAllMatch<T>(string fieldName, IReadOnlyList<T>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(T);
            stb.StartCollection(elementType, value.Count > 0);
            for (var i = 0; i < value.Count; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.AppendObjectOrNull(value[i]);
                stb.GoToNextCollectionItemStart(elementType, i);
            }
            stb.EndCollection(elementType, value.Count);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerable<bool>? value)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(bool);
            var itemCount   = 0;

            foreach (var item in value)
            {
                if (itemCount == 0) stb.StartCollection(elementType, true);
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerable<bool?>? value)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(bool?);
            var itemCount   = 0;
            foreach (var item in value)
            {
                if (itemCount == 0) stb.StartCollection(elementType, true);
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate<TFmt>(string fieldName, IEnumerable<TFmt>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(TFmt);
            var itemCount   = 0;
            foreach (var item in value)
            {
                if (itemCount == 0) stb.StartCollection(elementType, true);
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerable<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(TFmtStruct);
            var itemCount   = 0;
            foreach (var item in value)
            {
                if (itemCount == 0) stb.StartCollection(elementType, true);
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate<TToStyle, TStylerType>
        (string fieldName, IEnumerable<TToStyle?>? value, CustomTypeStyler<TStylerType> customTypeStyler)
        where TToStyle : TStylerType
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(TToStyle);
            var itemCount   = 0;
            foreach (var item in value)
            {
                if (itemCount == 0) stb.StartCollection(elementType, true);
                if (item != null)
                {
                    customTypeStyler(item, stb.OwningAppender);
                }
                else
                {
                    stb.Sb.Append(stb.Settings.NullStyle);
                }
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerable<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(string);
            var itemCount   = 0;
            foreach (var item in value)
            {
                if (itemCount == 0) stb.StartCollection(elementType, true);
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.Sb.Append(item ?? stb.Settings.NullStyle);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllCharSequenceEnumerate<TCharSeq>(string fieldName, IEnumerable<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(TCharSeq);
            var itemCount   = 0;
            foreach (var item in value)
            {
                if (itemCount == 0) stb.StartCollection(elementType, true);
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(StringBuilder);
            var itemCount   = 0;
            foreach (var item in value)
            {
                if (itemCount == 0) stb.StartCollection(elementType, true);
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate<TStyledObj>(string fieldName, IEnumerable<TStyledObj>? value)
        where TStyledObj : class, IStyledToStringObject
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(TStyledObj);
            var itemCount   = 0;
            foreach (var item in value)
            {
                if (itemCount == 0) stb.StartCollection(elementType, true);
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllMatchEnumerate<T>(string fieldName, IEnumerable<T>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var elementType = typeof(T);
            var itemCount   = 0;
            foreach (var item in value)
            {
                if (itemCount == 0) stb.StartCollection(elementType, true);
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendObjectOrNull(item);
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerator<bool>? value)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var elementType = typeof(bool);
            var itemCount   = 0;
            while (hasValue)
            {
                if (itemCount == 0) stb.StartCollection(elementType, true);
                stb.Sb.Append(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerator<bool?>? value)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var elementType = typeof(bool?);
            var itemCount   = 0;
            while (hasValue)
            {
                if (itemCount == 0) stb.StartCollection(elementType, true);
                stb.Sb.Append(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate<TFmt>(string fieldName, IEnumerator<TFmt>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var elementType = typeof(TFmt);
            var itemCount   = 0;
            while (hasValue)
            {
                if (itemCount == 0) stb.StartCollection(elementType, true);
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.Sb.Append(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerator<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var elementType = typeof(TFmtStruct);
            var itemCount   = 0;
            while (hasValue)
            {
                if (itemCount == 0) stb.StartCollection(elementType, true);
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.Sb.Append(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate<TToStyle, TStylerType>
        (string fieldName, IEnumerator<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler)
        where TToStyle : TStylerType
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var elementType = typeof(TToStyle);
            var itemCount   = 0;
            while (hasValue)
            {
                if (itemCount == 0) stb.StartCollection(elementType, true);
                customTypeStyler(value!.Current, stb.OwningAppender);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerator<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var elementType = typeof(string);
            var itemCount   = 0;
            while (hasValue)
            {
                if (itemCount == 0) stb.StartCollection(elementType, true);
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.Sb.Append(value!.Current ?? stb.Settings.NullStyle);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllCharSequenceEnumerate<TCharSeq>(string fieldName, IEnumerator<TCharSeq?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    where TCharSeq : ICharSequence
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var elementType = typeof(TCharSeq);
            var itemCount   = 0;
            while (hasValue)
            {
                if (itemCount == 0) stb.StartCollection(elementType, true);
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.AppendOrNull(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerator<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var elementType = typeof(StringBuilder);
            var itemCount   = 0;
            while (hasValue)
            {
                if (itemCount == 0) stb.StartCollection(elementType, true);
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.AppendOrNull(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate<TStyledObj>(string fieldName, IEnumerator<TStyledObj>? value)
        where TStyledObj : class, IStyledToStringObject
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var elementType = typeof(TStyledObj);
            var itemCount   = 0;
            while (hasValue)
            {
                if (itemCount == 0) stb.StartCollection(elementType, true);
                stb.AppendOrNull(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllMatchEnumerate<T>(string fieldName, IEnumerator<T>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipBody) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var elementType = typeof(T);
            var itemCount   = 0;
            while (hasValue)
            {
                if (itemCount == 0) stb.StartCollection(elementType, true);
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.AppendObjectOrNull(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart(elementType, itemCount++);
            }
            stb.EndCollection(elementType, itemCount);
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }
}
