using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StyledToString.StyledTypes.TypeKeyValueCollection;

namespace FortitudeCommon.Types.StyledToString.StyledTypes.TypeFieldKeyValueCollection;

public partial class SelectTypeKeyValueCollectionField<TExt> where TExt : StyledTypeBuilder
{
    public TExt AlwaysAddFiltered<TKey, TValue, TKBase, TVBase>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase =>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueFormatString, keyFormatString);

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase, TVBase>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
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
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;

                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase, TVBase>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
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
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;

                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKBase, TVBase>(string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
         where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitKeyedCollectionBuilder<TKey, TValue>? ekcb = null;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var count = 0;
            foreach (var kvp in value)
            {
                if (!filterPredicate(count++, kvp.Key, kvp.Value)) continue;
                if (ekcb == null)
                {
                    ekcb        = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKBase, TVBase>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? valueFormatString = null
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitKeyedCollectionBuilder<TKey, TValue>? ekcb = null;
        stb.FieldNameJoin(fieldName);
        var count     = 0;
        var hasValue  = value?.MoveNext() ?? false;
        while (hasValue)
        {
            var kvp = value!.Current;
            if (!filterPredicate(count++, kvp.Key, kvp.Value))
            {
                hasValue = value.MoveNext();
                continue;
            }
            if (ekcb == null)
            {
                ekcb        = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            }
            ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueFormatString, keyFormatString);
            hasValue = value.MoveNext();
        }
        if (ekcb != null) ekcb.AppendCollectionComplete();
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
         where TKey : TKBase where TValue : TVBase1, TVBase2 =>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueStyler, keyFormatString);

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;

                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;

                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyFormatString);
            }
            ekcb.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
         where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitKeyedCollectionBuilder<TKey, TValue>? ekcb = null;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var count = 0;
            foreach (var kvp in value)
            {
                if (!filterPredicate(count++, kvp.Key, kvp.Value)) continue;
                if (ekcb == null)
                {
                    ekcb        = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyFormatString);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKBase, TVBase1, TVBase2>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler
      , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitKeyedCollectionBuilder<TKey, TValue>? ekcb = null;
        stb.FieldNameJoin(fieldName);
        var count    = 0;
        var hasValue = value?.MoveNext() ?? false;
        while (hasValue)
        {
            var kvp = value!.Current;
            if (!filterPredicate(count++, kvp.Key, kvp.Value))
            {
                hasValue = value.MoveNext();
                continue;
            }
            if (ekcb == null)
            {
                ekcb        = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            }
            ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyFormatString);
            hasValue = value.MoveNext();
        }
        if (ekcb != null) ekcb.AppendCollectionComplete();
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (string fieldName, IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
         where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 =>
        AlwaysAddFilteredEnumerate(fieldName, value, filterPredicate, valueStyler, keyStyler);

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (string fieldName, KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < value.Length; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;

                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyStyler);
            }
            ekcb.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFiltered<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (string fieldName, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var ekcb = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            for (var i = 0; i < value.Count; i++)
            {
                var kvp = value[i];
                if (!filterPredicate(i, kvp.Key, kvp.Value)) continue;

                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyStyler);
            }
            ekcb.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (string fieldName, IEnumerable<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitKeyedCollectionBuilder<TKey, TValue>? ekcb = null;
        stb.FieldNameJoin(fieldName);
        if (value != null)
        {
            var count = 0;
            foreach (var kvp in value)
            {
                if (!filterPredicate(count++, kvp.Key, kvp.Value)) continue;
                if (ekcb == null)
                {
                    ekcb        = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
                }
                ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyStyler);
            }
            if (ekcb != null) ekcb.AppendCollectionComplete();
        }
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }

    public TExt AlwaysAddFilteredEnumerate<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (string fieldName, IEnumerator<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        if (stb.SkipFields) return stb.StyleTypeBuilder;
        ExplicitKeyedCollectionBuilder<TKey, TValue>? ekcb = null;
        stb.FieldNameJoin(fieldName);
        var count    = 0;
        var hasValue = value?.MoveNext() ?? false;
        while (hasValue)
        {
            var kvp = value!.Current;
            if (!filterPredicate(count++, kvp.Key, kvp.Value))
            {
                hasValue = value.MoveNext();
                continue;
            }
            if (ekcb == null)
            {
                ekcb        = stb.OwningAppender.StartExplicitKeyedCollectionType<TKey, TValue>(value);
            }
            ekcb.AddKeyValueMatchAndGoToNextEntry(kvp.Key, kvp.Value, valueStyler, keyStyler);
            hasValue = value.MoveNext();
        }
        if (ekcb != null) ekcb.AppendCollectionComplete();
        else
        {
            stb.Sb.Append(stb.Settings.NullStyle);
        }
        return stb.AddGoToNext();
    }
}
