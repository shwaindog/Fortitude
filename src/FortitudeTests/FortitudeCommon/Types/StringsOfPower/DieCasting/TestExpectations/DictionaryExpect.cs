// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;

public interface IKeyedCollectionExpect : IFormatExpectation
{
    Type KeyType { get; }
    
    Type KeyUnderlyingType { get; }
    
    Type ValueType { get; }
    Type ValueUnderlyingType { get; }
    
    bool KeyTypeIsNullable { get; }
    bool KeyTypeIsNullableStruct { get; }
    bool KeyTypeIsNotNullableStruct { get; }
    bool ValueTypeIsNullableStruct { get; }
    bool ValueTypeIsNotNullableStruct { get; }
    bool KeyTypeIsStruct { get; }
    bool KeyTypeIsClass { get; }
    bool ContainsNullKeys { get; }
    
    bool ContainsDuplicateKeys { get; }

    bool HasRestrictingPredicateFilter { get; }
    bool HasRestrictingSubListFilter { get; }
    
    string? KeyFormatString { get; }
}

public class KeyedSubListDictionaryExpect<TKey, TValue> : DictionaryExpect<TKey, TValue, TKey, TValue, TKey>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public KeyedSubListDictionaryExpect(List<KeyValuePair<TKey, TValue>>? inputList, string? valueFormatString = null, string? keyFormatString = null
      , Expression<Func<List<TKey>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueFormatString, keyFormatString, elementFilterExpression, formatFlags, name, srcFile, srcLine)
    {
    }
    
    public KeyedSubListDictionaryExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , Expression<Func<List<TKey>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, null, null, elementFilterExpression, formatFlags, name, srcFile, srcLine)
    {
    }
    
    public KeyedSubListDictionaryExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, null, null, (Expression<Func<List<TKey>>>?)null, formatFlags, name, srcFile, srcLine)
    {
    }
}

public class KeyedNullStructValueSubListDictionaryExpect<TKey, TValue> : DictionaryExpect<TKey, TValue?, TKey, TValue?, TKey>
    where TValue : struct
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public KeyedNullStructValueSubListDictionaryExpect(List<KeyValuePair<TKey, TValue?>>? inputList, string? valueFormatString = null, string? keyFormatString = null
      , Expression<Func<List<TKey>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueFormatString, keyFormatString, elementFilterExpression, formatFlags, name, srcFile, srcLine)
    {
    }
    
    public KeyedNullStructValueSubListDictionaryExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , Expression<Func<List<TKey>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, null, null, elementFilterExpression, formatFlags, name, srcFile, srcLine)
    {
    }
    
    public KeyedNullStructValueSubListDictionaryExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, null, null, (Expression<Func<List<TKey>>>?)null, formatFlags, name, srcFile, srcLine)
    {
    }
}

public class DictionaryExpect<TKey, TValue> : DictionaryExpect<TKey, TValue, TKey, TValue, TKey>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public DictionaryExpect(List<KeyValuePair<TKey, TValue>>? inputList, string? valueFormatString = null, string? keyFormatString = null
      , Expression<Func<KeyValuePredicate<TKey, TValue>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueFormatString, keyFormatString, elementFilterExpression, formatFlags, name, srcFile, srcLine)
    {
    }
    
    public DictionaryExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , Expression<Func<KeyValuePredicate<TKey, TValue>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, null, null, elementFilterExpression, formatFlags, name, srcFile, srcLine)
    {
    }
    
    public DictionaryExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, null, null, (Expression<Func<KeyValuePredicate<TKey, TValue>>>?)null, formatFlags, name, srcFile, srcLine)
    {
    }
}

public class DictionaryExpect<TKey, TValue, TKFilterBase, TVFilterBase, TKSubListDerived> : 
    ExpectBase<List<KeyValuePair<TKey, TValue>>?>, IKeyedCollectionExpect
    where TKey : TKFilterBase?
    where TValue : TVFilterBase?
    where TKSubListDerived : TKey?
{
    private readonly string? filterName;

    private Type? keyType;
    private Type? valueType;

    private Type? keyFilterBaseType;
    private Type? valueFilterBaseType;
    private Type? subListDerivedType;

    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public DictionaryExpect(List<KeyValuePair<TKey, TValue>>? inputList, string? valueFormatString = null, string? keyFormatString = null
      , Expression<Func<KeyValuePredicate<TKFilterBase, TVFilterBase>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0)
        : base(inputList, valueFormatString, formatFlags,
               name
            ?? ((elementFilterExpression?.Body as MemberExpression)?.Member.Name)
            ?? (inputList != null
                   ? $"List<{typeof(KeyValuePair<TKey, TValue>).CachedCSharpNameNoConstraints()}> {{ Count: {inputList.Count}}}"
                   : null), srcFile, srcLine)
    {
        KeyFormatString = keyFormatString;
        if (elementFilterExpression != null)
        {
            var elementFilter = elementFilterExpression.Compile();
            KeyValuePredicate = elementFilter.Invoke();
            var expression = (MemberExpression)elementFilterExpression.Body;
            filterName = expression.Member.Name;
        }
        else { KeyValuePredicate = ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate; }
    }

    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public DictionaryExpect(List<KeyValuePair<TKey, TValue>>? inputList, string? valueFormatString = null, string? keyFormatString = null
      , Expression<Func<List<TKSubListDerived>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0)
        : base(inputList, valueFormatString, formatFlags,
               name
            ?? ((elementFilterExpression?.Body as MemberExpression)?.Member.Name)
            ?? (inputList != null
                   ? $"List<{typeof(KeyValuePair<TKey, TValue>).CachedCSharpNameNoConstraints()}> {{ Count: {inputList.Count}}}"
                   : null), srcFile, srcLine)
    {
        KeyFormatString = keyFormatString;
        if (elementFilterExpression != null)
        {
            var elementFilter = elementFilterExpression.Compile();
            KeySubListPredicate = elementFilter.Invoke();
            var expression = (MemberExpression)elementFilterExpression.Body;
            filterName = expression.Member.Name;
        }
        else { KeySubListPredicate = ISupportsSubsetDisplayKeys<TKSubListDerived?>.GetAllKeysSubListPredicate(inputList); }
    }

    public override bool InputIsEmpty => (Input?.Count ?? 0) >= 0;

    public bool KeyTypeIsNullable => KeyType.IsNullable() || ContainsNullKeys;
    public bool KeyTypeIsNullableStruct => KeyType.IsNullable();
    public bool KeyTypeIsNotNullableStruct => !KeyTypeIsNullableStruct;
    public bool KeyTypeIsClass => !KeyTypeIsStruct;
    public bool KeyTypeIsStruct => KeyType.IsValueType;

    public bool ValueTypeIsNullableStruct => ValueType.IsNullable();
    public bool ValueTypeIsNotNullableStruct => !ValueTypeIsNullableStruct;

    public bool ContainsNullKeys
    {
        get { return Input?.Any(kvp => kvp.Key == null) ?? false; }
    }

    public bool ContainsDuplicateKeys
    {
        get { return (Input?.GroupBy(kvp => kvp.Key).Select(grp => grp.Count()).Max() ?? 0)  <= 1; }
    }

    public Type KeyType => keyType ??= typeof(TKey);
    public Type ValueType => valueType ??= typeof(TValue);

    public Type KeyUnderlyingType => KeyType.IfNullableGetUnderlyingTypeOrThis();
    public Type ValueUnderlyingType => ValueType.IfNullableGetUnderlyingTypeOrThis();

    public Type KeyFilterBaseType => keyFilterBaseType ??= typeof(TKFilterBase);
    public Type ValueFilterBaseType => valueFilterBaseType ??= typeof(TVFilterBase);
    public Type KeySubListFilterBaseType => subListDerivedType ??= typeof(TKSubListDerived);
    
    public string? KeyFormatString { get; private set; }

    public override Type CoreType => typeof(TKey).IfNullableGetUnderlyingTypeOrThis();

    public bool HasRestrictingPredicateFilter =>
        KeyValuePredicate != null
     && !Equals(KeyValuePredicate, ISupportsKeyedCollectionPredicate<TKey, TValue>.GetNoFilterPredicate);

    public bool HasRestrictingSubListFilter
    {
        get
        {
            var kFilterBases = Input?.Select(kvp => kvp.Key).Distinct().ToList();
            return KeySubListPredicate != null
                || kFilterBases?.Intersect(KeySubListPredicate?.OfType<TKey>() ?? []).Count() < (kFilterBases?.Count ?? 0);
        }
    }

    public override string ShortTestName
    {
        get
        {
            {
                var result = new MutableString();
                result.Append(base.ShortTestName);
                if (filterName != null && filterName != Name)
                {
                    result.Append("_")
                          .Append(filterName)
                          .Append("_");
                }

                return result.ToString();
            }
        }
    }

    public KeyValuePredicate<TKFilterBase, TVFilterBase>? KeyValuePredicate { get; init; }

    public IReadOnlyList<TKSubListDerived?>? KeySubListPredicate { get; init; }

    public override ISinglePropertyTestStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry)
    {
        var flags = scaffoldEntry.ScaffoldingFlags;

        return flags.HasSubListFilter()
            ? scaffoldEntry.CreateStringBearerFunc(KeyType, ValueType, KeySubListFilterBaseType)()
            : (flags.HasFilterPredicate()
                ? scaffoldEntry.CreateStringBearerFunc(KeyType, ValueType, KeyFilterBaseType, ValueFilterBaseType)()
                : scaffoldEntry.CreateStringBearerFunc(KeyType, ValueType)());
    }

    public override IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry, StyleOptions stringStyle)
    {
        var createdStringBearer = CreateNewStringBearer(scaffoldEntry);

        var acceptsNullables = scaffoldEntry.ScaffoldingFlags.HasAcceptsNullables();

        if (createdStringBearer is IMoldSupportedValue<List<KeyValuePair< TKey, TValue>>> nullArrayMold)
            nullArrayMold.Value = Input;

        if (HasRestrictingSubListFilter && createdStringBearer is ISupportsSubsetDisplayKeys<TKSubListDerived> supportsSettingSubListFilter)
            supportsSettingSubListFilter.DisplayKeys = KeySubListPredicate!;
        if (HasRestrictingPredicateFilter && createdStringBearer is ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase> supportsSettingPredicateFilter)
            supportsSettingPredicateFilter.KeyValuePredicate = KeyValuePredicate!;
        if (ValueFormatString != null && createdStringBearer is ISupportsValueFormatString supportsValueFormatString)
            supportsValueFormatString.ValueFormatString = ValueFormatString;
        if (KeyFormatString != null && createdStringBearer is ISupportsKeyFormatString supportsKeyFormatString)
            supportsKeyFormatString.KeyFormatString = KeyFormatString;

        return createdStringBearer;
    }

    protected override void AdditionalToStringExpectFields(IStringBuilder sb, ScaffoldingStringBuilderInvokeFlags forThisScaffold)
    {
        if (filterName != null)
        {
            sb.Append(", FilterName: ")
              .Append(filterName);
        }
        AddExpectedResultsList(sb);
    }
};

