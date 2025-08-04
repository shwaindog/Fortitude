// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable.Strings;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;

public interface INonNullFieldIncludeAllCollection<out T> : IRecyclableObject where T : StyledTypeBuilder
{
    T WithName(string fieldName, bool[]? value);

    T WithName(string fieldName, bool?[]? value);

    T WithName<TNum>
    (string fieldName, TNum[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TNum>
    (string fieldName, TNum?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TStruct>
        (string fieldName, TStruct[]? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    T WithName<TStruct>
        (string fieldName, TStruct?[]? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    T WithName
    (string fieldName, string[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IStyledToStringObject[]? value);

    T WithName
    (string fieldName, IFrozenString[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName
    (string fieldName, IStringBuilder[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName
    (string fieldName, StringBuilder[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    [CallsObjectToString]
    T WithName
    (string fieldName, object?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);


    T WithName(string fieldName, IReadOnlyList<bool>? value);

    T WithName(string fieldName, IReadOnlyList<bool?>? value);

    T WithName<TNum>
    (string fieldName, IReadOnlyList<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TNum>
    (string fieldName, IReadOnlyList<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TStruct>
        (string fieldName, IReadOnlyList<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    T WithName<TStruct>
        (string fieldName, IReadOnlyList<TStruct?>? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    T WithName
    (string fieldName, IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IReadOnlyList<IStyledToStringObject?>? value);

    T WithName
    (string fieldName, IReadOnlyList<IFrozenString?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName
    (string fieldName, IReadOnlyList<IStringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName
    (string fieldName, IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    [CallsObjectToString]
    T WithName
    (string fieldName, IReadOnlyList<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);


    T WithName(string fieldName, IEnumerable<bool>? value);

    T WithName(string fieldName, IEnumerable<bool?>? value);

    T WithName<TNum>
    (string fieldName, IEnumerable<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TNum>
    (string fieldName, IEnumerable<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TStruct>
        (string fieldName, IEnumerable<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    T WithName<TStruct>
        (string fieldName, IEnumerable<TStruct?>? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    T WithName
    (string fieldName, IEnumerable<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IEnumerable<IStyledToStringObject?>? value);

    T WithName
    (string fieldName, IEnumerable<IFrozenString?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName
    (string fieldName, IEnumerable<IStringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName
    (string fieldName, IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    [CallsObjectToString]
    T WithName
    (string fieldName, IEnumerable<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);


    T WithName(string fieldName, IEnumerator<bool>? value);

    T WithName(string fieldName, IEnumerator<bool?>? value);

    T WithName<TNum>
    (string fieldName, IEnumerator<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TNum>
    (string fieldName, IEnumerator<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TNum : struct, INumber<TNum>;

    T WithName<TStruct>
        (string fieldName, IEnumerator<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    T WithName<TStruct>
        (string fieldName, IEnumerator<TStruct?>? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    T WithName
    (string fieldName, IEnumerator<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName(string fieldName, IEnumerator<IStyledToStringObject?>? value);

    T WithName
    (string fieldName, IEnumerator<IFrozenString?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName
    (string fieldName, IEnumerator<IStringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    T WithName
    (string fieldName, IEnumerator<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);

    [CallsObjectToString]
    T WithName
    (string fieldName, IEnumerator<object?> value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null);
}

public class NonNullFieldIncludeAllCollection<TExt> : RecyclableObject, INonNullFieldIncludeAllCollection<TExt>
    where TExt : StyledTypeBuilder
{
    private IStyleTypeBuilderComponentAccess<TExt> stb = null!;

    private IAlwaysFieldIncludeAllCollection<TExt> aicf = null!;

    public NonNullFieldIncludeAllCollection<TExt> Initialize(IStyleTypeBuilderComponentAccess<TExt> styledComplexTypeBuilder
      , IAlwaysFieldIncludeAllCollection<TExt> allAlwaysIncludeAddAllCollectionField)
    {
        stb  = styledComplexTypeBuilder;
        aicf = allAlwaysIncludeAddAllCollectionField;

        return this;
    }

    public TExt WithName(string fieldName, bool[]? value) => value != null ? aicf.WithName(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, bool?[]? value) => value != null ? aicf.WithName(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WithName<TNum>
    (string fieldName, TNum[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName<TNum>(string fieldName, TNum?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName<TStruct>
        (string fieldName, TStruct[]? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? aicf.WithName(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt WithName<TStruct>
        (string fieldName, TStruct?[]? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? aicf.WithName(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, string?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, IStyledToStringObject?[]? value) => 
        value != null ? aicf.WithName(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IFrozenString?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IStringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, StringBuilder?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;


    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public TExt WithName
    (string fieldName, object?[]? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, IReadOnlyList<bool>? value) => 
        value != null ? aicf.WithName(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, IReadOnlyList<bool?>? value) => 
        value != null ? aicf.WithName(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WithName<TNum>
    (string fieldName, IReadOnlyList<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName<TNum>
    (string fieldName, IReadOnlyList<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName<TStruct>
        (string fieldName, IReadOnlyList<TStruct>? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? aicf.WithName(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt WithName<TStruct>
        (string fieldName, IReadOnlyList<TStruct?>? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? aicf.WithName(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IReadOnlyList<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, IReadOnlyList<IStyledToStringObject?>? value) => 
        value != null ? aicf.WithName(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IReadOnlyList<IFrozenString?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IReadOnlyList<IStringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IReadOnlyList<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;


    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public TExt WithName
    (string fieldName, IReadOnlyList<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, IEnumerable<bool>? value) => 
        value != null ? aicf.WithName(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, IEnumerable<bool?>? value) => 
        value != null ? aicf.WithName(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WithName<TNum>
    (string fieldName, IEnumerable<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName<TNum>
    (string fieldName, IEnumerable<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName<TStruct>
        (string fieldName, IEnumerable<TStruct>? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? aicf.WithName(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt WithName<TStruct>
        (string fieldName, IEnumerable<TStruct?>? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? aicf.WithName(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IEnumerable<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, IEnumerable<IStyledToStringObject?>? value) => 
        value != null ? aicf.WithName(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IEnumerable<IFrozenString?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IEnumerable<IStringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IEnumerable<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;


    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public TExt WithName
    (string fieldName, IEnumerable<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, IEnumerator<bool>? value) => 
        value != null ? aicf.WithName(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, IEnumerator<bool?>? value) => 
        value != null ? aicf.WithName(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WithName<TNum>
    (string fieldName, IEnumerator<TNum>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName<TNum>
    (string fieldName, IEnumerator<TNum?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName<TStruct>
        (string fieldName, IEnumerator<TStruct>? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? aicf.WithName(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt WithName<TStruct>
        (string fieldName, IEnumerator<TStruct?>? value, StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? aicf.WithName(fieldName, value, structToString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IEnumerator<string?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, IEnumerator<IStyledToStringObject?>? value) => 
        value != null ? aicf.WithName(fieldName, value) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IEnumerator<IFrozenString?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IEnumerator<IStringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IEnumerator<StringBuilder?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;


    [Obsolete("Warning that the type does not support IStyledToStringObject for efficient conversion")]
    public TExt WithName
    (string fieldName, IEnumerator<object?>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? aicf.WithName(fieldName, value, formatString) : stb.StyleTypeBuilder;

    public override void StateReset()
    {
        stb  = null!;
        aicf = null!;
        base.StateReset();
    }
}
