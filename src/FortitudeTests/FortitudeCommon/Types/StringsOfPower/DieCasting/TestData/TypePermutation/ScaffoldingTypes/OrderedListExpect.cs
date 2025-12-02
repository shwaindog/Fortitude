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

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public interface IOrderedListExpect : IFormatExpectation
{
    bool ElementTypeIsNullable { get; }
    bool ElementTypeIsStruct { get; }
    
    bool ElementTypeIsNullableStruct { get; }
    bool ElementTypeIsNotNullableStruct { get; }
    Type ElementTypeScaffoldType { get; }
    Type ElementType { get; }

    bool ElementTypeIsClass { get; }
    bool ContainsNullElements { get; }

    bool HasRestrictingFilter { get; }
}

// ReSharper disable twice ExplicitCallerInfoArgument
public class OrderedListExpect<TInputElement>
(
    List<TInputElement>? inputList
  , string? valueFormatString = null
  , Expression<Func<OrderedCollectionPredicate<TInputElement>>>? elementFilterExpression = null
  , FieldContentHandling contentHandling = DefaultCallerTypeFlags
  , string? name = null
  , [CallerFilePath] string srcFile = ""
  , [CallerLineNumber] int srcLine = 0)
    : OrderedListExpect<TInputElement, TInputElement>
        (inputList, valueFormatString, elementFilterExpression, contentHandling
       , name, srcFile, srcLine);

public class OrderedListExpect<TInputElement, TFilterBase> : ExpectBase<List<TInputElement>?>, IOrderedListExpect
{
    private readonly string? filterName;

    private Type? elementType;

    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public OrderedListExpect(List<TInputElement>? inputList, string? valueFormatString = null
      , Expression<Func<OrderedCollectionPredicate<TFilterBase>>>? elementFilterExpression = null
      , FieldContentHandling contentHandling = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0)
        : base(inputList, valueFormatString, contentHandling,
               name
            ?? ((elementFilterExpression?.Body as MemberExpression)?.Member.Name)
            ?? (inputList != null
                   ? $"List<{typeof(TInputElement).CachedCSharpNameNoConstraints()}> {{ Count: {inputList.Count}}}"
                   : null), srcFile, srcLine)
    {
        if (elementFilterExpression != null)
        {
            var elementFilter = elementFilterExpression.Compile();
            ElementPredicate = elementFilter.Invoke();
            var expression = (MemberExpression)elementFilterExpression.Body;
            filterName = expression.Member.Name;
        }
        else { ElementPredicate = ISupportsOrderedCollectionPredicate<TFilterBase?>.GetNoFilterPredicate; }
    }

    public override bool InputIsEmpty => (Input?.Count ?? 0) >= 0;

    public bool ElementTypeIsNullable => ElementType.IsNullable() || ContainsNullElements;
    public bool ElementTypeIsNullableStruct => ElementType.IsNullable();
    public bool ElementTypeIsNotNullableStruct => !ElementTypeIsNullableStruct;
    public bool ElementTypeIsClass => !ElementTypeIsStruct;
    public bool ElementTypeIsStruct => ElementType.IsValueType;

    public bool ContainsNullElements
    {
        get { return Input?.Any(i => i == null) ?? false; }
    }

    public Type ElementType => elementType ??= typeof(TInputElement);
    public Type ElementTypeScaffoldType =>
        ElementType.IsNullable()
            ? ElementType.IfNullableGetUnderlyingTypeOrThis()
            : ElementType;

    public override Type CoreType => typeof(TInputElement).IfNullableGetUnderlyingTypeOrThis();

    public bool HasRestrictingFilter =>
        ElementPredicate == null
     || !Equals(ElementPredicate, ISupportsOrderedCollectionPredicate<TFilterBase>.GetNoFilterPredicate);

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

    public OrderedCollectionPredicate<TFilterBase>? ElementPredicate { get; init; }

    public override ISinglePropertyTestStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry)
    {
        var flags = scaffoldEntry.ScaffoldingFlags;

        return flags.HasFilterPredicate() && !flags.IsNullableSpanFormattableOnly()
            ? (flags.IsAcceptsAnyGeneric()
                ? scaffoldEntry.CreateStringBearerFunc(ElementType, typeof(TFilterBase))()
                : scaffoldEntry.CreateStringBearerFunc(ElementTypeScaffoldType, typeof(TFilterBase))())
            : (flags.IsAcceptsAnyGeneric()
                ? scaffoldEntry.CreateStringBearerFunc(ElementType)()
                : scaffoldEntry.CreateStringBearerFunc(ElementTypeScaffoldType)());
    }

    public override IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry, StyleOptions stringStyle)
    {
        var createdStringBearer = CreateNewStringBearer(scaffoldEntry);

        var acceptsNullables = scaffoldEntry.ScaffoldingFlags.HasAcceptsNullables();

        if (acceptsNullables && createdStringBearer is IMoldSupportedValue<TInputElement?[]?> nullArrayMold)
            nullArrayMold.Value = Input?.ToArray();
        else if (createdStringBearer is IMoldSupportedValue<TInputElement[]?> arrayMold)
            arrayMold.Value = Input?.OfType<TInputElement>().ToArray();
        else if (createdStringBearer is IMoldSupportedValue<IReadOnlyList<TInputElement>?> listMold)
            listMold.Value = Input!;
        else if (createdStringBearer is IMoldSupportedValue<IEnumerable<TInputElement>?> enumerableMold)
            enumerableMold.Value = Input!;
        else if (createdStringBearer is IMoldSupportedValue<IEnumerator<TInputElement>?> enumeratorMold)
            enumeratorMold.Value = Input?.GetEnumerator();
        else if (acceptsNullables && createdStringBearer is IMoldSupportedValue<object?[]?> nullObjArrayMold)
        {
            nullObjArrayMold.Value = Input?.Select(i => i as object).ToArray();
        }
        else if (createdStringBearer is IMoldSupportedValue<object[]?> objArrayMold) { objArrayMold.Value = Input?.OfType<object>().ToArray(); }
        else if (createdStringBearer is IMoldSupportedValue<IReadOnlyList<object?>?> objListMold)
            objListMold.Value = Input?.Select(i => i as object).ToList();
        else if (createdStringBearer is IMoldSupportedValue<IEnumerable<object?>?> objEnumerableMold)
            objEnumerableMold.Value = Input?.Select(i => i as object).ToList();
        else if (createdStringBearer is IMoldSupportedValue<IEnumerator<object?>?> objEnumeratorMold)
            objEnumeratorMold.Value = Input?.Select(i => i as object).ToList().GetEnumerator();

        if (HasRestrictingFilter && createdStringBearer is ISupportsOrderedCollectionPredicate<object> supportsSettingObjPredicateFilter)
            supportsSettingObjPredicateFilter.ElementPredicate = ElementPredicate!.ToObjectCastingFilter();
        if (HasRestrictingFilter && createdStringBearer is ISupportsOrderedCollectionPredicate<TFilterBase> supportsSettingPredicateFilter)
            supportsSettingPredicateFilter.ElementPredicate = ElementPredicate!;
        if (ValueFormatString != null && createdStringBearer is ISupportsValueFormatString supportsValueFormatString)
            supportsValueFormatString.ValueFormatString = ValueFormatString;

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
