using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;

#pragma warning disable CS0618 // Type or member is obsolete
public partial class SelectTypeCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt WhenPopulatedWithFilter(string fieldName, bool[]? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        var found = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
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
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        var found = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                stb.Sb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmtStruct>
    (string fieldName, TFmtStruct[]? value, OrderedCollectionPredicate<TFmtStruct> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        var found = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
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
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    // public TExt WhenPopulatedWithFilter<TFmtStruct>
    // (string fieldName, TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    //     where TFmtStruct : struct, ISpanFormattable
    // {
    //     var found = false;
    //     if (value != null)
    //     {
    //         for (var i = 0; i < value.Length; i++)
    //         {
    //             var item = value[i];
    //             if(!filterPredicate(i, item)) continue;
    //             if (!found)
    //             {
    //                 stb.FieldNameJoin(fieldName);
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
    //     return stb.StyleTypeBuilder;
    // }

    public TExt WhenPopulatedWithFilter<TStruct>
    (string fieldName, TStruct[]? value, OrderedCollectionPredicate<TStruct> filterPredicate
      , StructStyler<TStruct> structToString) where TStruct : struct
    {
        var found = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
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
        return stb.StyleTypeBuilder;
    }

    // public TExt WhenPopulatedWithFilter<TStruct>
    // (string fieldName, TStruct?[]? value, OrderedCollectionPredicate<TStruct?> filterPredicate
    //   , StructStyler<TStruct> structToString) where TStruct : struct
    // {
    //     var found = false;
    //     if (value != null)
    //     {
    //         for (var i = 0; i < value.Length; i++)
    //         {
    //             var item = value[i];
    //             if(!filterPredicate(i, item)) continue;
    //             if (!found)
    //             {
    //                 stb.FieldNameJoin(fieldName);
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
    //     return stb.StyleTypeBuilder;
    // }

    public TExt WhenPopulatedWithFilter
    (string fieldName, string?[]? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        var found = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
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
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TStyledObj>(string fieldName
      , TStyledObj[]? value, OrderedCollectionPredicate<TStyledObj> filterPredicate)
        where TStyledObj : class, IStyledToStringObject
    {
        var found = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
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
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter
    (string fieldName, ICharSequence?[]? value, OrderedCollectionPredicate<ICharSequence?> filterPredicate)
    {
        var found = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
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
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate)
    {
        var found = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
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
        return stb.StyleTypeBuilder;
    }

    [CallsObjectToString]
    public TExt WhenPopulatedWithFilterMatch<T>(string fieldName, T[]? value, OrderedCollectionPredicate<T> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class
    {
        var found = false;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
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
        var found = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
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
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        var found = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
                    stb.StartCollection();
                    found = true;
                }
                stb.Sb.AppendOrNull(item);
                stb.GoToNextCollectionItemStart();
            }
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct>? value, OrderedCollectionPredicate<TFmtStruct> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        var found = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
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
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    // public TExt WhenPopulatedWithFilter<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
    //   , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    //     where TFmtStruct : struct, ISpanFormattable
    // {
    //     var found = false;
    //     if (value != null)
    //     {
    //         for (var i = 0; i < value.Count; i++)
    //         {
    //             var item = value[i];
    //             if(!filterPredicate(i, item)) continue;
    //             if (!found)
    //             {
    //                 stb.FieldNameJoin(fieldName);
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
    //     return stb.StyleTypeBuilder;
    // }

    public TExt WhenPopulatedWithFilter<TStruct>
        (string fieldName, IReadOnlyList<TStruct>? value, OrderedCollectionPredicate<TStruct> filterPredicate
          , StructStyler<TStruct> structToString) where TStruct : struct
    {
        var found = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
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
        return stb.StyleTypeBuilder;
    }

    // public TExt WhenPopulatedWithFilter<TStruct>
    //     (string fieldName, IReadOnlyList<TStruct?>? value, OrderedCollectionPredicate<TStruct?> filterPredicate
    //       , StructStyler<TStruct> structToString) where TStruct : struct
    // {
    //     var found = false;
    //     if (value != null)
    //     {
    //         for (var i = 0; i < value.Count; i++)
    //         {
    //             var item = value[i];
    //             if(!filterPredicate(i, item)) continue;
    //             if (!found)
    //             {
    //                 stb.FieldNameJoin(fieldName);
    //                 stb.StartCollection();
    //                 found = true;
    //             }
    //             stb.AppendOrNull(item, structToString);
    //             stb.GoToNextCollectionItemStart();
    //         }
    //     }
    //     if (found)
    //     {
    //         stb.EndCollection();
    //         return stb.Sb.AddGoToNext(stb);
    //     }
    //     return stb.StyleTypeBuilder;
    // }

    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        var found = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
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
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TStyledObj>(string fieldName, IReadOnlyList<TStyledObj>? value
      , OrderedCollectionPredicate<TStyledObj> filterPredicate)
        where TStyledObj : class, IStyledToStringObject
    {
        var found = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
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
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<ICharSequence?>? value, OrderedCollectionPredicate<ICharSequence?> filterPredicate)
    {
        var found = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
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
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate)
    {
        var found = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
                if (!found)
                {
                    stb.FieldNameJoin(fieldName);
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
        return stb.StyleTypeBuilder;
    }


    [CallsObjectToString]
    public TExt WhenPopulatedWithFilterMatch<T>(string fieldName, IReadOnlyList<T>? value, OrderedCollectionPredicate<T> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class
    {
        var found = false;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if(!filterPredicate(i, item)) continue;
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
        }
        if (found)
        {
            stb.EndCollection();
            return stb.Sb.AddGoToNext(stb);
        }
        return stb.StyleTypeBuilder;
    }
}