using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;

#pragma warning disable CS0618 // Type or member is obsolete

public interface IPopulatedFieldIncludeFilteredCollection<out T> where T : StyledTypeBuilder
{
    T WithName(string fieldName, bool[]? value, OrderedCollectionPredicate<bool> filterPredicate);

    T WithName(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate);

    T WithName<TNum>(string fieldName, TNum[]? value, OrderedCollectionPredicate<TNum> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TNum>(string fieldName, TNum?[]? value, OrderedCollectionPredicate<TNum?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TStruct>
        (string fieldName, TStruct[]? value, OrderedCollectionPredicate<TStruct> filterPredicate
          , StructStyler<TStruct> structToString) where TStruct : struct;

    T WithName<TStruct>
        (string fieldName, TStruct?[]? value, OrderedCollectionPredicate<TStruct?> filterPredicate
          , StructStyler<TStruct> structToString) where TStruct : struct;

    T WithName(string fieldName, string?[]? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IStyledToStringObject?[]? value, OrderedCollectionPredicate<IStyledToStringObject?> filterPredicate);

    T WithName(string fieldName, IFrozenString?[]? value, OrderedCollectionPredicate<IFrozenString?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IStringBuilder?[]? value, OrderedCollectionPredicate<IStringBuilder?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    [CallsObjectToString]
    T WithName(string fieldName, object?[]? value, OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    
    T WithName(string fieldName, IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate);

    T WithName(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate);

    T WithName<TNum>(string fieldName, IReadOnlyList<TNum>? value, OrderedCollectionPredicate<TNum> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TNum>(string fieldName, IReadOnlyList<TNum?>? value, OrderedCollectionPredicate<TNum?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TStruct>
        (string fieldName, IReadOnlyList<TStruct>? value, OrderedCollectionPredicate<TStruct> filterPredicate
          , StructStyler<TStruct> structToString) where TStruct : struct;

    T WithName<TStruct>
        (string fieldName, IReadOnlyList<TStruct?>? value, OrderedCollectionPredicate<TStruct?> filterPredicate
          , StructStyler<TStruct> structToString) where TStruct : struct;

    T WithName(string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IReadOnlyList<IStyledToStringObject?>? value, OrderedCollectionPredicate<IStyledToStringObject?> filterPredicate);

    T WithName(string fieldName, IReadOnlyList<IFrozenString?>? value, OrderedCollectionPredicate<IFrozenString?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IReadOnlyList<IStringBuilder?>? value, OrderedCollectionPredicate<IStringBuilder?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);


    [CallsObjectToString]
    T WithName(string fieldName, IReadOnlyList<object?>? value, OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);
}

public class PopulatedFieldIncludeFilteredCollection<TExt> : RecyclableObject, IPopulatedFieldIncludeFilteredCollection<TExt>
    where TExt : StyledTypeBuilder
{
    private IStyleTypeBuilderComponentAccess<TExt> stb = null!;

    public PopulatedFieldIncludeFilteredCollection<TExt> Initialize(IStyleTypeBuilderComponentAccess<TExt> styledComplexTypeBuilder)
    {
        stb  = styledComplexTypeBuilder;

        return this;
    }
    
    public TExt WithName(string fieldName, bool[]? value, OrderedCollectionPredicate<bool> filterPredicate)
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

    public TExt WithName(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate)
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

    public TExt WithName<TNum>
    (string fieldName, TNum[]? value, OrderedCollectionPredicate<TNum> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>
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

    public TExt WithName<TNum>
    (string fieldName, TNum?[]? value, OrderedCollectionPredicate<TNum?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>
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

    public TExt WithName<TStruct>
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

    public TExt WithName<TStruct>
    (string fieldName, TStruct?[]? value, OrderedCollectionPredicate<TStruct?> filterPredicate
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

    public TExt WithName
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

    public TExt WithName(string fieldName, IStyledToStringObject?[]? value, OrderedCollectionPredicate<IStyledToStringObject?> filterPredicate)
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

    public TExt WithName
    (string fieldName, IFrozenString?[]? value, OrderedCollectionPredicate<IFrozenString?> filterPredicate
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

    public TExt WithName
    (string fieldName, IStringBuilder?[]? value, OrderedCollectionPredicate<IStringBuilder?> filterPredicate
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

    public TExt WithName(string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate
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
    public TExt WithName(string fieldName, object?[]? value, OrderedCollectionPredicate<object?> filterPredicate
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

    
    public TExt WithName(string fieldName, IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate)
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

    public TExt WithName(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate)
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

    public TExt WithName<TNum>(string fieldName, IReadOnlyList<TNum>? value, OrderedCollectionPredicate<TNum> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>
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

    public TExt WithName<TNum>(string fieldName, IReadOnlyList<TNum?>? value, OrderedCollectionPredicate<TNum?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>
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

    public TExt WithName<TStruct>
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

    public TExt WithName<TStruct>
        (string fieldName, IReadOnlyList<TStruct?>? value, OrderedCollectionPredicate<TStruct?> filterPredicate
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

    public TExt WithName(string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string?> filterPredicate
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

    public TExt WithName(string fieldName, IReadOnlyList<IStyledToStringObject?>? value, OrderedCollectionPredicate<IStyledToStringObject?> filterPredicate)
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

    public TExt WithName(string fieldName, IReadOnlyList<IFrozenString?>? value, OrderedCollectionPredicate<IFrozenString?> filterPredicate
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

    public TExt WithName(string fieldName, IReadOnlyList<IStringBuilder?>? value, OrderedCollectionPredicate<IStringBuilder?> filterPredicate
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

    public TExt WithName(string fieldName, IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate
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
    public TExt WithName(string fieldName, IReadOnlyList<object?>? value, OrderedCollectionPredicate<object?> filterPredicate
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


    public override void StateReset()
    {
        stb  = null!;
        base.StateReset();
    }
}