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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromAnyNullEnumeratorKeyValueAddAllStringBearer<TKey, TValue> : FormattedKeyValueMoldScaffold<TKey, TValue>
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterate
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings
              , ValueFormatString
              , KeyFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromAnyEnumeratorKeyValueAddAllStringBearer<TKey, TValue> : FormattedKeyValueMoldScaffold<TKey, TValue>
{
    public IEnumerator? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings
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

    public override Type KeyedCollectionType => KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings?.GetType() 
                                             ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterate
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings
              , ValueFormatString
              , KeyFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromNullEnumeratorKeyValueAddAllStringBearer<TKey, TValue> : FormattedKeyValueMoldScaffold<TKey, TValue>
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings
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

    public override Type KeyedCollectionType => KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings?.GetType() 
                                            ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterate<StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings
              , ValueFormatString
              , KeyFormatString
              , FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromEnumeratorKeyValueAddAllStringBearer<TKey, TValue> : FormattedKeyValueMoldScaffold<TKey, TValue>
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterate<IEnumerator<KeyValuePair<TKey, TValue>>, TKey, TValue>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings, ValueFormatString, KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromAnyNullEnumeratorKeyValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateValueRevealer
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromAnyEnumeratorKeyValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateValueRevealer
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromNullEnumeratorKeyValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateValueRevealer
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue, TVRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromEnumeratorKeyValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateValueRevealer<IEnumerator<KeyValuePair<TKey, TValue>>, TKey, TValue, TVRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();

}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromAnyNullEnumeratorKeyStructValueRevealerAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerMoldScaffold<TKey, TValue>
    where TValue : struct
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateNullValueRevealer
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromAnyEnumeratorKeyStructValueRevealerAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerMoldScaffold<TKey, TValue>
    where TValue : struct
{
    public IEnumerator? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerator<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateNullValueRevealer
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromNullEnumeratorKeyStructValueRevealerAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerMoldScaffold<TKey, TValue>
    where TValue : struct
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateNullValueRevealer
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromEnumeratorKeyStructValueRevealerAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerMoldScaffold<TKey, TValue>
    where TValue : struct
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerator<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateNullValueRevealer<IEnumerator<KeyValuePair<TKey, TValue?>>, TKey, TValue>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromAnyNullEnumeratorKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase?
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateBothRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromAnyEnumeratorKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase?
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerator? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateBothRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromNullEnumeratorKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase?
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateBothRevealers
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue, TKRevealBase, TVRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromEnumeratorKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase?
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateBothRevealers<IEnumerator<KeyValuePair<TKey, TValue>>, TKey, TValue, TKRevealBase, TVRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromAnyNullEnumeratorStructKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateBothWithNullKeyRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromAnyEnumeratorStructKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateBothWithNullKeyRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromNullEnumeratorStructKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateBothWithNullKeyRevealers
               <StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>, TKey, TValue, TVRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromEnumeratorStructKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey?, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateBothWithNullKeyRevealers<IEnumerator<KeyValuePair<TKey?, TValue>>, TKey, TValue, TVRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromAnyNullEnumeratorKeyRevealerStructValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateBothWithNullValueRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromAnyEnumeratorKeyRevealerStructValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerator? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateBothWithNullValueRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromNullEnumeratorKeyRevealerStructValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateBothWithNullValueRevealers
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue, TKRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromEnumeratorKeyRevealerStructValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateBothWithNullValueRevealers<IEnumerator<KeyValuePair<TKey, TValue?>>, TKey, TValue, TKRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromNullEnumeratorStructKeyRevealerStructValueRevealerAddAllStringBearer<TKey, TValue> : 
    StructKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateBothNullRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromEnumeratorStructKeyRevealerStructValueRevealerAddAllStringBearer<TKey, TValue> : 
    StructKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public IEnumerator<KeyValuePair<TKey?, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
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
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey?, TValue?>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllIterateBothNullRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}
