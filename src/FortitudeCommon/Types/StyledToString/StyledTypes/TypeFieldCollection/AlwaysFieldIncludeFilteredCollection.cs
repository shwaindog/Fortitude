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
        var found = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection();
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        var found = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection();
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered<TFmtStruct>
    (string fieldName, TFmtStruct[]? value, OrderedCollectionPredicate<TFmtStruct> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        var found = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection();
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    }
    //
    // public TExt AlwaysAddFiltered<TFmtStruct>
    // (string fieldName, TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    //     where TFmtStruct : struct, ISpanFormattable
    // {
    //     var found = false;
    //     stb.FieldNameJoin(fieldName);
    //     if (value != null)
    //     {
    //         for (var i = 0; i < value.Length; i++)
    //         {
    //             var item = value[i];
    //             if(!filterPredicate(i, item)) continue;
    //             if (!found)
    //             {
    //                 stb.StartCollection();
    //                 found = true;
    //             }
    //             stb.Sb.Append(item);
    //             stb.GoToNextCollectionItemStart();
    //         }
    //     }
    //     if (found)
    //     {
    //         stb.EndCollection();
    //         return stb.Sb.AddGoToNext(stb);
    //     }
    //     return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    // }

    public TExt AlwaysAddFiltered<TStruct>
    (string fieldName, TStruct[]? value, OrderedCollectionPredicate<TStruct> filterPredicate
      , StructStyler<TStruct> structToString) where TStruct : struct
    {
        var found = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection();
                    found = true;
                }
                stb.AppendOrNull(item, structToString);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    }
    //
    // public TExt AlwaysAddFiltered<TStruct>
    // (string fieldName, TStruct?[]? value, OrderedCollectionPredicate<TStruct?> filterPredicate
    //   , StructStyler<TStruct> structToString) where TStruct : struct
    // {
    //     var found = false;
    //     stb.FieldNameJoin(fieldName);
    //     if (value != null)
    //     {
    //         for (var i = 0; i < value.Length; i++)
    //         {
    //             var item = value[i];
    //             if(!filterPredicate(i, item)) continue;
    //             if (!found)
    //             {
    //                 stb.StartCollection();
    //                 found = true;
    //             }
    //             stb.Sb.Append(item);
    //             stb.GoToNextCollectionItemStart();
    //         }
    //     }
    //     if (found)
    //     {
    //         stb.EndCollection();
    //         return stb.Sb.AddGoToNext(stb);
    //     }
    //     return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    // }

    public TExt AlwaysAddFiltered
    (string fieldName, string?[]? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        var found = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection();
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered<TStyledObj>(string fieldName, TStyledObj[]? value, OrderedCollectionPredicate<TStyledObj> filterPredicate)
        where TStyledObj : class, IStyledToStringObject 
    {
        var found = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection();
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered
    (string fieldName, ICharSequence?[]? value, OrderedCollectionPredicate<ICharSequence?> filterPredicate)
    {
        var found = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection();
                    found = true;
                }
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered(string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate)
    {
        var found = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection();
                    found = true;
                }
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredMatch<T>(string fieldName, T[]? value, OrderedCollectionPredicate<T> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class
    {
        var found = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection();
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    }

    
    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        var found = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection();
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        var found = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection();
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct>? value, OrderedCollectionPredicate<TFmtStruct> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        var found = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection();
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    }
    //
    // public TExt AlwaysAddFiltered<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    //     where TFmtStruct : struct, ISpanFormattable
    // {
    //     var found = false;
    //     stb.FieldNameJoin(fieldName);
    //     if (value != null)
    //     {
    //         for (var i = 0; i < value.Count; i++)
    //         {
    //             var item = value[i];
    //             if(!filterPredicate(i, item)) continue;
    //             if (!found)
    //             {
    //                 stb.StartCollection();
    //                 found = true;
    //             }
    //             stb.Sb.Append(item);
    //             stb.GoToNextCollectionItemStart();
    //         }
    //     }
    //     if (found)
    //     {
    //         stb.EndCollection();
    //         return stb.Sb.AddGoToNext(stb);
    //     }
    //     return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    // }

    public TExt AlwaysAddFiltered<TStruct>
        (string fieldName, IReadOnlyList<TStruct>? value, OrderedCollectionPredicate<TStruct> filterPredicate
          , StructStyler<TStruct> structToString) where TStruct : struct
    {
        var found = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection();
                    found = true;
                }
                stb.AppendOrNull(item, structToString);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    }
    //
    // public TExt AlwaysAddFiltered<TStruct>
    //     (string fieldName, IReadOnlyList<TStruct?>? value, OrderedCollectionPredicate<TStruct?> filterPredicate
    //       , StructStyler<TStruct> structToString) where TStruct : struct
    // {
    //     var found = false;
    //     stb.FieldNameJoin(fieldName);
    //     if (value != null)
    //     {
    //         for (var i = 0; i < value.Count; i++)
    //         {
    //             var item = value[i];
    //             if(!filterPredicate(i, item)) continue;
    //             if (!found)
    //             {
    //                 stb.StartCollection();
    //                 found = true;
    //             }
    //             stb.Sb.Append(item);
    //             stb.GoToNextCollectionItemStart();
    //         }
    //     }
    //     if (found)
    //     {
    //         stb.EndCollection();
    //         return stb.Sb.AddGoToNext(stb);
    //     }
    //     return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    // }

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        var found = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection();
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered<TStyledObj>(string fieldName, IReadOnlyList<TStyledObj>? value, OrderedCollectionPredicate<TStyledObj> filterPredicate)
        where TStyledObj : class, IStyledToStringObject 
    {
        var found = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection();
                    found = true;
                }
                stb.Sb.Append(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<ICharSequence?>? value
      , OrderedCollectionPredicate<ICharSequence?> filterPredicate)
    {
        var found = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection();
                    found = true;
                }
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    }

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate)
    {
        var found = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection();
                    found = true;
                }
                stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    }


    [CallsObjectToString]
    public TExt AlwaysAddFilteredMatch<T>(string fieldName, IReadOnlyList<T>? value, OrderedCollectionPredicate<T> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class
    {
        var found = false;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.StartCollection();
                    found = true;
                }
                _ = formatString.IsNotNullOrEmpty()
                    ? stb.AppendFormattedOrNull(item, formatString)
                    : stb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.Sb.Append(stb.OwningAppender.NullStyle).AddGoToNext(stb);
    }
}
