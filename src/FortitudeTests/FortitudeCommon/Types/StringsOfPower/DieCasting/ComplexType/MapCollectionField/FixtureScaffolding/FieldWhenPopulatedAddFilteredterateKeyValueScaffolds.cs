// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.FixtureScaffolding;

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueAnyNullEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> :
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBoth
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBoth?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBoth)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBoth
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueAnyEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> :
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?
{
    public IEnumerator? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBoth
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBoth?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBoth)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBoth
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueNullEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> :
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBoth
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBoth?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterate
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue, TKFilterBase, TVFilterBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBoth)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBoth
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> :
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBoth
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBoth?.GetType() ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterate
               <IEnumerator<KeyValuePair<TKey, TValue>>, TKey, TValue, TKFilterBase, TVFilterBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBoth)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBoth
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerAnyNullEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerAnyEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerNullEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateValueRevealer
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>
                 , TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateValueRevealer
               <IEnumerator<KeyValuePair<TKey, TValue>>, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerAnyNullEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : struct
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateNullValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerAnyEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : struct
{
    public IEnumerator? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerator<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateNullValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerNullEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : struct
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateNullValueRevealer
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue, TKFilterBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : struct
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerator<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateNullValueRevealer
               <IEnumerator<KeyValuePair<TKey, TValue?>>, TKey, TValue, TKFilterBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerAnyNullEnumeratorWhenPopulatedWithFilterStringBearer
    <TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateBothRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerAnyEnumeratorWhenPopulatedWithFilterStringBearer
    <TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerator? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers?.GetType() 
     ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateBothRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerNullEnumeratorWhenPopulatedWithFilterStringBearer
    <TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateBothRevealers
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>
                 , TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerEnumeratorWhenPopulatedWithFilterStringBearer
    <TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers?.GetType() 
     ?? typeof(IEnumerator<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateBothRevealers
               <IEnumerator<KeyValuePair<TKey, TValue>>, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerAnyNullEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateBothWithNullKeyRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerAnyEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers?.GetType() 
     ?? typeof(IEnumerator<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateBothWithNullKeyRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerNullEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateBothWithNullKeyRevealers
               <StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>, TKey, TValue, TVFilterBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey?, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers?.GetType() 
     ?? typeof(IEnumerator<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateBothWithNullKeyRevealers
               <IEnumerator<KeyValuePair<TKey?, TValue>>, TKey, TValue, TVFilterBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerAnyNullEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateBothWithNullValueRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerAnyEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerator? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers?.GetType() 
     ?? typeof(IEnumerator<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateBothWithNullValueRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerNullEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateBothWithNullValueRevealers
               <StructEnumerator<IEnumerator<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue, TKFilterBase, TKRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers?.GetType() 
     ?? typeof(IEnumerator<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateBothWithNullValueRevealers
               <IEnumerator<KeyValuePair<TKey, TValue?>>, TKey, TValue, TKFilterBase, TKRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerNullEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue>
    : FilteredStructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers?.GetType() 
     ?? typeof(StructEnumerator<IEnumerator<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateBothNullRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerEnumeratorWhenPopulatedWithFilterStringBearer<TKey, TValue>
    : FilteredStructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public IEnumerator<KeyValuePair<TKey?, TValue?>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
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
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers?.GetType() 
     ?? typeof(IEnumerator<KeyValuePair<TKey?, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterIterateBothNullRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

