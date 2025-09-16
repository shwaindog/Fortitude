using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeOrderedCollection;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt AlwaysAddFiltered(string fieldName, bool[]? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionBuilder<bool>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    eoctb = stb.OwningAppender.StartExplicitCollectionType<bool[], bool>(value);
                }
                eoctb.AddElementAndGoToNextElement(item);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionBuilder<bool?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    eoctb = stb.OwningAppender.StartExplicitCollectionType<bool?[], bool?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmt>
    (string fieldName, TFmt[]? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionBuilder<TFmt>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    eoctb = stb.OwningAppender.StartExplicitCollectionType<TFmt[], TFmt>(value);
                }
                eoctb.AddElementAndGoToNextElement(item);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmtStruct>
    (string fieldName, TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionBuilder<TFmtStruct?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    eoctb = stb.OwningAppender.StartExplicitCollectionType<TFmtStruct?[], TFmtStruct?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TToStyle, TToStyleBase, TStylerType>
    (string fieldName, TToStyle[]? value, OrderedCollectionPredicate<TToStyleBase> filterPredicate
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionBuilder<TToStyle>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    eoctb = stb.OwningAppender.StartExplicitCollectionType<TToStyle[], TToStyle>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, customTypeStyler);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered
    (string fieldName, string?[]? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionBuilder<string>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    eoctb = stb.OwningAppender.StartExplicitCollectionType<string?[], string>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSequence<TCharSeq>
    (string fieldName, TCharSeq?[]? value, OrderedCollectionPredicate<ICharSequence?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionBuilder<TCharSeq?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    eoctb = stb.OwningAppender.StartExplicitCollectionType<TCharSeq?[], TCharSeq?>(value!);
                }
                eoctb.AddCharSequenceElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionBuilder<StringBuilder?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    eoctb = stb.OwningAppender.StartExplicitCollectionType<StringBuilder?[], StringBuilder?>(value!);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TStyledObj, TBase>(string fieldName, TStyledObj[]? value, OrderedCollectionPredicate<TBase> filterPredicate)
        where TStyledObj : IStyledToStringObject, TBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionBuilder<TStyledObj?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    eoctb = stb.OwningAppender.StartExplicitCollectionType<TStyledObj?[], TStyledObj?>(value!);
                }
                eoctb.AddElementAndGoToNextElement(item);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredMatch<T, TBase>(string fieldName, T[]? value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionBuilder<T?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    eoctb = stb.OwningAppender.StartExplicitCollectionType<T?[], T?>(value!);
                }
                eoctb.AddMatchElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }


    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionBuilder<bool>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    eoctb = stb.OwningAppender.StartExplicitCollectionType<IReadOnlyList<bool>, bool>(value);
                }
                eoctb.AddElementAndGoToNextElement(item);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionBuilder<bool?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    eoctb = stb.OwningAppender.StartExplicitCollectionType<IReadOnlyList<bool?>, bool?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmt>(string fieldName, IReadOnlyList<TFmt>? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionBuilder<TFmt>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    eoctb = stb.OwningAppender.StartExplicitCollectionType<IReadOnlyList<TFmt>, TFmt>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionBuilder<TFmtStruct?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    eoctb = stb.OwningAppender.StartExplicitCollectionType<IReadOnlyList<TFmtStruct?>, TFmtStruct?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TToStyle, TToStyleBase, TStylerType>
    (string fieldName, IReadOnlyList<TToStyle>? value, OrderedCollectionPredicate<TToStyleBase> filterPredicate
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionBuilder<TToStyle>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    eoctb = stb.OwningAppender.StartExplicitCollectionType<IReadOnlyList<TToStyle>, TToStyle>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, customTypeStyler);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionBuilder<string?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    eoctb = stb.OwningAppender.StartExplicitCollectionType<IReadOnlyList<string?>, string?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredCharSequence<TCharSeq>(string fieldName, IReadOnlyList<TCharSeq?>? value
      , OrderedCollectionPredicate<ICharSequence?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionBuilder<TCharSeq?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    eoctb = stb.OwningAppender.StartExplicitCollectionType<IReadOnlyList<TCharSeq?>, TCharSeq?>(value);
                }
                eoctb.AddCharSequenceElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<StringBuilder?>? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionBuilder<StringBuilder?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    eoctb = stb.OwningAppender.StartExplicitCollectionType<IReadOnlyList<StringBuilder?>, StringBuilder?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    [CallsObjectToString]
    public TExt AlwaysAddFilteredMatch<T, TBase>(string fieldName, IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : class, TBase
        where TBase : class
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionBuilder<T>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    eoctb = stb.OwningAppender.StartExplicitCollectionType<IReadOnlyList<T>, T>(value);
                }
                eoctb.AddMatchElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }
}
