// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public class NullClassValueRevealerKeyedSubListDictExpect<TKey, TValue> : ValueRevealerDictExpect<TKey, TValue?, TKey, TValue?, TKey, TValue>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public NullClassValueRevealerKeyedSubListDictExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , string? keyFormatString = null
      , Expression<Func<List<TKey>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyFormatString, elementFilterExpression, formatFlags, name, srcFile, srcLine)
    {
    }
    
    public NullClassValueRevealerKeyedSubListDictExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<List<TKey>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, null, elementFilterExpression, formatFlags, name, srcFile, srcLine)
    {
    }
    
    public NullClassValueRevealerKeyedSubListDictExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, null, (Expression<Func<List<TKey>>>?)null, formatFlags, name, srcFile, srcLine)
    {
    }
}

public class NullStructValueRevealerKeyedSubListDictExpect<TKey, TValue> : ValueRevealerDictExpect<TKey, TValue?, TKey, TValue?, TKey, TValue>
    where TValue : struct
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public NullStructValueRevealerKeyedSubListDictExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , string? keyFormatString = null
      , Expression<Func<List<TKey>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyFormatString, elementFilterExpression, formatFlags, name, srcFile, srcLine)
    {
    }
    
    public NullStructValueRevealerKeyedSubListDictExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<List<TKey>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, null, elementFilterExpression, formatFlags, name, srcFile, srcLine)
    {
    }
    
    public NullStructValueRevealerKeyedSubListDictExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, null, (Expression<Func<List<TKey>>>?)null, formatFlags, name, srcFile, srcLine)
    {
    }
}

public class ValueRevealerKeyedSubListDictExpect<TKey, TValue> : ValueRevealerDictExpect<TKey, TValue, TKey, TValue, TKey, TValue>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public ValueRevealerKeyedSubListDictExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , string? keyFormatString = null
      , Expression<Func<List<TKey>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyFormatString, elementFilterExpression, formatFlags, name, srcFile, srcLine)
    {
    }
    
    public ValueRevealerKeyedSubListDictExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<List<TKey>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, null, elementFilterExpression, formatFlags, name, srcFile, srcLine)
    {
    }
    
    public ValueRevealerKeyedSubListDictExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, null, (Expression<Func<List<TKey>>>?)null, formatFlags, name, srcFile, srcLine)
    {
    }
}

public class NullStructValueRevealerDictExpect<TKey, TValue> : ValueRevealerDictExpect<TKey, TValue?, TKey, TValue?, TKey, TValue>
 where TValue : struct
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public NullStructValueRevealerDictExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , string? keyFormatString = null
      , Expression<Func<KeyValuePredicate<TKey, TValue?>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyFormatString, elementFilterExpression, formatFlags, name, srcFile, srcLine)
    {
    }
    
    public NullStructValueRevealerDictExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<KeyValuePredicate<TKey, TValue?>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, null, elementFilterExpression, formatFlags, name, srcFile, srcLine)
    {
    }
    
    public NullStructValueRevealerDictExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, null, (Expression<Func<KeyValuePredicate<TKey, TValue?>>>?)null, formatFlags, name, srcFile, srcLine)
    {
    }

    // public override ISinglePropertyTestStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry)
    // {
    //     
    //     var flags = scaffoldEntry.ScaffoldingFlags;
    //
    //     return flags.HasSubListFilter()
    //         ? scaffoldEntry.CreateStringBearerFunc(KeyType, typeof(TValue?), KeySubListFilterBaseType, ValueRevealerBaseType)()
    //         : (flags.HasFilterPredicate()
    //             ? scaffoldEntry.CreateStringBearerFunc(KeyType, typeof(TValue?), KeyFilterBaseType, typeof(TValue?), ValueRevealerBaseType)()
    //             : scaffoldEntry.CreateStringBearerFunc(KeyType, typeof(TValue?), ValueRevealerBaseType)());
    // }
}

public class NullClassValueRevealerDictExpect<TKey, TValue> : ValueRevealerDictExpect<TKey, TValue?, TKey, TValue?, TKey, TValue>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public NullClassValueRevealerDictExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , string? keyFormatString = null
      , Expression<Func<KeyValuePredicate<TKey, TValue?>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyFormatString, elementFilterExpression, formatFlags, name, srcFile, srcLine)
    {
    }
    
    public NullClassValueRevealerDictExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<KeyValuePredicate<TKey, TValue?>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, null, elementFilterExpression, formatFlags, name, srcFile, srcLine)
    {
    }
    
    public NullClassValueRevealerDictExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, null, (Expression<Func<KeyValuePredicate<TKey, TValue?>>>?)null, formatFlags, name, srcFile, srcLine)
    {
    }
}

public class ValueRevealerDictExpect<TKey, TValue> : ValueRevealerDictExpect<TKey, TValue, TKey, TValue, TKey, TValue>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public ValueRevealerDictExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , string? keyFormatString = null
      , Expression<Func<KeyValuePredicate<TKey, TValue>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyFormatString, elementFilterExpression, formatFlags, name, srcFile, srcLine)
    {
    }
    
    public ValueRevealerDictExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<KeyValuePredicate<TKey, TValue>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, null, elementFilterExpression, formatFlags, name, srcFile, srcLine)
    {
    }
    
    public ValueRevealerDictExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, null, (Expression<Func<KeyValuePredicate<TKey, TValue>>>?)null, formatFlags, name, srcFile, srcLine)
    {
    }
}

public class ValueRevealerDictExpect<TKey, TValue, TKFilterBase, TVFilterBase, TKSubListDerived, TVRevealBase> : 
    DictionaryExpect<TKey, TValue, TKFilterBase, TVFilterBase, TKSubListDerived>
    where TKey : TKFilterBase
    where TValue : TVFilterBase?
    where TKSubListDerived : TKey
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public ValueRevealerDictExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , Expression<Func<PalantírReveal<TVRevealBase>>> valueRevealerExpression
      , string? keyFormatString = null
      , Expression<Func<KeyValuePredicate<TKFilterBase, TVFilterBase>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0)
        : base(inputList, null, keyFormatString, elementFilterExpression, formatFlags, name, srcFile, srcLine)
    {
        var valueRevealer = valueRevealerExpression.Compile();
        ValueRevealer = valueRevealer.Invoke();
        var expression = (MemberExpression)valueRevealerExpression.Body;
        ValueRevealerName = expression.Member.Name;
    }

    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public ValueRevealerDictExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , Expression<Func<PalantírReveal<TVRevealBase>>> valueRevealerExpression
      , string? keyFormatString = null
      , Expression<Func<List<TKSubListDerived>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0)
        : base(inputList, null, keyFormatString, elementFilterExpression, formatFlags, name, srcFile, srcLine)
    {
        var valueRevealer = valueRevealerExpression.Compile();
        ValueRevealer = valueRevealer.Invoke();
        var expression = (MemberExpression)valueRevealerExpression.Body;
        ValueRevealerName = expression.Member.Name;
    }

    public override bool InputIsEmpty => (Input?.Count ?? 0) >= 0;
    
    public PalantírReveal<TVRevealBase> ValueRevealer { get; init; }

    public Type ValueRevealerBaseType { get; init; } = typeof(TVRevealBase);
    
    public string ValueRevealerName { get; init; }

    public override ISinglePropertyTestStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry)
    {
        var flags = scaffoldEntry.ScaffoldingFlags;

        return flags.HasSubListFilter()
            ? (flags.HasAcceptsNullableStruct() 
                ? scaffoldEntry.CreateStringBearerFunc(KeyType, ValueUnderlyingType, KeySubListFilterBaseType)()
                : scaffoldEntry.CreateStringBearerFunc(KeyType, ValueType, KeySubListFilterBaseType, ValueRevealerBaseType)())
            : (flags.HasFilterPredicate()
                ? ( flags.HasAcceptsNullableStruct()
                    ? scaffoldEntry.CreateStringBearerFunc(KeyType, ValueUnderlyingType, KeyFilterBaseType)()
                    : scaffoldEntry.CreateStringBearerFunc(KeyType, ValueType, KeyFilterBaseType, ValueFilterBaseType, ValueRevealerBaseType)())
                : ( flags.HasAcceptsNullableStruct()
                    ? scaffoldEntry.CreateStringBearerFunc(KeyType, ValueUnderlyingType)()
                    : scaffoldEntry.CreateStringBearerFunc(KeyType, ValueType, ValueRevealerBaseType)()
                ));
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
        if (createdStringBearer is ISupportsValueRevealer<TVRevealBase> supportsValueValueRevealer)
            supportsValueValueRevealer.ValueRevealer = ValueRevealer;
        if (KeyFormatString != null && createdStringBearer is ISupportsKeyFormatString supportsKeyFormatString)
            supportsKeyFormatString.KeyFormatString = KeyFormatString;

        return createdStringBearer;
    }
};

