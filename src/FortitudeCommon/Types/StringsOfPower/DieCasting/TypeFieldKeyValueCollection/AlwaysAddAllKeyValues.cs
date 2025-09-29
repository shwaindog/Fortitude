﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt>  where TExt : TypeMolder
{
    public TExt AlwaysAddAll<TKey, TValue>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null) =>
        AlwaysAddAllEnumerate(fieldName, value, valueFormatString, keyFormatString);

    public TExt AlwaysAddAll<TKey, TValue>
        (string fieldName, KeyValuePair<TKey, TValue>[]? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue>
        (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue>
        (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            foreach (var kvp in value)
            {
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue>
        (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value, IEnumerable<KeyValuePair<TKey, TValue>>? valueAgain
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            foreach (var kvp in value)
            {
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue>
        (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value!);
            while(hasValue)
            {
                var kvp = value!.Current;
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString);
                hasValue = value.MoveNext();
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TVRevealBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
         where TValue : TVRevealBase =>
        AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyFormatString);

    public TExt AlwaysAddAll<TKey, TValue, TVRevealBase>(string fieldName, KeyValuePair<TKey, TValue>[]? value
          , PalantírReveal<TVRevealBase> valueRevealer
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TVRevealBase>(string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , PalantírReveal<TVRevealBase> valueRevealer
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TVRevealBase>(string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
          , PalantírReveal<TVRevealBase> valueRevealer
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            foreach (var kvp in value)
            {
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TVRevealBase>(string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
          , PalantírReveal<TVRevealBase> valueRevealer, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value!);
            while(hasValue)
            {
                var kvp = value!.Current;
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyFormatString);
                hasValue = value.MoveNext();
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKRevealBase where TValue : TVRevealBase =>
        AlwaysAddAllEnumerate(fieldName, value, valueRevealer, keyRevealer);

    public TExt AlwaysAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(string fieldName, KeyValuePair<TKey, TValue>[]? value
          , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKRevealBase where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TKRevealBase, TVRevealBase>(string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKRevealBase where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase> (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
          , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKRevealBase where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            foreach (var kvp in value)
            {
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
          , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKRevealBase where TValue : TVRevealBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var ekcb = stb.Master.StartExplicitKeyedCollectionType<TKey, TValue>(value!);
            while(hasValue)
            {
                var kvp = value!.Current;
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueRevealer, keyRevealer);
                hasValue = value.MoveNext();
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }
}
