// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.FixtureScaffolding;

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueAnyNullEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBoth
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBoth?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBoth)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBoth
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueAnyEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IEnumerator? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBoth
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBoth?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBoth)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBoth
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueNullEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBoth
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBoth?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterate
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBoth)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBoth
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBoth
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBoth?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterate<IEnumerator<KeyValuePair<TKey, TValue>>, TKey, TValue>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBoth)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBoth
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerAnyNullEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerAnyEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerNullEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateValueRevealer
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateValueRevealer<IEnumerator<KeyValuePair<TKey, TValue>>, TKey, TValue, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerAnyNullEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateNullValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerAnyEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public IEnumerator? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateNullValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerNullEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateNullValueRevealer
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateNullValueRevealer<IEnumerator<KeyValuePair<TKey, TValue?>>, TKey, TValue>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerAnyNullEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateBothRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerAnyEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerator? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateBothRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerNullEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateBothRevealers
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue, TKRevealBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateBothRevealers
               <IEnumerator<KeyValuePair<TKey, TValue>>, TKey, TValue, TKRevealBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyExceptNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerAnyNullEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateBothNullKeyRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyExceptNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerAnyEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateBothNullKeyRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyExceptNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerNullEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateBothNullKeyRevealers
               <StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>, TKey, TValue, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsAnyExceptNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey?, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateBothNullKeyRevealers
               <IEnumerator<KeyValuePair<TKey?, TValue>>, TKey, TValue, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerAnyNullEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateBothWithNullValueRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerAnyEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerator? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateBothWithNullValueRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerNullEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateBothWithNullValueRevealers
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue, TKRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateBothWithNullValueRevealers
               <IEnumerator<KeyValuePair<TKey, TValue?>>, TKey, TValue, TKRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerNullEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue> : 
    StructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateBothNullRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerEnumeratorWhenNonNullAddAllStringBearer<TKey, TValue> : 
    StructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public IEnumerator<KeyValuePair<TKey?, TValue?>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey?, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllIterateBothNullRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

