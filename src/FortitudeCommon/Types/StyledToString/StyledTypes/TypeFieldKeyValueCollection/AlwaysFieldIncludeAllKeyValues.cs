// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeKeyValueCollection;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt>  where TExt : StyledTypeBuilder
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
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
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
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
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
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
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
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
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
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value!);
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

    public TExt AlwaysAddAll<TKey, TValue, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
         where TValue : TVBase =>
        AlwaysAddAllEnumerate(fieldName, value, valueStyler, keyFormatString);

    public TExt AlwaysAddAll<TKey, TValue, TVBase>(string fieldName, KeyValuePair<TKey, TValue>[]? value
          , CustomTypeStyler<TVBase> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TVBase>(string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , CustomTypeStyler<TVBase> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TVBase>(string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
          , CustomTypeStyler<TVBase> valueStyler
          , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            foreach (var kvp in value)
            {
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TVBase>(string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
          , CustomTypeStyler<TVBase> valueStyler, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value!);
            while(hasValue)
            {
                var kvp = value!.Current;
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyFormatString);
                hasValue = value.MoveNext();
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TKBase, TVBase>(string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase =>
        AlwaysAddAllEnumerate(fieldName, value, valueStyler, keyStyler);

    public TExt AlwaysAddAll<TKey, TValue, TKBase, TVBase>(string fieldName, KeyValuePair<TKey, TValue>[]? value
          , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyStyler);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAll<TKey, TValue, TKBase, TVBase>(string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
          , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyStyler);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TKBase, TVBase> (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
          , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            foreach (var kvp in value)
            {
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyStyler);
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddAllEnumerate<TKey, TValue, TKBase, TVBase>(string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
          , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        var hasValue = value?.MoveNext() ?? false;
        if (hasValue)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value!);
            while(hasValue)
            {
                var kvp = value!.Current;
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyStyler);
                hasValue = value.MoveNext();
            }
            ekcb.AppendCollectionComplete();
        }
        else
            stb.Sb.Append(stb.Settings.NullStyle);
        return stb.AddGoToNext();
    }
}
