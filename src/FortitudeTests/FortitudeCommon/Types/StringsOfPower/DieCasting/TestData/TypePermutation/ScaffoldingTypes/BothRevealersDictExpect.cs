// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public class BothRevealersKeyedSubListDictExpect<TKey, TValue> : BothRevealersDictExpect<TKey, TValue, TKey, TValue, TKey, TValue, TKey>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public BothRevealersKeyedSubListDictExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , Expression<Func<List<TKey?>>>? elementFilterExpression = null
      , FieldContentHandling contentHandling = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, elementFilterExpression, contentHandling, name, srcFile, srcLine)
    {
    }
    
    public BothRevealersKeyedSubListDictExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , FieldContentHandling contentHandling = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, (Expression<Func<List<TKey?>>>?)null, contentHandling, name, srcFile, srcLine)
    {
    }
}

public class BothRevealersDictExpect<TKey, TValue> : BothRevealersDictExpect<TKey, TValue, TKey, TValue, TKey, TValue, TKey>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public BothRevealersDictExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , Expression<Func<KeyValuePredicate<TKey, TValue>>>? elementFilterExpression = null
      , FieldContentHandling contentHandling = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, elementFilterExpression, contentHandling, name, srcFile, srcLine)
    {
    }
    
    public BothRevealersDictExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , FieldContentHandling contentHandling = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, (Expression<Func<KeyValuePredicate<TKey, TValue>>>?)null, contentHandling, name, srcFile, srcLine)
    {
    }
}

public class BothRevealersDictExpect<TKey, TValue, TKFilterBase, TVFilterBase, TKSubListDerived, TVRevealBase, TKRevealBase> : 
    ValueRevealerDictExpect<TKey, TValue, TKFilterBase, TVFilterBase, TKSubListDerived, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase?
    where TValue : TVFilterBase?, TVRevealBase?
    where TKSubListDerived : TKey
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public BothRevealersDictExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , Expression<Func<PalantírReveal<TVRevealBase>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKRevealBase>>> keyRevealerExpression
      , Expression<Func<KeyValuePredicate<TKFilterBase, TVFilterBase>>>? elementFilterExpression = null
      , FieldContentHandling contentHandling = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0)
        : base(inputList, valueRevealerExpression, null, elementFilterExpression, contentHandling, name, srcFile, srcLine)
    {
        var keyRevealer = keyRevealerExpression.Compile();
        KeyRevealer = keyRevealer.Invoke();
        var expression = (MemberExpression)valueRevealerExpression.Body;
        KeyRevealerName = expression.Member.Name;
    }

    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public BothRevealersDictExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , Expression<Func<PalantírReveal<TVRevealBase>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKRevealBase>>> keyRevealerExpression
      , Expression<Func<List<TKSubListDerived?>>>? elementFilterExpression = null
      , FieldContentHandling contentHandling = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0)
        : base(inputList, valueRevealerExpression, null, elementFilterExpression, contentHandling, name, srcFile, srcLine)
    {
        var keyRevealer = keyRevealerExpression.Compile();
        KeyRevealer = keyRevealer.Invoke();
        var expression = (MemberExpression)keyRevealerExpression.Body;
        KeyRevealerName = expression.Member.Name;
    }

    public override bool InputIsEmpty => (Input?.Count ?? 0) >= 0;
    
    public PalantírReveal<TKRevealBase> KeyRevealer { get; init; }

    public Type KeyRevealerBaseType { get; init; } = typeof(TKRevealBase);
    
    public string KeyRevealerName { get; init; }

    public override ISinglePropertyTestStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry)
    {
        
        var flags = scaffoldEntry.ScaffoldingFlags;

        return flags.HasSubListFilter()
            ? scaffoldEntry.CreateStringBearerFunc(KeyType, ValueType, KeySubListFilterBaseType, KeyRevealerBaseType, ValueRevealerBaseType)()
            : (flags.HasFilterPredicate()
                ? scaffoldEntry.CreateStringBearerFunc(KeyType, ValueType, KeyFilterBaseType, ValueFilterBaseType, KeyRevealerBaseType, ValueRevealerBaseType)()
                : scaffoldEntry.CreateStringBearerFunc(KeyType, ValueType, KeyRevealerBaseType, ValueRevealerBaseType)());
    }

    public override IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry, StyleOptions stringStyle)
    {
        var createdStringBearer = CreateNewStringBearer(scaffoldEntry);
        
        if (createdStringBearer is IMoldSupportedValue<List<KeyValuePair<TKey, TValue>>> nullArrayMold)
            nullArrayMold.Value = Input;

        if (HasRestrictingSubListFilter && createdStringBearer is ISupportsSubsetDisplayKeys<TKSubListDerived> supportsSettingSubListFilter)
            supportsSettingSubListFilter.DisplayKeys = KeySubListPredicate!;
        if (HasRestrictingPredicateFilter && createdStringBearer is ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase> supportsSettingPredicateFilter)
            supportsSettingPredicateFilter.KeyValuePredicate = KeyValuePredicate!;
        if (createdStringBearer is ISupportsKeyRevealer<TKRevealBase> supportsKeyRevealer)
            supportsKeyRevealer.KeyRevealer = KeyRevealer;
        if (createdStringBearer is ISupportsValueRevealer<TVRevealBase> supportsValueRevealer)
            supportsValueRevealer.ValueRevealer = ValueRevealer;
        if (KeyFormatString != null && createdStringBearer is ISupportsKeyFormatString supportsKeyFormatString)
            supportsKeyFormatString.KeyFormatString = KeyFormatString;

        return createdStringBearer;
    }
};

