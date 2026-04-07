// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.FixtureScaffolding;

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueAnyNullEnumeratorAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerator
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerator?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);


    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerator)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerator
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueAnyEnumeratorAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IEnumerator? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerator
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerator?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);


    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerator)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerator
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueNullEnumeratorAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerator
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerator?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);


    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterate
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerator)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerator
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueEnumeratorAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerator
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerator?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);


    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerator);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterate<IEnumerator<KeyValuePair<TKey, TValue>>, TKey, TValue>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerator)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerator
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerAnyNullEnumeratorAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerAnyEnumeratorAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerNullEnumeratorAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateValueRevealer
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerEnumeratorAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateValueRevealer<IEnumerator<KeyValuePair<TKey, TValue>>, TKey, TValue, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerAnyNullEnumeratorAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateNullValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerAnyEnumeratorAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public IEnumerator? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateNullValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerNullEnumeratorAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateNullValueRevealer
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerEnumeratorAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateNullValueRevealer<IEnumerator<KeyValuePair<TKey, TValue?>>, TKey, TValue>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerAnyNullEnumeratorAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> :
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase? 
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateBothRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerAnyEnumeratorAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> :
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase? 
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerator? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateBothRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerNullEnumeratorAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> :
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase? 
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateBothRevealers
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue, TKRevealBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerEnumeratorAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> :
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase? 
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateBothRevealers<IEnumerator<KeyValuePair<TKey, TValue>>, TKey, TValue, TKRevealBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyExceptNullableStruct | KeyNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerAnyNullEnumeratorAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct 
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersKeyStruct
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersKeyStruct?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersKeyStruct);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateBothWithNullKeyRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersKeyStruct)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersKeyStruct
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyExceptNullableStruct | KeyNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerAnyEnumeratorAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct 
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersKeyStruct
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersKeyStruct?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersKeyStruct);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateBothWithNullKeyRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersKeyStruct)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersKeyStruct
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyExceptNullableStruct | KeyNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerNullEnumeratorAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct 
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersKeyStruct
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersKeyStruct?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersKeyStruct);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateBothWithNullKeyRevealers
               <StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>, TKey, TValue, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersKeyStruct)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersKeyStruct
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsAnyExceptNullableStruct | KeyNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerEnumeratorAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct 
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey?, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersKeyStruct
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersKeyStruct?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersKeyStruct);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateBothWithNullKeyRevealers<IEnumerator<KeyValuePair<TKey?, TValue>>, TKey, TValue, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersKeyStruct)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersKeyStruct
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerAnyNullEnumeratorAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase> :
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase?
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersStruct
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersStruct?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersStruct);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateBothWithNullValueRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersStruct)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersStruct
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerAnyEnumeratorAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase> :
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase?
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerator? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersStruct
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersStruct?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersStruct);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateBothWithNullValueRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersStruct)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersStruct
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerNullEnumeratorAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase> :
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase?
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersStruct
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersStruct?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersStruct);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateBothWithNullValueRevealers
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue, TKRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersStruct)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersStruct
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerEnumeratorAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase> :
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase?
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersStruct
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersStruct?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersStruct);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateBothWithNullValueRevealers<IEnumerator<KeyValuePair<TKey, TValue?>>, TKey, TValue, TKRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersStruct)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealersStruct
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerNullEnumeratorAlwaysAddAllStringBearer<TKey, TValue> :
    StructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct 
    where TValue : struct
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothStructRevealers
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothStructRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothStructRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateBothNullRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothStructRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothStructRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerEnumeratorAlwaysAddAllStringBearer<TKey, TValue> :
    StructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct 
    where TValue : struct
{
    public IEnumerator<KeyValuePair<TKey?, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothStructRevealers
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
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothStructRevealers?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey?, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothStructRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllIterateBothNullRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothStructRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothStructRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}