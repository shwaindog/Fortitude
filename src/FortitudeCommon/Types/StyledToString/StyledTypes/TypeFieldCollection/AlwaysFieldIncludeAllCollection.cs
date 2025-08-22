// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt AlwaysAddAll(string fieldName, bool[]? value)
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Length; i++)
            {
                stb.Sb.Append(value[i]);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll(string fieldName, bool?[]? value)
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Length; i++)
            {
                var boolItem = value[i];
                if (boolItem != null)
                {
                    stb.Sb.Append(boolItem); 
                }
                else
                {
                    stb.Sb.Append(stb.OwningAppender.NullStyle);
                }
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TFmtStruct>(string fieldName, TFmtStruct[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TFmtStruct : struct, ISpanFormattable
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Length; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.Sb.Append(value[i]);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TFmtStruct>(string fieldName, TFmtStruct?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TFmtStruct : struct, ISpanFormattable
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Length; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.Sb.Append(value[i]);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TStruct>
        (string fieldName, TStruct[]? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Length; i++)
            {
                customTypeStyler(value[i], stb.OwningAppender);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TStruct>
        (string fieldName, TStruct?[]? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Length; i++)
            {
                stb.AppendOrNull(value[i], customTypeStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll(string fieldName, string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Length; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.Sb.Append(value[i] ?? stb.OwningAppender.NullStyle);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll(string fieldName, ICharSequence?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Length; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.AppendOrNull(value[i]);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll(string fieldName, StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Length; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.AppendOrNull(value[i]);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }
    
    public TExt AlwaysAddAll<TStyledObj>(string fieldName, TStyledObj[]? value)
        where TStyledObj : class, IStyledToStringObject 
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Length; i++)
            {
                stb.AppendOrNull(value[i]);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<T, TBase>(string fieldName, T?[]? value, CustomTypeStyler<TBase?> customTypeStyler)
        where T : class, TBase where TBase: class 
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Length; i++)
            {
                stb.AppendOrNull(value[i], customTypeStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }
    
    [CallsObjectToString]
    public TExt AlwaysAddAllMatch<T>(string fieldName, T[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Length; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.AppendOrNull(value[i]);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<bool>? value) 
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Count; i++)
            {
                stb.Sb.Append(value[i]);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<bool?>? value) 
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Count; i++)
            {
                stb.Sb.Append(value[i]);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TFmtStruct : struct, ISpanFormattable
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Count; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.Sb.Append(value[i]);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TFmtStruct : struct, ISpanFormattable
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Count; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.Sb.AppendOrNull(value[i]);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }
    

    public TExt AlwaysAddAll<TStruct>
        (string fieldName, IReadOnlyList<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler) 
        where TStruct : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Count; i++)
            {
                customTypeStyler(value[i], stb.OwningAppender);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<TStruct>
        (string fieldName, IReadOnlyList<TStruct?>? value, CustomTypeStyler<TStruct> customTypeStyler) 
        where TStruct : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Count; i++)
            {
                stb.AppendOrNull(value[i], customTypeStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }
    
    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Count; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.Sb.Append(value[i] ?? stb.OwningAppender.NullStyle);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<ICharSequence?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Count; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.AppendOrNull(value[i]);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll(string fieldName, IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Count; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.AppendOrNull(value[i]);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }
    
    public TExt AlwaysAddAll<TStyledObj>(string fieldName, IReadOnlyList<TStyledObj>? value)
        where TStyledObj : class, IStyledToStringObject 
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Count; i++)
            {
                stb.AppendOrNull(value[i]);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAll<T, TBase>(string fieldName, IReadOnlyList<T?>? value, CustomTypeStyler<TBase?> customTypeStyler)
        where T : class, TBase where TBase: class 
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Count; i++)
            {
                stb.AppendOrNull(value[i], customTypeStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    
    [CallsObjectToString]
    public TExt AlwaysAddAllMatch<T>(string fieldName, IReadOnlyList<T>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Count; i++)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value[i], formatString)
                    : stb.AppendOrNull(value[i]);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerable<bool>? value) 
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            foreach (var item in value)
            {
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerable<bool?>? value) 
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            foreach (var item in value)
            {
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerable<TFmtStruct>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TFmtStruct : struct, ISpanFormattable
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            foreach (var item in value)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerable<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TFmtStruct : struct, ISpanFormattable
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            foreach (var item in value)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.Sb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }
    

    public TExt AlwaysAddAllEnumerate<TStruct>
        (string fieldName, IEnumerable<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler) 
        where TStruct : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            foreach (var item in value)
            {
                customTypeStyler(item, stb.OwningAppender);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate<TStruct>
        (string fieldName, IEnumerable<TStruct?>? value, CustomTypeStyler<TStruct> customTypeStyler) 
        where TStruct : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            foreach (var item in value)
            {
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }


    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerable<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            foreach (var item in value)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.Sb.Append(item ?? stb.OwningAppender.NullStyle);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerable<ICharSequence?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            foreach (var item in value)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            foreach (var item in value)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }
    
    public TExt AlwaysAddAllEnumerate<TStyledObj>(string fieldName, IEnumerable<TStyledObj>? value)
        where TStyledObj : class, IStyledToStringObject 
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            foreach (var item in value)
            {
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate<T, TBase>(string fieldName, IEnumerable<T?>? value, CustomTypeStyler<TBase?> customTypeStyler)
        where T : class, TBase where TBase: class 
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            foreach (var item in value)
            {
                stb.AppendOrNull(item, customTypeStyler);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllMatchEnumerate<T>(string fieldName, IEnumerable<T>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            foreach (var item in value)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerator<bool>? value) 
    {
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartCollection();
            while (hasValue)
            {
                stb.Sb.Append(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerator<bool?>? value) 
    {
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartCollection();
            while (hasValue)
            {
                stb.Sb.Append(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerator<TFmtStruct>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TFmtStruct : struct, ISpanFormattable
    {
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartCollection();
            while (hasValue)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.Sb.Append(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate<TFmtStruct>(string fieldName, IEnumerator<TFmtStruct?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TFmtStruct : struct, ISpanFormattable
    {
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartCollection();
            while (hasValue)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.Sb.AppendOrNull(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }
    

    public TExt AlwaysAddAllEnumerate<TStruct>
        (string fieldName, IEnumerator<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler) 
        where TStruct : struct
    {
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartCollection();
            while (hasValue)
            {
                customTypeStyler(value!.Current, stb.OwningAppender);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate<TStruct>
        (string fieldName, IEnumerator<TStruct?>? value, CustomTypeStyler<TStruct> customTypeStyler) 
        where TStruct : struct
    {
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartCollection();
            while (hasValue)
            {
                stb.AppendOrNull(value!.Current, customTypeStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }
    
    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerator<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartCollection();
            while (hasValue)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.Sb.Append(value!.Current ?? stb.OwningAppender.NullStyle);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerator<ICharSequence?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartCollection();
            while (hasValue)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.AppendOrNull(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate(string fieldName, IEnumerator<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartCollection();
            while (hasValue)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.AppendOrNull(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate<TStyledObj>(string fieldName, IEnumerator<TStyledObj>? value)
        where TStyledObj : class, IStyledToStringObject 
    {
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartCollection();
            while (hasValue)
            {
                stb.AppendOrNull(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt AlwaysAddAllEnumerate<T, TBase>(string fieldName, IEnumerator<T?>? value, CustomTypeStyler<TBase?> customTypeStyler)
        where T : class, TBase where TBase: class 
    {
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartCollection();
            while (hasValue)
            {
                stb.AppendOrNull(value!.Current, customTypeStyler);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    [CallsObjectToString]
    public TExt AlwaysAddAllMatchEnumerate<T>(string fieldName, IEnumerator<T>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartCollection();
            while (hasValue)
            {
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(value!.Current, formatString)
                    : stb.AppendOrNull(value!.Current);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }
}
