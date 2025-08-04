// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;

public interface IAlwaysFieldIncludeAllCollection<out T> where T : StyledTypeBuilder
{
    T WithName(string fieldName, bool[]? value);

    T WithName(string fieldName, bool?[]? value);

    T WithName<TNum>(string fieldName, TNum[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TNum>(string fieldName, TNum?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TStruct>
        (string fieldName, TStruct[]? value, StructStyler<TStruct> structToString) where TStruct : struct;

    T WithName<TStruct>
        (string fieldName, TStruct?[]? value, StructStyler<TStruct> structToString) where TStruct : struct;

    T WithName(string fieldName, string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IStyledToStringObject?[]? value);

    T WithName(string fieldName, IFrozenString?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IStringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    [CallsObjectToString]
    T WithName(string fieldName, object?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    
    T WithName(string fieldName, IReadOnlyList<bool>? value);

    T WithName(string fieldName, IReadOnlyList<bool?>? value);

    T WithName<TNum>(string fieldName, IReadOnlyList<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TNum>(string fieldName, IReadOnlyList<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TStruct>
        (string fieldName, IReadOnlyList<TStruct>? value, StructStyler<TStruct> structToString) where TStruct : struct;

    T WithName<TStruct>
        (string fieldName, IReadOnlyList<TStruct?>? value, StructStyler<TStruct> structToString) where TStruct : struct;

    T WithName(string fieldName, IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IReadOnlyList<IStyledToStringObject?>? value);

    T WithName(string fieldName, IReadOnlyList<IFrozenString?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IReadOnlyList<IStringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);


    [CallsObjectToString]
    T WithName(string fieldName, IReadOnlyList<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    
    T WithName(string fieldName, IEnumerable<bool>? value);

    T WithName(string fieldName, IEnumerable<bool?>? value);

    T WithName<TNum>(string fieldName, IEnumerable<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TNum>(string fieldName, IEnumerable<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TStruct>
        (string fieldName, IEnumerable<TStruct>? value, StructStyler<TStruct> structToString) where TStruct : struct;

    T WithName<TStruct>
        (string fieldName, IEnumerable<TStruct?>? value, StructStyler<TStruct> structToString) where TStruct : struct;

    T WithName(string fieldName, IEnumerable<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IEnumerable<IStyledToStringObject?>? value);

    T WithName(string fieldName, IEnumerable<IFrozenString?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IEnumerable<IStringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    [CallsObjectToString]
    T WithName(string fieldName, IEnumerable<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    
    T WithName(string fieldName, IEnumerator<bool>? value);

    T WithName(string fieldName, IEnumerator<bool?>? value);

    T WithName<TNum>(string fieldName, IEnumerator<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TNum>(string fieldName, IEnumerator<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TStruct>
        (string fieldName, IEnumerator<TStruct>? value, StructStyler<TStruct> structToString) where TStruct : struct;

    T WithName<TStruct>
        (string fieldName, IEnumerator<TStruct?>? value, StructStyler<TStruct> structToString) where TStruct : struct;

    T WithName(string fieldName, IEnumerator<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IEnumerator<IStyledToStringObject?>? value);

    T WithName(string fieldName, IEnumerator<IFrozenString?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IEnumerator<IStringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IEnumerator<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    [CallsObjectToString]
    T WithName(string fieldName, IEnumerator<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);
}

public class AlwaysFieldIncludeAllCollection<TExt> : RecyclableObject, IAlwaysFieldIncludeAllCollection<TExt>
    where TExt : StyledTypeBuilder
{
    private IStyleTypeBuilderComponentAccess<TExt> stb = null!;

    public AlwaysFieldIncludeAllCollection<TExt> Initialize(IStyleTypeBuilderComponentAccess<TExt> styledComplexTypeBuilder)
    {
        stb = styledComplexTypeBuilder;

        return this;
    }

    public TExt WithName(string fieldName, bool[]? value)
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

    public TExt WithName(string fieldName, bool?[]? value)
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

    public TExt WithName<TNum>(string fieldName, TNum[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TNum : struct, INumber<TNum>
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

    public TExt WithName<TNum>(string fieldName, TNum?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TNum : struct, INumber<TNum>
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

    public TExt WithName<TStruct>
        (string fieldName, TStruct[]? value, StructStyler<TStruct> structToString) where TStruct : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Length; i++)
            {
                structToString(value[i], stb.OwningAppender);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt WithName<TStruct>
        (string fieldName, TStruct?[]? value, StructStyler<TStruct> structToString) where TStruct : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Length; i++)
            {
                stb.AppendOrNull(value[i], structToString);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt WithName(string fieldName, string?[]? value
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

    public TExt WithName(string fieldName, IStyledToStringObject?[]? value)
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

    public TExt WithName(string fieldName, IFrozenString?[]? value
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

    public TExt WithName(string fieldName, IStringBuilder?[]? value
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

    public TExt WithName(string fieldName, StringBuilder?[]? value
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
    
    [CallsObjectToString]
    public TExt WithName(string fieldName, object?[]? value
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

    public TExt WithName(string fieldName, IReadOnlyList<bool>? value) 
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

    public TExt WithName(string fieldName, IReadOnlyList<bool?>? value) 
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

    public TExt WithName<TNum>(string fieldName, IReadOnlyList<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TNum : struct, INumber<TNum>
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

    public TExt WithName<TNum>(string fieldName, IReadOnlyList<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TNum : struct, INumber<TNum>
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
    

    public TExt WithName<TStruct>
        (string fieldName, IReadOnlyList<TStruct>? value, StructStyler<TStruct> structToString) 
        where TStruct : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Count; i++)
            {
                structToString(value[i], stb.OwningAppender);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt WithName<TStruct>
        (string fieldName, IReadOnlyList<TStruct?>? value, StructStyler<TStruct> structToString) 
        where TStruct : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            for (var i = 0; i < value.Count; i++)
            {
                stb.AppendOrNull(value[i], structToString);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }


    public TExt WithName(string fieldName, IReadOnlyList<string?>? value
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

    public TExt WithName(string fieldName, IReadOnlyList<IFrozenString?>? value
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

    public TExt WithName(string fieldName, IReadOnlyList<IStringBuilder?>? value
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

    public TExt WithName(string fieldName, IReadOnlyList<StringBuilder?>? value
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
    
    [CallsObjectToString]
    public TExt WithName(string fieldName, IReadOnlyList<object?>? value
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

    public TExt WithName(string fieldName, IReadOnlyList<IStyledToStringObject?>? value)
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

    public TExt WithName(string fieldName, IEnumerable<bool>? value) 
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

    public TExt WithName(string fieldName, IEnumerable<bool?>? value) 
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

    public TExt WithName<TNum>(string fieldName, IEnumerable<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TNum : struct, INumber<TNum>
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

    public TExt WithName<TNum>(string fieldName, IEnumerable<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TNum : struct, INumber<TNum>
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
    

    public TExt WithName<TStruct>
        (string fieldName, IEnumerable<TStruct>? value, StructStyler<TStruct> structToString) 
        where TStruct : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            foreach (var item in value)
            {
                structToString(item, stb.OwningAppender);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt WithName<TStruct>
        (string fieldName, IEnumerable<TStruct?>? value, StructStyler<TStruct> structToString) 
        where TStruct : struct
    {
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            stb.StartCollection();
            foreach (var item in value)
            {
                stb.AppendOrNull(item, structToString);
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }


    public TExt WithName(string fieldName, IEnumerable<string?>? value
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

    public TExt WithName(string fieldName, IEnumerable<IFrozenString?>? value
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

    public TExt WithName(string fieldName, IEnumerable<IStringBuilder?>? value
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

    public TExt WithName(string fieldName, IEnumerable<StringBuilder?>? value
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

    public TExt WithName(string fieldName, IEnumerable<IStyledToStringObject?>? value)
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

    [CallsObjectToString]
    public TExt WithName(string fieldName, IEnumerable<object?>? value
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

    public TExt WithName(string fieldName, IEnumerator<bool>? value) 
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

    public TExt WithName(string fieldName, IEnumerator<bool?>? value) 
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

    public TExt WithName<TNum>(string fieldName, IEnumerator<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TNum : struct, INumber<TNum>
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

    public TExt WithName<TNum>(string fieldName, IEnumerator<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where TNum : struct, INumber<TNum>
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
    

    public TExt WithName<TStruct>
        (string fieldName, IEnumerator<TStruct>? value, StructStyler<TStruct> structToString) 
        where TStruct : struct
    {
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartCollection();
            while (hasValue)
            {
                structToString(value!.Current, stb.OwningAppender);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }

    public TExt WithName<TStruct>
        (string fieldName, IEnumerator<TStruct?>? value, StructStyler<TStruct> structToString) 
        where TStruct : struct
    {
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            stb.StartCollection();
            while (hasValue)
            {
                stb.AppendOrNull(value!.Current, structToString);
                hasValue = value.MoveNext();
                stb.GoToNextCollectionItemStart();
            }
            stb.EndCollection();
        }
        else
            stb.Sb.Append(stb.OwningAppender.NullStyle);
        return stb.Sb.AddGoToNext(stb);
    }


    public TExt WithName(string fieldName, IEnumerator<string?>? value
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

    public TExt WithName(string fieldName, IEnumerator<IFrozenString?>? value
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

    public TExt WithName(string fieldName, IEnumerator<IStringBuilder?>? value
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

    public TExt WithName(string fieldName, IEnumerator<StringBuilder?>? value
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

    public TExt WithName(string fieldName, IEnumerator<IStyledToStringObject?>? value)
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

    [CallsObjectToString]
    public TExt WithName(string fieldName, IEnumerator<object?>? value
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


    public override void StateReset()
    {
        stb = null!;
        base.StateReset();
    }
}
