// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeOrderedCollection;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldCollection;

#pragma warning disable CS0618 // Type or member is obsolete
public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt WhenPopulatedWithFilter(string fieldName, bool[]? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<bool>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<bool[], bool>(value);
                }
                eoctb.AddElementAndGoToNextElement(item);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<bool?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<bool?[], bool?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmt>
    (string fieldName, TFmt[]? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TFmt>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<TFmt[], TFmt>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmtStruct>
    (string fieldName, TFmtStruct?[]? value, OrderedCollectionPredicate<TFmtStruct?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TFmtStruct?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<TFmtStruct?[], TFmtStruct?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TToStyle, TStylerType, TToStyleBase>
    (string fieldName, TToStyle[]? value, OrderedCollectionPredicate<TToStyleBase> filterPredicate
      , StringBearerRevealState<TStylerType> stringBearerRevealState) where TToStyle : TStylerType, TToStyleBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TToStyle>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<TToStyle[], TToStyle>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, stringBearerRevealState);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter
    (string fieldName, string?[]? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<string?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<string?[], string?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterCharSequence<TCharSeq>
        (string fieldName, TCharSeq?[]? value, OrderedCollectionPredicate<ICharSequence?> filterPredicate
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TCharSeq?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<TCharSeq?[], TCharSeq?>(value);
                }
                eoctb.AddCharSequenceElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, StringBuilder?[]? value, OrderedCollectionPredicate<StringBuilder?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<StringBuilder?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<StringBuilder?[], StringBuilder?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterMatch<T, TBase>(string fieldName, T[]? value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<T>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<T[], T>(value);
                }
                eoctb.AddMatchElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }


    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<bool?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<IReadOnlyList<bool>, bool?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<bool?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<IReadOnlyList<bool?>, bool?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmt>(string fieldName, IReadOnlyList<TFmt>? value, OrderedCollectionPredicate<TFmt> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TFmt>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<IReadOnlyList<TFmt>, TFmt>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TFmtStruct>(string fieldName, IReadOnlyList<TFmtStruct?>? value
      , OrderedCollectionPredicate<TFmtStruct?> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TFmtStruct?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<IReadOnlyList<TFmtStruct?>, TFmtStruct?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter<TToStyle, TStylerType, TToStyleBase>
    (string fieldName, IReadOnlyList<TToStyle>? value, OrderedCollectionPredicate<TToStyleBase> filterPredicate
      , StringBearerRevealState<TStylerType> stringBearerRevealState) where TToStyle : TStylerType, TToStyleBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TToStyle>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<IReadOnlyList<TToStyle>, TToStyle>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, stringBearerRevealState);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<string?>? value, OrderedCollectionPredicate<string?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<string?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<IReadOnlyList<string?>, string?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterCharSequence<TCharSeq>(string fieldName, IReadOnlyList<TCharSeq?>? value
      , OrderedCollectionPredicate<ICharSequence?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null) where TCharSeq : ICharSequence
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<TCharSeq?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<IReadOnlyList<TCharSeq?>, TCharSeq?>(value);
                }
                eoctb.AddCharSequenceElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilter(string fieldName, IReadOnlyList<StringBuilder?>? value
      , OrderedCollectionPredicate<StringBuilder?> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<StringBuilder?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<IReadOnlyList<StringBuilder?>, StringBuilder?>(value);
                }
                eoctb.AddElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }

    public TExt WhenPopulatedWithFilterMatch<T, TBase>(string fieldName, IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? formatString = null)
        where T : TBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitOrderedCollectionMold<T?>? eoctb = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                if (!filterPredicate(i, item)) continue;
                if (eoctb == null)
                {
                    stb.FieldNameJoin(fieldName);
                    eoctb = stb.Master.StartExplicitCollectionType<IReadOnlyList<T?>, T?>(value);
                }
                eoctb.AddMatchElementAndGoToNextElement(item, formatString);
            }
        }
        if (eoctb != null)
        {
            eoctb.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        return stb.StyleTypeBuilder;
    }
}
