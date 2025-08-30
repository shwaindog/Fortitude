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
    
    public TExt WhenNonNullAddFiltered<TFmt>
    (string fieldName, TFmt[]? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TToStyle, TStylerType, TToStyleBase>
        (string fieldName, TToStyle[]? value, OrderedCollectionPredicate<TToStyleBase> filterPredicate
          , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType, TToStyleBase =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, customTypeStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
    (string fieldName, string?[]? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
    (string fieldName, ICharSequence?[]? value, OrderedCollectionPredicate<ICharSequence?> filterPredicate) =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
    (string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate) =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TStyledObj>(string fieldName, TStyledObj[]? value
      , OrderedCollectionPredicate<TStyledObj> filterPredicate)
        where TStyledObj : class, IStyledToStringObject => 
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<T, TBase1, TBase2>(string fieldName, T[]? value, OrderedCollectionPredicate<TBase1?> filterPredicate
      , CustomTypeStyler<TBase2?> customTypeStyler) where T : class, TBase1, TBase2  where TBase1: class where TBase2: class   => 
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, customTypeStyler) : stb.StyleTypeBuilder;

    
    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredMatch<T, TBase>
    (string fieldName, T[]? value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase =>
        value != null ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered(string fieldName, IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate) => 
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate) => 
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TFmt>
    (string fieldName, IReadOnlyList<TFmt>? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TFmt : ISpanFormattable =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TToStyle, TStylerType, TToStyleBase>
        (string fieldName, IReadOnlyList<TToStyle>? value, OrderedCollectionPredicate<TToStyleBase> filterPredicate
          , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType, TToStyleBase =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, customTypeStyler) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
    (string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
    (string fieldName, IReadOnlyList<ICharSequence?>? value, OrderedCollectionPredicate<ICharSequence?> filterPredicate) =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered
    (string fieldName, IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate) =>
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    public TExt WhenNonNullAddFiltered<TStyledObj, TBase>(string fieldName, IReadOnlyList<TStyledObj?>? value
      , OrderedCollectionPredicate<TBase?> filterPredicate)
        where TStyledObj : class, IStyledToStringObject, TBase where TBase : class  => 
        value != null ? AlwaysAddFiltered(fieldName, value, filterPredicate) : stb.StyleTypeBuilder;

    
    [CallsObjectToString]
    public TExt WhenNonNullAddFilteredMatch<T>
    (string fieldName, IReadOnlyList<T>? value, OrderedCollectionPredicate<T> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) 
        where T : class => value != null ? AlwaysAddFilteredMatch(fieldName, value, filterPredicate, formatString) : stb.StyleTypeBuilder;

}