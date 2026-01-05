// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations;

public class BothRevealersKeyedSubListDictExpect<TKey, TValue> : BothRevealersDictExpect<TKey, TValue, TKey, TValue, TKey, TValue, TKey>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public BothRevealersKeyedSubListDictExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , Expression<Func<List<TKey>>>? elementFilterExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, elementFilterExpression, formatFlags, name, srcFile, srcLine) { }

    public BothRevealersKeyedSubListDictExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, (Expression<Func<List<TKey>>>?)null, formatFlags, name, srcFile
           , srcLine) { }
}

public class KeyRevealerNullStructValueRevealerKeyedSubListDictExpect<TKey, TValue> : BothRevealersDictExpect<TKey, TValue?, TKey, TValue?, TKey, TValue, TKey>
    where TValue : struct
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public KeyRevealerNullStructValueRevealerKeyedSubListDictExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , Expression<Func<List<TKey>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, elementFilterExpression, formatFlags, name, srcFile, srcLine) { }

    public KeyRevealerNullStructValueRevealerKeyedSubListDictExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, (Expression<Func<List<TKey>>>?)null, formatFlags, name, srcFile
           , srcLine) { }
}


public class KeyRevealerNullClassValueRevealerKeyedSubListDictExpect<TKey, TValue> : BothRevealersDictExpect<TKey, TValue?, TKey, TValue?, TKey, TValue, TKey>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public KeyRevealerNullClassValueRevealerKeyedSubListDictExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , Expression<Func<List<TKey>>>? elementFilterExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, elementFilterExpression, formatFlags, name, srcFile, srcLine) { }

    public KeyRevealerNullClassValueRevealerKeyedSubListDictExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, (Expression<Func<List<TKey>>>?)null, formatFlags, name, srcFile
           , srcLine) { }
}


public class BothRevealersDictExpect<TKey, TValue> : BothRevealersDictExpect<TKey, TValue, TKey, TValue, TKey, TValue, TKey>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public BothRevealersDictExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , Expression<Func<KeyValuePredicate<TKey, TValue>>>? elementFilterExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, elementFilterExpression, formatFlags, name, srcFile, srcLine) { }

    public BothRevealersDictExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, (Expression<Func<KeyValuePredicate<TKey, TValue>>>?)null, formatFlags
           , name, srcFile, srcLine) { }
}

public class NullStructKeyRevealersKeyedDictExpect<TKey, TValue> : BothRevealersDictExpect<TKey?, TValue, TKey?, TValue, TKey?, TValue, TKey>
    where TKey : struct
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public NullStructKeyRevealersKeyedDictExpect(List<KeyValuePair<TKey?, TValue>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , Expression<Func<KeyValuePredicate<TKey?, TValue>>>? elementFilterExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, elementFilterExpression, formatFlags, name, srcFile, srcLine) { }

    public NullStructKeyRevealersKeyedDictExpect(List<KeyValuePair<TKey?, TValue>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, (Expression<Func<KeyValuePredicate<TKey?, TValue>>>?)null, formatFlags, name, srcFile
           , srcLine) { }
}

public class KeyRevealerNullStructValueRevealerKeyedDictExpect<TKey, TValue> : BothRevealersDictExpect<TKey, TValue?, TKey, TValue?, TKey, TValue, TKey>
    where TValue : struct
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public KeyRevealerNullStructValueRevealerKeyedDictExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , Expression<Func<KeyValuePredicate<TKey, TValue?>>>? elementFilterExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, elementFilterExpression, formatFlags, name, srcFile, srcLine) { }

    public KeyRevealerNullStructValueRevealerKeyedDictExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, (Expression<Func<KeyValuePredicate<TKey, TValue?>>>?)null, formatFlags, name, srcFile
           , srcLine) { }
}

public class BothNullStructRevealersKeyedDictExpect<TKey, TValue> : BothRevealersDictExpect<TKey?, TValue?, TKey?, TValue?, TKey?, TValue, TKey>
    where TKey : struct
    where TValue : struct
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public BothNullStructRevealersKeyedDictExpect(List<KeyValuePair<TKey?, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , Expression<Func<KeyValuePredicate<TKey?, TValue?>>>? elementFilterExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, elementFilterExpression, formatFlags, name, srcFile, srcLine) { }

    public BothNullStructRevealersKeyedDictExpect(List<KeyValuePair<TKey?, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, (Expression<Func<KeyValuePredicate<TKey?, TValue?>>>?)null, formatFlags, name, srcFile
           , srcLine) { }
}

public class NullClassKeyRevealersKeyedDictExpect<TKey, TValue> : BothRevealersDictExpect<TKey?, TValue, TKey?, TValue, TKey?, TValue, TKey>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public NullClassKeyRevealersKeyedDictExpect(List<KeyValuePair<TKey?, TValue>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , Expression<Func<KeyValuePredicate<TKey?, TValue>>>? elementFilterExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, elementFilterExpression, formatFlags, name, srcFile, srcLine) { }

    public NullClassKeyRevealersKeyedDictExpect(List<KeyValuePair<TKey?, TValue>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, (Expression<Func<KeyValuePredicate<TKey?, TValue>>>?)null, formatFlags, name, srcFile
           , srcLine) { }
}

public class KeyRevealerNullClassValueRevealerKeyedDictExpect<TKey, TValue> : BothRevealersDictExpect<TKey, TValue?, TKey, TValue?, TKey, TValue, TKey>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public KeyRevealerNullClassValueRevealerKeyedDictExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , Expression<Func<KeyValuePredicate<TKey, TValue?>>>? elementFilterExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, elementFilterExpression, formatFlags, name, srcFile, srcLine) { }

    public KeyRevealerNullClassValueRevealerKeyedDictExpect(List<KeyValuePair<TKey, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, (Expression<Func<KeyValuePredicate<TKey, TValue?>>>?)null, formatFlags, name, srcFile
           , srcLine) { }
}

public class BothNullClassRevealersKeyedDictExpect<TKey, TValue> : BothRevealersDictExpect<TKey?, TValue?, TKey?, TValue?, TKey?, TValue, TKey>
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public BothNullClassRevealersKeyedDictExpect(List<KeyValuePair<TKey?, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , Expression<Func<KeyValuePredicate<TKey?, TValue?>>>? elementFilterExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, elementFilterExpression, formatFlags, name, srcFile, srcLine) { }

    public BothNullClassRevealersKeyedDictExpect(List<KeyValuePair<TKey?, TValue?>>? inputList
      , Expression<Func<PalantírReveal<TValue>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKey>>> keyRevealerExpression
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) :
        base(inputList, valueRevealerExpression, keyRevealerExpression, (Expression<Func<KeyValuePredicate<TKey?, TValue?>>>?)null, formatFlags, name, srcFile
           , srcLine) { }
}

public class BothRevealersDictExpect<TKey, TValue, TKFilterBase, TVFilterBase, TKSubListDerived, TVRevealBase, TKRevealBase> :
    ValueRevealerDictExpect<TKey, TValue, TKFilterBase, TVFilterBase, TKSubListDerived, TVRevealBase>
    where TKey : TKFilterBase
    where TValue : TVFilterBase
    where TKSubListDerived : TKey
{
    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public BothRevealersDictExpect(List<KeyValuePair<TKey, TValue>>? inputList
      , Expression<Func<PalantírReveal<TVRevealBase>>> valueRevealerExpression
      , Expression<Func<PalantírReveal<TKRevealBase>>> keyRevealerExpression
      , Expression<Func<KeyValuePredicate<TKFilterBase, TVFilterBase>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0)
        : base(inputList, valueRevealerExpression, null, elementFilterExpression, formatFlags, name, srcFile, srcLine)
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
      , Expression<Func<List<TKSubListDerived>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0)
        : base(inputList, valueRevealerExpression, null, elementFilterExpression, formatFlags, name, srcFile, srcLine)
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
            ? (flags.HasAcceptsNullableStruct()
                ? (flags.HasKeyNullableStruct()
                    ? scaffoldEntry.CreateStringBearerFunc(KeyUnderlyingType, ValueUnderlyingType, KeySubListFilterBaseType)()
                    : scaffoldEntry.CreateStringBearerFunc(KeyType, ValueUnderlyingType, KeySubListFilterBaseType, KeyRevealerBaseType)())
                : (flags.HasKeyNullableStruct()
                    ? scaffoldEntry.CreateStringBearerFunc(KeyUnderlyingType, ValueType, KeySubListFilterBaseType, ValueRevealerBaseType)()
                    : scaffoldEntry.CreateStringBearerFunc(KeyType, ValueType, KeySubListFilterBaseType, KeyRevealerBaseType
                                                         , ValueRevealerBaseType)())
            )
            : (flags.HasFilterPredicate()
                ? (flags.HasAcceptsNullableStruct()
                    ? (flags.HasKeyNullableStruct()
                        ? scaffoldEntry.CreateStringBearerFunc(KeyUnderlyingType, ValueUnderlyingType)()
                        : scaffoldEntry.CreateStringBearerFunc(KeyType, ValueUnderlyingType, KeyFilterBaseType, KeyRevealerBaseType)())
                    : (flags.HasKeyNullableStruct()
                        ? scaffoldEntry.CreateStringBearerFunc(KeyUnderlyingType, ValueType, ValueFilterBaseType, ValueRevealerBaseType)()
                        : scaffoldEntry.CreateStringBearerFunc(KeyType, ValueType, KeyFilterBaseType, ValueFilterBaseType, KeyRevealerBaseType
                                                             , ValueRevealerBaseType)()
                    ))
                : (flags.HasAcceptsNullableStruct()
                    ? (flags.HasKeyNullableStruct()
                        ? scaffoldEntry.CreateStringBearerFunc(KeyUnderlyingType, ValueUnderlyingType)()
                        : scaffoldEntry.CreateStringBearerFunc(KeyType, ValueUnderlyingType, KeyRevealerBaseType)())
                    : (flags.HasKeyNullableStruct()
                        ? scaffoldEntry.CreateStringBearerFunc(KeyUnderlyingType, ValueType, ValueRevealerBaseType)()
                        : scaffoldEntry.CreateStringBearerFunc(KeyType, ValueType, KeyRevealerBaseType , ValueRevealerBaseType)()
                    )
                )
            );
    }

    public override IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry, StyleOptions stringStyle)
    {
        var createdStringBearer = CreateNewStringBearer(scaffoldEntry);

        if (createdStringBearer is IMoldSupportedValue<List<KeyValuePair<TKey, TValue>>> nullArrayMold) nullArrayMold.Value = Input;

        if (HasRestrictingSubListFilter && createdStringBearer is ISupportsSubsetDisplayKeys<TKSubListDerived> supportsSettingSubListFilter)
            supportsSettingSubListFilter.DisplayKeys = KeySubListPredicate!;
        if (HasRestrictingPredicateFilter &&
            createdStringBearer is ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase> supportsSettingPredicateFilter)
            supportsSettingPredicateFilter.KeyValuePredicate = KeyValuePredicate!;
        if (createdStringBearer is ISupportsKeyRevealer<TKRevealBase> supportsKeyRevealer) supportsKeyRevealer.KeyRevealer         = KeyRevealer;
        if (createdStringBearer is ISupportsValueRevealer<TVRevealBase> supportsValueRevealer) supportsValueRevealer.ValueRevealer = ValueRevealer;
        if (KeyFormatString != null && createdStringBearer is ISupportsKeyFormatString supportsKeyFormatString)
            supportsKeyFormatString.KeyFormatString = KeyFormatString;

        return createdStringBearer;
    }
};
