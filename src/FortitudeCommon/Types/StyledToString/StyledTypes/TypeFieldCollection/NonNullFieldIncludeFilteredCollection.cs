using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable.Strings;
#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;

public interface INonNullFieldIncludeFilteredCollection<out T> where T : StyledTypeBuilder
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

public class NonNullFieldIncludeFilteredCollection<TExt> : RecyclableObject, INonNullFieldIncludeFilteredCollection<TExt> 
    where TExt : StyledTypeBuilder
{
    private IStyleTypeBuilderComponentAccess<TExt> stb = null!;

    private IAlwaysFieldIncludeFilteredCollection<TExt> afifc = null!;

    public NonNullFieldIncludeFilteredCollection<TExt> Initialize(IStyleTypeBuilderComponentAccess<TExt> styledComplexTypeBuilder
      , IAlwaysFieldIncludeFilteredCollection<TExt> alwaysIncludeAddAllCollectionField)
    {
        stb  = styledComplexTypeBuilder;
        afifc = alwaysIncludeAddAllCollectionField;

        return this;
    }

    public TExt WithName(string fieldName, bool[]? value, OrderedCollectionPredicate<bool> filterPredicate) => 
        value != null ? afifc.WithName(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate) => 
        value != null ? afifc.WithName(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;
    
    public TExt WithName<TNum>
    (string fieldName, TNum[]? value, OrderedCollectionPredicate<TNum> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? afifc.WithName(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WithName<TNum>(string fieldName, TNum?[]? value, OrderedCollectionPredicate<TNum?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? afifc.WithName(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WithName<TStruct>
        (string fieldName, TStruct[]? value, OrderedCollectionPredicate<TStruct> filterPredicate
          , StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? afifc.WithName(fieldName, value, filterPredicate, structToString) : stb.StyleTypeBuilder;

    public TExt WithName<TStruct>
        (string fieldName, TStruct?[]? value, OrderedCollectionPredicate<TStruct?> filterPredicate
          , StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? afifc.WithName(fieldName, value, filterPredicate, structToString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, string?[]? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? afifc.WithName(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, IStyledToStringObject?[]? value, OrderedCollectionPredicate<IStyledToStringObject?> filterPredicate) => 
        value != null ? afifc.WithName(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IFrozenString?[]? value, OrderedCollectionPredicate<IFrozenString?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? afifc.WithName(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IStringBuilder?[]? value, OrderedCollectionPredicate<IStringBuilder?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? afifc.WithName(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? afifc.WithName(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    
    [CallsObjectToString]
    public TExt WithName
    (string fieldName, object?[]? value, OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? afifc.WithName(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate) => 
        value != null ? afifc.WithName(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate) => 
        value != null ? afifc.WithName(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WithName<TNum>
    (string fieldName, IReadOnlyList<TNum>? value, OrderedCollectionPredicate<TNum> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? afifc.WithName(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WithName<TNum>
    (string fieldName, IReadOnlyList<TNum?>? value, OrderedCollectionPredicate<TNum?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? afifc.WithName(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WithName<TStruct>
        (string fieldName, IReadOnlyList<TStruct>? value, OrderedCollectionPredicate<TStruct> filterPredicate
          , StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? afifc.WithName(fieldName, value, filterPredicate, structToString) : stb.StyleTypeBuilder;

    public TExt WithName<TStruct>
        (string fieldName, IReadOnlyList<TStruct?>? value, OrderedCollectionPredicate<TStruct?> filterPredicate
          , StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? afifc.WithName(fieldName, value, filterPredicate, structToString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? afifc.WithName(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WithName(string fieldName, IReadOnlyList<IStyledToStringObject?>? value, OrderedCollectionPredicate<IStyledToStringObject?> filterPredicate) => 
        value != null ? afifc.WithName(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IReadOnlyList<IFrozenString?>? value, OrderedCollectionPredicate<IFrozenString?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? afifc.WithName(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IReadOnlyList<IStringBuilder?>? value, OrderedCollectionPredicate<IStringBuilder?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? afifc.WithName(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WithName
    (string fieldName, IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? afifc.WithName(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    
    [CallsObjectToString]
    public TExt WithName
    (string fieldName, IReadOnlyList<object?>? value, OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? afifc.WithName(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public override void StateReset()
    {
        stb = null!;
        base.StateReset();
    }
}