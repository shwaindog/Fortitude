// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType.FixtureScaffolding;


[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromAnyNullEnumeratorKeyValueAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : TKFilterBase?
    where TValue : TVFilterBase?
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings
    {
        get => Value?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue>>();
            var shouldContinue = value.Value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Value.Current);
                shouldContinue = value.Value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromAnyEnumeratorKeyValueAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : TKFilterBase?
    where TValue : TVFilterBase?
{
    public IEnumerator? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue>>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add((KeyValuePair<TKey, TValue>)value.Current);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings?.GetType() 
     ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromNullEnumeratorKeyValueAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : TKFilterBase?
    where TValue : TVFilterBase?
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings
    {
        get => Value?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue>>();
            var shouldContinue = value.Value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Value.Current);
                shouldContinue = value.Value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterate
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue, TKFilterBase, TVFilterBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromEnumeratorKeyValueAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : TKFilterBase?
    where TValue : TVFilterBase?
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue>>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Current);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings?.GetType() 
     ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterate<IEnumerator<KeyValuePair<TKey, TValue>>, TKey, TValue, TKFilterBase, TVFilterBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromAnyNullEnumeratorKeyValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : TKFilterBase?
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
    {
        get => Value?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue>>();
            var shouldContinue = value.Value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Value.Current);
                shouldContinue = value.Value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateValueRevealer
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromAnyEnumeratorKeyValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : TKFilterBase?
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue>>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add((KeyValuePair<TKey, TValue>)value.Current);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateValueRevealer
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromNullEnumeratorKeyValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : TKFilterBase?
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
    {
        get => Value?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue>>();
            var shouldContinue = value.Value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Value.Current);
                shouldContinue = value.Value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateValueRevealer
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>
                 , TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromEnumeratorKeyValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : TKFilterBase?
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue>>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Current);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateValueRevealer
               <IEnumerator<KeyValuePair<TKey, TValue>>, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromAnyNullEnumeratorKeyStructValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : TKFilterBase?
    where TValue : struct
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
    {
        get => Value?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue?>>();
            var shouldContinue = value.Value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Value.Current);
                shouldContinue = value.Value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateNullValueRevealer
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromAnyEnumeratorKeyStructValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : TKFilterBase?
    where TValue : struct
{
    public IEnumerator? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue?>>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add((KeyValuePair<TKey, TValue?>)value.Current);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerator<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateNullValueRevealer
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromNullEnumeratorKeyStructValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : TKFilterBase?
    where TValue : struct
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
    {
        get => Value?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue?>>();
            var shouldContinue = value.Value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Value.Current);
                shouldContinue = value.Value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateNullValueRevealer
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue, TKFilterBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromEnumeratorKeyStructValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : TKFilterBase?
    where TValue : struct
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue?>>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Current);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerator<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateNullValueRevealer<IEnumerator<KeyValuePair<TKey, TValue?>>, TKey, TValue, TKFilterBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromAnyNullEnumeratorKeyRevealerValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase?, TKRevealBase?
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
    {
        get => Value?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue>>();
            var shouldContinue = value.Value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Value.Current);
                shouldContinue = value.Value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateBothRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromAnyEnumeratorKeyRevealerValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase?, TKRevealBase?
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerator? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue>>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add((KeyValuePair<TKey, TValue>)value.Current);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType =>
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateBothRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromNullEnumeratorKeyRevealerValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase?, TKRevealBase?
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
    {
        get => Value?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue>>();
            var shouldContinue = value.Value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Value.Current);
                shouldContinue = value.Value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateBothRevealers
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>
                 , TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromEnumeratorKeyRevealerValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase?, TKRevealBase?
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue>>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Current);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType =>
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateBothRevealers
               <IEnumerator<KeyValuePair<TKey, TValue>>, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsAnyExceptNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromAnyNullEnumeratorStructKeyRevealerValueRevealerAddFilteredStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
    {
        get => Value?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey?, TValue>>();
            var shouldContinue = value.Value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Value.Current);
                shouldContinue = value.Value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateBothWithNullKeyRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsAnyExceptNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromAnyEnumeratorStructKeyRevealerValueRevealerAddFilteredStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey?, TValue>>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add((KeyValuePair<TKey?, TValue>)value.Current);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType =>
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateBothWithNullKeyRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsAnyExceptNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromNullEnumeratorStructKeyRevealerValueRevealerAddFilteredStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
    {
        get => Value?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey?, TValue>>();
            var shouldContinue = value.Value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Value.Current);
                shouldContinue = value.Value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateBothWithNullKeyRevealers
               <StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>, TKey, TValue, TVFilterBase, TVRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsAnyExceptNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromEnumeratorStructKeyRevealerValueRevealerAddFilteredStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey?, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey?, TValue>>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Current);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType =>
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateBothWithNullKeyRevealers<IEnumerator<KeyValuePair<TKey?, TValue>>, TKey, TValue, TVFilterBase, TVRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromAnyNullEnumeratorKeyRevealerStructValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase?, TKRevealBase?
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
    {
        get => Value?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue?>>();
            var shouldContinue = value.Value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Value.Current);
                shouldContinue = value.Value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateBothWithNullValueRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromAnyEnumeratorKeyRevealerStructValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase?, TKRevealBase?
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerator? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue?>>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add((KeyValuePair<TKey, TValue?>)value.Current);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType =>
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateBothWithNullValueRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromNullEnumeratorKeyRevealerStructValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase?, TKRevealBase?
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
    {
        get => Value?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue?>>();
            var shouldContinue = value.Value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Value.Current);
                shouldContinue = value.Value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateBothWithNullValueRevealers
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue, TKFilterBase, TKRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromEnumeratorKeyRevealerStructValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase?, TKRevealBase?
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue?>>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Current);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType =>
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateBothWithNullValueRevealers<IEnumerator<KeyValuePair<TKey, TValue?>>, TKey, TValue, TKFilterBase, TKRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromNullEnumeratorStructKeyRevealerStructValueRevealerAddFilteredStringBearer<TKey, TValue>
    : FilteredKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
    {
        get => Value?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey?, TValue?>>();
            var shouldContinue = value.Value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Value.Current);
                shouldContinue = value.Value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateBothNullRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromEnumeratorStructKeyRevealerStructValueRevealerAddFilteredStringBearer<TKey, TValue>
    : FilteredKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public IEnumerator<KeyValuePair<TKey?, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey?, TValue?>>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Current);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override Type KeyedCollectionType =>
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey?, TValue?>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredIterateBothNullRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}