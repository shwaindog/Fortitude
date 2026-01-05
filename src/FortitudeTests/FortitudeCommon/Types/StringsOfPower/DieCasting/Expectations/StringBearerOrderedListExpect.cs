// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

// ReSharper disable twice ExplicitCallerInfoArgument
public class NullStructStringBearerOrderedListExpect<TChildScaffoldListElement>
    : OrderedListExpect<TChildScaffoldListElement?, TChildScaffoldListElement?>, IComplexOrderedListExpect
    where TChildScaffoldListElement : struct, ISinglePropertyTestStringBearer, IUnknownPalantirRevealerFactory
{
    private ScaffoldingPartEntry? calledScaffoldingPart;

    private Type? calledValueType;
    private bool  excludeCalledType;

    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public NullStructStringBearerOrderedListExpect(List<TChildScaffoldListElement?>? inputList
      , Expression<Func<OrderedCollectionPredicate<TChildScaffoldListElement?>>>? elementFilter = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0)
        : base(inputList, valueFormatString, elementFilter, formatFlags, name, srcFile, srcLine)
    {
        FieldValueExpectation =
            new FieldExpect<TChildScaffoldListElement?>
                (null, ValueFormatString, false, null, formatFlags, name, srcFile, srcLine);
    }

    public ITypedFormatExpectation<TChildScaffoldListElement?> FieldValueExpectation { get; }

    public override bool InputIsEmpty => (Input?.Count ?? 0) >= 0;

    public TChildScaffoldListElement RevealerScaffold { get; set; }

    public BuildExpectedOutput WhenValueExpectedOutput { get; set; } = null!;

    public override string GetExpectedOutputFor(ScaffoldingStringBuilderInvokeFlags condition, StyleOptions stringStyle, string? formatString = null)
    {
        FieldValueExpectation.ClearExpectations();
        foreach (var expectedResult in ExpectedResults) { FieldValueExpectation.Add(expectedResult); }
        var expectValue = FieldValueExpectation.GetExpectedOutputFor(condition, stringStyle, formatString);
        if (expectValue != IFormatExpectation.NoResultExpectedValue && expectValue != "null" && expectValue != "" && !excludeCalledType)
        {
            expectValue = WhenValueExpectedOutput
                (excludeCalledType ? "" : (calledValueType ?? typeof(TChildScaffoldListElement)).CachedCSharpNameNoConstraints()
               , RevealerScaffold.PropertyName, condition, FieldValueExpectation);
        }
        return expectValue;
    }

    public override ISinglePropertyTestStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry)
    {
        var flags = scaffoldEntry.ScaffoldingFlags;

        return flags.OnlyAcceptsNullableStruct()
            ? scaffoldEntry.CreateStringBearerFunc(ElementTypeScaffoldType)()
            : (flags.HasFilterPredicate()
                ? scaffoldEntry.CreateStringBearerFunc(ElementType, ElementType)()
                : scaffoldEntry.CreateStringBearerFunc(ElementType)());
    }

    public override IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry, StyleOptions stringStyle)
    {
        excludeCalledType     = false;
        var scaffFlags = scaffoldEntry.ScaffoldingFlags;
        calledScaffoldingPart = new ScaffoldingPartEntry(typeof(TChildScaffoldListElement), scaffFlags);
        RevealerScaffold      = calledScaffoldingPart.CreateTypedStringBearerFunc<TChildScaffoldListElement>()();
        var createdStringBearer = CreateNewStringBearer(scaffoldEntry);

        var acceptsNullables = scaffFlags.HasAcceptsNullables();

        if (acceptsNullables && createdStringBearer is IMoldSupportedValue<TChildScaffoldListElement?[]?> nullArrayMold)
            calledValueType = (nullArrayMold.Value = Input?.ToArray())?.GetType();
        else if (createdStringBearer is IMoldSupportedValue<TChildScaffoldListElement?[]?> arrayMold)
            calledValueType = (arrayMold.Value = Input?.OfType<TChildScaffoldListElement?>().ToArray())?.GetType();
        else if (createdStringBearer is IMoldSupportedValue<IReadOnlyList<TChildScaffoldListElement?>?> listMold)
            calledValueType = (listMold.Value = Input!)?.GetType();
        else if (createdStringBearer is IMoldSupportedValue<IEnumerable<TChildScaffoldListElement?>?> enumerableMold)
            calledValueType = (enumerableMold.Value = Input!)?.GetType();
        else if (createdStringBearer is IMoldSupportedValue<IEnumerator<TChildScaffoldListElement?>?> enumeratorMold)
            calledValueType = (enumeratorMold.Value = Input?.GetEnumerator())?.GetType();
        else if (acceptsNullables && createdStringBearer is IMoldSupportedValue<object?[]?> nullObjArrayMold)
        {
            calledValueType   = (nullObjArrayMold.Value = Input?.Select(i => i as object).ToArray())?.GetType();
            excludeCalledType = calledValueType?.FullName?.StartsWith("System") ?? true;
        }
        else if (createdStringBearer is IMoldSupportedValue<object[]?> objArrayMold)
        {
            calledValueType   = (objArrayMold.Value = Input?.OfType<object>().ToArray())?.GetType();
            excludeCalledType = calledValueType?.FullName?.StartsWith("System") ?? true;
        }
        else if (createdStringBearer is IMoldSupportedValue<IReadOnlyList<object?>?> objListMold)
        {
            calledValueType   = (objListMold.Value = Input?.Select(i => i as object).ToList())?.GetType();
            excludeCalledType = calledValueType?.FullName?.StartsWith("System") ?? true;
        }
        else if (createdStringBearer is IMoldSupportedValue<IEnumerable<object?>?> objEnumerableMold)
        {
            calledValueType   = (objEnumerableMold.Value = Input?.Select(i => i as object).ToList())?.GetType();
            excludeCalledType = calledValueType?.FullName?.StartsWith("System") ?? true;
        }
        else if (createdStringBearer is IMoldSupportedValue<IEnumerator<object?>?> objEnumeratorMold)
        {
            calledValueType   = (objEnumeratorMold.Value = Input?.Select(i => i as object).ToList().GetEnumerator())?.GetType();
            excludeCalledType = calledValueType?.FullName?.StartsWith("System") ?? true;
        }

        calledValueType ??= typeof(TChildScaffoldListElement?);
        if (scaffFlags.HasCallsAsReadOnlySpan())
        {
            calledValueType = scaffFlags.HasAcceptsNonNullableObject() 
                ? typeof(ReadOnlySpan<object>) 
                : (scaffFlags.HasAcceptsNullableObject()
                    ? typeof(ReadOnlySpan<object?>)
                    : typeof(ReadOnlySpan<TChildScaffoldListElement?>));
        }
        if (scaffFlags.HasCallsAsSpan())
        {
            calledValueType = scaffFlags.HasAcceptsNonNullableObject() 
                ? typeof(Span<object>) 
                : (scaffFlags.HasAcceptsNullableObject()
                    ? typeof(Span<object?>)
                    : typeof(Span<TChildScaffoldListElement?>));
        }

        if (HasRestrictingFilter && createdStringBearer is ISupportsOrderedCollectionPredicate<object> supportsSettingObjPredicateFilter)
        {
            supportsSettingObjPredicateFilter.ElementPredicate = ElementPredicate!.ToObjectCastingFilter();

            excludeCalledType = calledValueType.FullName?.StartsWith("System") ?? false;
        }
        if (!Equals(ElementPredicate, ISupportsOrderedCollectionPredicate<TChildScaffoldListElement?>.GetNoFilterPredicate)
         && createdStringBearer is ISupportsOrderedCollectionPredicate<TChildScaffoldListElement?> supportsSettingPredicateFilter)
            supportsSettingPredicateFilter.ElementPredicate =
                ElementPredicate ?? ISupportsOrderedCollectionPredicate<TChildScaffoldListElement?>.GetNoFilterPredicate;
        return createdStringBearer;
    }
};

// ReSharper disable twice ExplicitCallerInfoArgument
public class NullStringBearerOrderedListExpect<TChildScaffoldListElement>
(
    List<TChildScaffoldListElement?>? inputList
  , Expression<Func<OrderedCollectionPredicate<TChildScaffoldListElement?>>>? elementFilter = null
  , string? valueFormatString = null
  , FormatFlags formatFlags = DefaultCallerTypeFlags
  , string? name = null
  , [CallerFilePath] string srcFile = ""
  , [CallerLineNumber] int srcLine = 0)
    :
        StringBearerOrderedListExpect<TChildScaffoldListElement?, TChildScaffoldListElement?, TChildScaffoldListElement>
        (inputList, elementFilter, valueFormatString, formatFlags, name, srcFile, srcLine)
    where TChildScaffoldListElement : ISinglePropertyTestStringBearer, IUnknownPalantirRevealerFactory { }

public class StringBearerOrderedListExpect<TChildScaffoldListElement>
(
    List<TChildScaffoldListElement>? inputList
  , Expression<Func<OrderedCollectionPredicate<TChildScaffoldListElement>>>? elementFilter = null
  , string? valueFormatString = null
  , FormatFlags formatFlags = DefaultCallerTypeFlags
  , string? name = null
  , [CallerFilePath] string srcFile = ""
  , [CallerLineNumber] int srcLine = 0)
    :
        StringBearerOrderedListExpect<TChildScaffoldListElement, TChildScaffoldListElement, TChildScaffoldListElement>
        (inputList, elementFilter, valueFormatString, formatFlags, name, srcFile, srcLine)
    where TChildScaffoldListElement : ISinglePropertyTestStringBearer?, IUnknownPalantirRevealerFactory? { }

public class StringBearerOrderedListExpect<TChildScaffoldListElement, TFilterBase, TRevealBase> :
    OrderedListExpect<TChildScaffoldListElement, TFilterBase>, IComplexOrderedListExpect
    where TChildScaffoldListElement : TFilterBase?, TRevealBase?, ISinglePropertyTestStringBearer?, IUnknownPalantirRevealerFactory?
{
    private ScaffoldingPartEntry? calledScaffoldingPart;

    private Type? calledValueType;
    private bool  excludeCalledType;

    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public StringBearerOrderedListExpect(List<TChildScaffoldListElement>? inputList
      , Expression<Func<OrderedCollectionPredicate<TFilterBase>>>? elementFilter = null
      , string? valueFormatString = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0)
        : base(inputList, valueFormatString, elementFilter, formatFlags, name, srcFile, srcLine)
    {
        FieldValueExpectation =
            new FieldExpect<TChildScaffoldListElement?>
                (default, ValueFormatString, false, default, formatFlags, name, srcFile, srcLine);
    }

    public ITypedFormatExpectation<TChildScaffoldListElement?> FieldValueExpectation { get; }

    public override bool InputIsEmpty => (Input?.Count ?? 0) >= 0;

    public TChildScaffoldListElement RevealerScaffold { get; set; } = default!;

    public BuildExpectedOutput WhenValueExpectedOutput { get; set; } = null!;

    public override string GetExpectedOutputFor(ScaffoldingStringBuilderInvokeFlags condition, StyleOptions stringStyle, string? formatString = null)
    {
        FieldValueExpectation.ClearExpectations();
        foreach (var expectedResult in ExpectedResults) { FieldValueExpectation.Add(expectedResult); }
        var expectValue = FieldValueExpectation.GetExpectedOutputFor(condition, stringStyle, formatString);
        if (expectValue != IFormatExpectation.NoResultExpectedValue && expectValue != "null" && expectValue != "")
        {
            expectValue = WhenValueExpectedOutput
                (excludeCalledType ? "" :(calledValueType ?? typeof(TChildScaffoldListElement)).CachedCSharpNameNoConstraints()
               , RevealerScaffold?.PropertyName ?? "NoRevealerScaffold", condition, FieldValueExpectation);
        }
        return expectValue;
    }

    public override ISinglePropertyTestStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry)
    {
        var flags = scaffoldEntry.ScaffoldingFlags;

        return flags.HasFilterPredicate()
            ? scaffoldEntry.CreateStringBearerFunc(ElementType, ElementType)()
            : scaffoldEntry.CreateStringBearerFunc(ElementType)();
    }

    public override IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry, StyleOptions stringStyle)
    {
        excludeCalledType     = false;
        var scaffFlags = scaffoldEntry.ScaffoldingFlags;
        calledScaffoldingPart = new ScaffoldingPartEntry(typeof(TChildScaffoldListElement), scaffFlags);
        RevealerScaffold      = calledScaffoldingPart.CreateTypedStringBearerFunc<TChildScaffoldListElement>()();
        var createdStringBearer = CreateNewStringBearer(scaffoldEntry);

        var acceptsNullables = scaffFlags.HasAcceptsNullables();

        if (acceptsNullables && createdStringBearer is IMoldSupportedValue<TChildScaffoldListElement?[]?> nullArrayMold)
            calledValueType = (nullArrayMold.Value = Input?.ToArray())?.GetType();
        else if (createdStringBearer is IMoldSupportedValue<TChildScaffoldListElement[]?> arrayMold)
            calledValueType = (arrayMold.Value = Input?.OfType<TChildScaffoldListElement>().ToArray())?.GetType();
        else if (createdStringBearer is IMoldSupportedValue<IReadOnlyList<TChildScaffoldListElement>?> listMold)
            calledValueType = (listMold.Value = Input!)?.GetType();
        else if (createdStringBearer is IMoldSupportedValue<IEnumerable<TChildScaffoldListElement>?> enumerableMold)
            calledValueType = (enumerableMold.Value = Input!)?.GetType();
        else if (createdStringBearer is IMoldSupportedValue<IEnumerator<TChildScaffoldListElement>?> enumeratorMold)
            calledValueType = (enumeratorMold.Value = Input?.GetEnumerator())?.GetType();
        else if (acceptsNullables && createdStringBearer is IMoldSupportedValue<object?[]?> nullObjArrayMold)
        {
            calledValueType   = (nullObjArrayMold.Value = Input?.Select(i => i as object).ToArray())?.GetType();
            excludeCalledType = calledValueType?.FullName?.StartsWith("System") ?? true;
        }
        else if (createdStringBearer is IMoldSupportedValue<object[]?> objArrayMold)
        {
            calledValueType   = (objArrayMold.Value = Input?.OfType<object>().ToArray())?.GetType();
            excludeCalledType = calledValueType?.FullName?.StartsWith("System") ?? true;
        }
        else if (createdStringBearer is IMoldSupportedValue<IReadOnlyList<object?>?> objListMold)
        {
            calledValueType   = (objListMold.Value = Input?.Select(i => i as object).ToList())?.GetType();
            excludeCalledType = calledValueType?.FullName?.StartsWith("System") ?? true;
        }
        else if (createdStringBearer is IMoldSupportedValue<IEnumerable<object?>?> objEnumerableMold)
        {
            calledValueType   = (objEnumerableMold.Value = Input?.Select(i => i as object).ToList())?.GetType();
            excludeCalledType = calledValueType?.FullName?.StartsWith("System") ?? true;
        }
        else if (createdStringBearer is IMoldSupportedValue<IEnumerator<object?>?> objEnumeratorMold)
        {
            calledValueType   = (objEnumeratorMold.Value = Input?.Select(i => i as object).ToList().GetEnumerator())?.GetType();
            excludeCalledType = calledValueType?.FullName?.StartsWith("System") ?? true;
        }
        
        calledValueType ??= typeof(TChildScaffoldListElement);
        if (scaffFlags.HasCallsAsReadOnlySpan())
        {
            calledValueType = scaffFlags.HasAcceptsNonNullableObject() 
                ? typeof(ReadOnlySpan<object>) 
                : (scaffFlags.HasAcceptsNullableObject()
                    ? typeof(ReadOnlySpan<object?>)
                    : typeof(ReadOnlySpan<TChildScaffoldListElement?>));
        }
        if (scaffFlags.HasCallsAsSpan())
        {
            calledValueType = scaffFlags.HasAcceptsNonNullableObject() 
                ? typeof(Span<object>) 
                : (scaffFlags.HasAcceptsNullableObject()
                    ? typeof(Span<object?>)
                    : typeof(Span<TChildScaffoldListElement?>));
        }

        if (HasRestrictingFilter && createdStringBearer is ISupportsOrderedCollectionPredicate<object?> supportsSettingNullObjPredicateFilter)
        {
            supportsSettingNullObjPredicateFilter.ElementPredicate = ElementPredicate!.ToNullableObjectCastingFilter();
        
            excludeCalledType = calledValueType.FullName?.StartsWith("System") ?? false;
        }
        if (HasRestrictingFilter && createdStringBearer is ISupportsOrderedCollectionPredicate<object> supportsSettingObjPredicateFilter)
        {
            supportsSettingObjPredicateFilter.ElementPredicate = ElementPredicate!.ToObjectCastingFilter();

            excludeCalledType = calledValueType.FullName?.StartsWith("System") ?? false;
        }
        if (!Equals(ElementPredicate, ISupportsOrderedCollectionPredicate<TFilterBase>.GetNoFilterPredicate)
         && createdStringBearer is ISupportsOrderedCollectionPredicate<TFilterBase> supportsSettingPredicateFilter)
            supportsSettingPredicateFilter.ElementPredicate =
                ElementPredicate ?? ISupportsOrderedCollectionPredicate<TFilterBase>.GetNoFilterPredicate;
        return createdStringBearer;
    }
};
