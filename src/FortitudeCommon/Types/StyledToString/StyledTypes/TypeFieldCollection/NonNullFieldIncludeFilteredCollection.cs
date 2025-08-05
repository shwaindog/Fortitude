using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using FortitudeCommon.Types.Mutable.Strings;
#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt WhenNonNullAddFiltered(string fieldName, bool[]? value, OrderedCollectionPredicate<bool> filterPredicate) => 
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate) => 
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;
    
    public TExt WhenNonNullAddFiltered<TNum>
    (string fieldName, TNum[]? value, OrderedCollectionPredicate<TNum> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TNum>(string fieldName, TNum?[]? value, OrderedCollectionPredicate<TNum?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TStruct>
        (string fieldName, TStruct[]? value, OrderedCollectionPredicate<TStruct> filterPredicate
          , StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, structToString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TStruct>
        (string fieldName, TStruct?[]? value, OrderedCollectionPredicate<TStruct?> filterPredicate
          , StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, structToString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
    (string fieldName, string?[]? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered(string fieldName, IStyledToStringObject?[]? value, OrderedCollectionPredicate<IStyledToStringObject?> filterPredicate) => 
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
    (string fieldName, IFrozenString?[]? value, OrderedCollectionPredicate<IFrozenString?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
    (string fieldName, IStringBuilder?[]? value, OrderedCollectionPredicate<IStringBuilder?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
    (string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    
    [CallsObjectToString]
    public TExt WhenNonNullAddFiltered
    (string fieldName, object?[]? value, OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered(string fieldName, IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate) => 
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate) => 
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TNum>
    (string fieldName, IReadOnlyList<TNum>? value, OrderedCollectionPredicate<TNum> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TNum>
    (string fieldName, IReadOnlyList<TNum?>? value, OrderedCollectionPredicate<TNum?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TNum : struct, INumber<TNum> =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TStruct>
        (string fieldName, IReadOnlyList<TStruct>? value, OrderedCollectionPredicate<TStruct> filterPredicate
          , StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, structToString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TStruct>
        (string fieldName, IReadOnlyList<TStruct?>? value, OrderedCollectionPredicate<TStruct?> filterPredicate
          , StructStyler<TStruct> structToString) where TStruct : struct =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, structToString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
    (string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered(string fieldName, IReadOnlyList<IStyledToStringObject?>? value, OrderedCollectionPredicate<IStyledToStringObject?> filterPredicate) => 
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
    (string fieldName, IReadOnlyList<IFrozenString?>? value, OrderedCollectionPredicate<IFrozenString?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
    (string fieldName, IReadOnlyList<IStringBuilder?>? value, OrderedCollectionPredicate<IStringBuilder?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
    (string fieldName, IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    
    [CallsObjectToString]
    public TExt WhenNonNullAddFiltered
    (string fieldName, IReadOnlyList<object?>? value, OrderedCollectionPredicate<object?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

}