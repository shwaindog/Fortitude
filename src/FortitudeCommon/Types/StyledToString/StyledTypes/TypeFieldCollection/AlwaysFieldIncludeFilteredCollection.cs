using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
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

    public TExt AlwaysAddFiltered<TNum>
    (string fieldName, TNum[]? value, OrderedCollectionPredicate<TNum> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>
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

    public TExt AlwaysAddFiltered<TNum>
    (string fieldName, TNum?[]? value, OrderedCollectionPredicate<TNum?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>
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

    public TExt AlwaysAddFiltered<TStruct>
    (string fieldName, TStruct?[]? value, OrderedCollectionPredicate<TStruct?> filterPredicate
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

    public TExt AlwaysAddFiltered(string fieldName, IStyledToStringObject?[]? value, OrderedCollectionPredicate<IStyledToStringObject?> filterPredicate)
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
    (string fieldName, IFrozenString?[]? value, OrderedCollectionPredicate<IFrozenString?> filterPredicate
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
    (string fieldName, IStringBuilder?[]? value, OrderedCollectionPredicate<IStringBuilder?> filterPredicate
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

    public TExt AlwaysAddFiltered(string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate
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

    [CallsObjectToString]
    public TExt AlwaysAddFiltered(string fieldName, object?[]? value, OrderedCollectionPredicate<object?> filterPredicate
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

    public TExt AlwaysAddFiltered<TNum>(string fieldName, IReadOnlyList<TNum>? value, OrderedCollectionPredicate<TNum> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>
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

    public TExt AlwaysAddFiltered<TNum>(string fieldName, IReadOnlyList<TNum?>? value, OrderedCollectionPredicate<TNum?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>
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

    public TExt AlwaysAddFiltered<TStruct>
        (string fieldName, IReadOnlyList<TStruct?>? value, OrderedCollectionPredicate<TStruct?> filterPredicate
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

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<IStyledToStringObject?>? value, OrderedCollectionPredicate<IStyledToStringObject?> filterPredicate)
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

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<IFrozenString?>? value, OrderedCollectionPredicate<IFrozenString?> filterPredicate
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

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<IStringBuilder?>? value, OrderedCollectionPredicate<IStringBuilder?> filterPredicate
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

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate
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


    [CallsObjectToString]
    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<object?>? value, OrderedCollectionPredicate<object?> filterPredicate
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
}
