using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeOrderedCollection;
using FortitudeCommon.Types.StringsOfPower.Forge;

// ReSharper disable MemberCanBePrivate.Global

#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldCollection;

public partial class SelectTypeCollectionField<TExt> where TExt : TypeMolder
{
    public TExt AlwaysAddFiltered(string fieldName, bool[]? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<bool>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item                 = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                eocm ??= stb.Master.StartExplicitCollectionType<bool[], bool>(value);
                eocm.AddElementAndGoToNextElement(item);       
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, bool?[]? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<bool?>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                eocm ??= stb.Master.StartExplicitCollectionType<bool?[], bool?>(value);
                eocm.AddElementAndGoToNextElement(item); 
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
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
        ExplicitOrderedCollectionMold<TFmt>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                eocm ??= stb.Master.StartExplicitCollectionType<TFmt[], TFmt>(value);
                eocm.AddElementAndGoToNextElement(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
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
        ExplicitOrderedCollectionMold<TFmtStruct?>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                eocm ??= stb.Master.StartExplicitCollectionType<TFmtStruct?[], TFmtStruct?>(value);
                eocm.AddElementAndGoToNextElement(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TToStyle, TToStyleBase, TStylerType>
    (string fieldName, TToStyle[]? value, OrderedCollectionPredicate<TToStyleBase> filterPredicate
      , StringBearerRevealState<TStylerType> stringBearerRevealState) where TToStyle : TToStyleBase, TStylerType
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<TToStyle>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                eocm ??= stb.Master.StartExplicitCollectionType<TToStyle[], TToStyle>(value);
                eocm.AddElementAndGoToNextElement(item, stringBearerRevealState);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
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
        ExplicitOrderedCollectionMold<string>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                eocm ??= stb.Master.StartExplicitCollectionType<string?[], string>(value);
                eocm.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
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
        ExplicitOrderedCollectionMold<TCharSeq?>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                eocm ??= stb.Master.StartExplicitCollectionType<TCharSeq?[], TCharSeq?>(value);
                eocm.AddCharSequenceElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
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
        ExplicitOrderedCollectionMold<StringBuilder?>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                eocm ??= stb.Master.StartExplicitCollectionType<StringBuilder?[], StringBuilder?>(value);
                eocm.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TStyledObj, TBase>(string fieldName, TStyledObj[]? value, OrderedCollectionPredicate<TBase> filterPredicate)
        where TStyledObj : IStringBearer, TBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<TStyledObj?>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                eocm ??= stb.Master.StartExplicitCollectionType<TStyledObj?[], TStyledObj?>(value);
                eocm.AddElementAndGoToNextElement(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
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
        ExplicitOrderedCollectionMold<T?>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                eocm ??= stb.Master.StartExplicitCollectionType<T?[], T?>(value);
                eocm.AddMatchElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }


    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<bool>? value, OrderedCollectionPredicate<bool> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<bool>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                eocm ??= stb.Master.StartExplicitCollectionType<IReadOnlyList<bool>, bool>(value);
                eocm.AddElementAndGoToNextElement(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered(string fieldName, IReadOnlyList<bool?>? value, OrderedCollectionPredicate<bool?> filterPredicate)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<bool?>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                eocm ??= stb.Master.StartExplicitCollectionType<IReadOnlyList<bool?>, bool?>(value);
                eocm.AddElementAndGoToNextElement(item);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
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
        ExplicitOrderedCollectionMold<TFmt>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                eocm ??= stb.Master.StartExplicitCollectionType<IReadOnlyList<TFmt>, TFmt>(value);
                eocm.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
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
        ExplicitOrderedCollectionMold<TFmtStruct?>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                eocm ??= stb.Master.StartExplicitCollectionType<IReadOnlyList<TFmtStruct?>, TFmtStruct?>(value);
                eocm.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TToStyle, TToStyleBase, TStylerType>
    (string fieldName, IReadOnlyList<TToStyle>? value, OrderedCollectionPredicate<TToStyleBase> filterPredicate
      , StringBearerRevealState<TStylerType> stringBearerRevealState) where TToStyle : TToStyleBase, TStylerType
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        ExplicitOrderedCollectionMold<TToStyle>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                eocm ??= stb.Master.StartExplicitCollectionType<IReadOnlyList<TToStyle>, TToStyle>(value);
                eocm.AddElementAndGoToNextElement(item, stringBearerRevealState);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
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
        ExplicitOrderedCollectionMold<string?>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                eocm ??= stb.Master.StartExplicitCollectionType<IReadOnlyList<string?>, string?>(value);
                eocm.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
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
        ExplicitOrderedCollectionMold<TCharSeq?>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                eocm ??= stb.Master.StartExplicitCollectionType<IReadOnlyList<TCharSeq?>, TCharSeq?>(value);
                eocm.AddCharSequenceElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
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
        ExplicitOrderedCollectionMold<StringBuilder?>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                eocm ??= stb.Master.StartExplicitCollectionType<IReadOnlyList<StringBuilder?>, StringBuilder?>(value);
                eocm.AddElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
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
        ExplicitOrderedCollectionMold<T>? eocm = null;
        if (value != null)
        {
            for (var i = 0; i < value.Count; i++)
            {
                var item         = value[i];
                var filterResult = filterPredicate(i, item);
                if (filterResult is { IncludeItem: false })
                {
                    if (filterResult is { KeepProcessing: true })
                    {
                        i += filterResult.SkipNextCount;
                        continue;
                    }
                    break;
                }
                eocm ??= stb.Master.StartExplicitCollectionType<IReadOnlyList<T>, T>(value);
                eocm.AddMatchElementAndGoToNextElement(item, formatString);
                if (filterResult is { KeepProcessing: false }) break;
                i += filterResult.SkipNextCount;
            }
        }
        if (eocm != null)
        {
            eocm.AppendCollectionComplete();
            return stb.AddGoToNext();
        }
        stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }
}
