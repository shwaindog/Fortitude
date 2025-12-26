// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public interface IOrderedListExpect : IFormatExpectation
{
    bool ElementTypeIsNullable { get; }
    bool ElementTypeIsStruct { get; }

    bool ElementTypeIsNullableStruct { get; }
    bool ElementTypeIsNotNullableStruct { get; }
    Type ElementTypeScaffoldType { get; }
    Type ElementType { get; }
    Type ElementCallType { get; }

    Type CollectionCallType { get; }

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
  , FormatFlags formatFlags = DefaultCallerTypeFlags
  , string? name = null
  , [CallerFilePath] string srcFile = ""
  , [CallerLineNumber] int srcLine = 0)
    : OrderedListExpect<TInputElement, TInputElement>
        (inputList, valueFormatString, elementFilterExpression, formatFlags
       , name, srcFile, srcLine);

public class OrderedListExpect<TInputElement, TFilterBase> : ExpectBase<List<TInputElement>?>, IOrderedListExpect
{
    private readonly string? filterName;

    private Type? elementType;

    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public OrderedListExpect(List<TInputElement>? inputList, string? valueFormatString = null
      , Expression<Func<OrderedCollectionPredicate<TFilterBase>>>? elementFilterExpression = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0)
        : base(inputList, valueFormatString, formatFlags,
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
        ElementCallType    = ElementType;
        CollectionCallType = inputList?.GetType() ?? typeof(List<TInputElement>);
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

    public Type CollectionCallType { get; protected set; }
    
    public Type ElementCallType { get; protected set; }

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
        ElementCallType         = ElementType;
        var createdStringBearer = CreateNewStringBearer(scaffoldEntry);

        var acceptsNullables = scaffoldEntry.ScaffoldingFlags.HasAcceptsNullables();

        var scaffFlags = scaffoldEntry.ScaffoldingFlags;
        if (acceptsNullables && createdStringBearer is IMoldSupportedValue<TInputElement?[]?> nullArrayMold)
        {
            nullArrayMold.Value = Input?.ToArray();
            CollectionCallType =
                scaffFlags.HasCallsAsSpan()
                    ? typeof(Span<TInputElement?>)
                    : scaffFlags.HasCallsAsReadOnlySpan()
                        ? typeof(ReadOnlySpan<TInputElement?>)
                        : typeof(TInputElement?[]);
        }
        else if (createdStringBearer is IMoldSupportedValue<TInputElement[]?> arrayMold)
        {
            arrayMold.Value = Input?.OfType<TInputElement>().ToArray();
            CollectionCallType =
                scaffFlags.HasCallsAsSpan()
                    ? typeof(Span<TInputElement>)
                    : scaffFlags.HasCallsAsReadOnlySpan()
                        ? typeof(ReadOnlySpan<TInputElement>)
                        : typeof(TInputElement[]);
        }
        else if (createdStringBearer is IMoldSupportedValue<IReadOnlyList<TInputElement>?> listMold)
        {
            listMold.Value     = Input!;
            CollectionCallType = Input?.GetType() ?? typeof(List<TInputElement>);
        }
        else if (createdStringBearer is IMoldSupportedValue<IEnumerable<TInputElement>?> enumerableMold)
        {
            enumerableMold.Value = Input!;
            CollectionCallType   = Input?.GetType() ?? typeof(List<TInputElement>);
        }
        else if (createdStringBearer is IMoldSupportedValue<IEnumerator<TInputElement>?> enumeratorMold)
        {
            enumeratorMold.Value = Input?.GetEnumerator();
            CollectionCallType   = enumeratorMold.Value?.GetType() ?? new List<TInputElement>().GetEnumerator().GetType();
        }
        else if (acceptsNullables && createdStringBearer is IMoldSupportedValue<object?[]?> nullObjArrayMold)
        {
            nullObjArrayMold.Value = Input?.Select(i => i as object).ToArray();
            CollectionCallType =
                scaffFlags.HasCallsAsSpan()
                    ? typeof(Span<object?>)
                    : scaffFlags.HasCallsAsReadOnlySpan()
                        ? typeof(ReadOnlySpan<object?>)
                        : typeof(object?[]);
            ElementCallType = typeof(object);
        }
        else if (createdStringBearer is IMoldSupportedValue<object[]?> objArrayMold)
        {
            objArrayMold.Value = Input?.OfType<object>().ToArray();
            CollectionCallType =
                scaffFlags.HasCallsAsSpan()
                    ? typeof(Span<object>)
                    : scaffFlags.HasCallsAsReadOnlySpan()
                        ? typeof(ReadOnlySpan<object>)
                        : typeof(object[]);
            ElementCallType = typeof(object);
        }
        else if (createdStringBearer is IMoldSupportedValue<IReadOnlyList<object?>?> objListMold)
        {
            objListMold.Value  = Input?.Select(i => i as object).ToList();
            CollectionCallType = objListMold.Value?.GetType() ?? typeof(List<object?>);
            ElementCallType    = typeof(object);
        }
        else if (createdStringBearer is IMoldSupportedValue<IEnumerable<object?>?> objEnumerableMold)
        {
            objEnumerableMold.Value = Input?.Select(i => i as object).ToList();
            CollectionCallType      = objEnumerableMold.Value?.GetType() ?? typeof(List<object?>);
            ElementCallType         = typeof(object);
        }
        else if (createdStringBearer is IMoldSupportedValue<IEnumerator<object?>?> objEnumeratorMold)
        {
            objEnumeratorMold.Value = Input?.Select(i => i as object).ToList().GetEnumerator();
            CollectionCallType      = objEnumeratorMold.Value?.GetType() ?? typeof(List<object?>);
            ElementCallType         = typeof(object);
        }
        if (HasRestrictingFilter && createdStringBearer is ISupportsOrderedCollectionPredicate<object> supportsSettingObjPredicateFilter)
        {
            supportsSettingObjPredicateFilter.ElementPredicate = ElementPredicate!.ToObjectCastingFilter();
        }
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
