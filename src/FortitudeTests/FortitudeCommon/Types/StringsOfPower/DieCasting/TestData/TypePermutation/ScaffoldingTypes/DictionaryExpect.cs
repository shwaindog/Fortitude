// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public interface IKeyedCollectionExpect : IFormatExpectation
{
    Type KeyType { get; }
    
    Type ValueType { get; }
    
    bool KeyTypeIsNullable { get; }
    bool KeyTypeIsNullableStruct { get; }
    bool KeyTypeIsNotNullableStruct { get; }
    bool KeyTypeIsStruct { get; }
    bool KeyTypeIsClass { get; }
    bool ContainsNullKeys { get; }
    
    bool ContainsDuplicateKeys { get; }

    bool HasRestrictingFilter { get; }
    
    string? KeyFormatString { get; }
}


public class KeyedCollectionExpect<TKey, TValue, TKFilterBase, TVFilterBase> : ExpectBase<List<KeyValuePair<TKey, TValue>>?>, IKeyedCollectionExpect
    where TKey : TKFilterBase
    where TValue : TVFilterBase
{
    private readonly string? filterName;

    private Type? keyType;
    private Type? valueType;

    private Type? keyFilterBaseType;
    private Type? valueFilterBaseType;

    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public KeyedCollectionExpect(List<KeyValuePair<TKey, TValue>>? inputList, string? valueFormatString = null, string? keyFormatString = null
      , Expression<Func<KeyValuePredicate<TKFilterBase, TVFilterBase>>>? elementFilterExpression = null
      , FieldContentHandling contentHandling = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0)
        : base(inputList, valueFormatString, contentHandling,
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

    public override bool InputIsEmpty => (Input?.Count ?? 0) >= 0;

    public bool KeyTypeIsNullable => KeyType.IsNullable() || ContainsNullKeys;
    public bool KeyTypeIsNullableStruct => KeyType.IsNullable();
    public bool KeyTypeIsNotNullableStruct => !KeyTypeIsNullableStruct;
    public bool KeyTypeIsClass => !KeyTypeIsStruct;
    public bool KeyTypeIsStruct => KeyType.IsValueType;

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

    public Type KeyFilterBaseType => keyFilterBaseType ??= typeof(TKFilterBase);
    public Type ValueFilterBaseType => valueFilterBaseType ??= typeof(TVFilterBase);
    
    public string? KeyFormatString { get; private set; }

    public override Type CoreType => typeof(TKey).IfNullableGetUnderlyingTypeOrThis();

    public bool HasRestrictingFilter =>
        KeyValuePredicate == null
     || !Equals(KeyValuePredicate, ISupportsKeyedCollectionPredicate<TKey, TValue>.GetNoFilterPredicate);

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

    public override IStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry)
    {
        var flags = scaffoldEntry.ScaffoldingFlags;

        return flags.HasFilterPredicate()
            ? scaffoldEntry.CreateStringBearerFunc(KeyType, ValueType, KeyFilterBaseType, ValueFilterBaseType)()
            : scaffoldEntry.CreateStringBearerFunc(KeyType, ValueType)();
    }

    public override IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry, StyleOptions stringStyle)
    {
        var createdStringBearer = CreateNewStringBearer(scaffoldEntry);

        var acceptsNullables = scaffoldEntry.ScaffoldingFlags.HasAcceptsNullables();

        if (acceptsNullables && createdStringBearer is IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?> nullArrayMold)
            nullArrayMold.Value = Input?.ToDictionary();
        else if (createdStringBearer is IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?> arrayMold)
            arrayMold.Value = Input?.ToArray();
        else if (createdStringBearer is IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?> listMold)
            listMold.Value = Input!;
        else if (createdStringBearer is IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?> enumerableMold)
            enumerableMold.Value = Input!;
        else if (createdStringBearer is IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?> enumeratorMold)
            enumeratorMold.Value = Input?.GetEnumerator();

        if (HasRestrictingFilter && createdStringBearer is ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase> supportsSettingPredicateFilter)
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

