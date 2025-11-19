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

public interface IComplexOrderedListExpect : IOrderedListExpect, IComplexFieldFormatExpectation
{
}

// ReSharper disable twice ExplicitCallerInfoArgument
public class NullCloakedOrderedListExpect<TChildScaffoldListElement>
(
    List<TChildScaffoldListElement?>? inputList
  , PalantírReveal<TChildScaffoldListElement> itemRevealer
  , Expression<Func<OrderedCollectionPredicate<TChildScaffoldListElement?>>>? elementFilter = null
  , string? formatString = null
  , FieldContentHandling contentHandling = DefaultCallerTypeFlags
  , string? name = null
  , [CallerFilePath] string srcFile = ""
  , [CallerLineNumber] int srcLine = 0)
    :
        CloakedOrderedListExpect<TChildScaffoldListElement?, TChildScaffoldListElement?, TChildScaffoldListElement>
        (inputList, itemRevealer, elementFilter, formatString, contentHandling, name, srcFile, srcLine)
    where TChildScaffoldListElement : ISinglePropertyTestStringBearer, IUnknownPalantirRevealerFactory
{
}
public class CloakedOrderedListExpect<TChildScaffoldListElement>
(
    List<TChildScaffoldListElement>? inputList
  , PalantírReveal<TChildScaffoldListElement> itemRevealer
  , Expression<Func<OrderedCollectionPredicate<TChildScaffoldListElement>>>? elementFilter = null
  , string? formatString = null
  , FieldContentHandling contentHandling = DefaultCallerTypeFlags
  , string? name = null
  , [CallerFilePath] string srcFile = ""
  , [CallerLineNumber] int srcLine = 0)
    :
        CloakedOrderedListExpect<TChildScaffoldListElement, TChildScaffoldListElement, TChildScaffoldListElement>
        (inputList, itemRevealer, elementFilter, formatString, contentHandling, name, srcFile, srcLine)
    where TChildScaffoldListElement : ISinglePropertyTestStringBearer?, IUnknownPalantirRevealerFactory?
{
}

public class CloakedOrderedListExpect<TChildScaffoldListElement, TFilterBase, TRevealBase> : 
    OrderedListExpect<TChildScaffoldListElement, TFilterBase>, IComplexOrderedListExpect
    where TChildScaffoldListElement : TFilterBase?, TRevealBase?, ISinglePropertyTestStringBearer?, IUnknownPalantirRevealerFactory?
{

    // ReSharper disable once ConvertToPrimaryConstructor
    // ReSharper disable twice ExplicitCallerInfoArgument
    public CloakedOrderedListExpect(List<TChildScaffoldListElement>? inputList
      , PalantírReveal<TRevealBase> itemRevealer  
      , Expression<Func<OrderedCollectionPredicate<TFilterBase>>>? elementFilter = null
      , string? formatString = null
      , FieldContentHandling contentHandling = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0)
        : base(inputList, formatString, elementFilter, contentHandling, name, srcFile, srcLine)
    {
        ItemRevealer = itemRevealer;
    }

    public override bool InputIsEmpty => (Input?.Count ?? 0) >= 0;
    
    public PalantírReveal<TRevealBase> ItemRevealer { get; set; }

    public BuildExpectedOutput WhenValueExpectedOutput { get; set; } = null!;

    public override IStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry)
    {
        var flags = scaffoldEntry.ScaffoldingFlags;

        return flags.HasFilterPredicate() && !flags.IsNullableSpanFormattableOnly()
            ? (flags.IsAcceptsAnyGeneric()
                ? scaffoldEntry.CreateStringBearerFunc(ElementType, typeof(TFilterBase), ElementType)()
                : scaffoldEntry.CreateStringBearerFunc(ElementTypeScaffoldType, typeof(TFilterBase), ElementTypeScaffoldType)())
            : (flags.IsAcceptsAnyGeneric()
                ? scaffoldEntry.CreateStringBearerFunc(ElementType, ElementType)()
                : scaffoldEntry.CreateStringBearerFunc(ElementTypeScaffoldType, ElementTypeScaffoldType)());
    }

    public override IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry, StyleOptions stringStyle)
    {
        var createdStringBearer = CreateNewStringBearer(scaffoldEntry);

        var acceptsNullables = scaffoldEntry.ScaffoldingFlags.HasAcceptsNullables();

        if (acceptsNullables && createdStringBearer is IMoldSupportedValue<TChildScaffoldListElement?[]?> nullArrayMold)
            nullArrayMold.Value = Input?.ToArray();
        else if (createdStringBearer is IMoldSupportedValue<TChildScaffoldListElement[]?> arrayMold)
            arrayMold.Value = Input?.OfType<TChildScaffoldListElement>().ToArray();
        else if (createdStringBearer is IMoldSupportedValue<IReadOnlyList<TChildScaffoldListElement>?> listMold)
            listMold.Value = Input!;
        else if (createdStringBearer is IMoldSupportedValue<IEnumerable<TChildScaffoldListElement>?> enumerableMold)
            enumerableMold.Value = Input!;
        else if (createdStringBearer is IMoldSupportedValue<IEnumerator<TChildScaffoldListElement>?> enumeratorMold)
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
        if (!Equals(ElementPredicate, ISupportsOrderedCollectionPredicate<TFilterBase>.GetNoFilterPredicate) 
         && createdStringBearer is ISupportsOrderedCollectionPredicate<TFilterBase> supportsSettingPredicateFilter)
            supportsSettingPredicateFilter.ElementPredicate = 
                ElementPredicate ?? ISupportsOrderedCollectionPredicate<TFilterBase>.GetNoFilterPredicate;
        if (createdStringBearer is ISupportsUnknownValueRevealer supportsValueRevealer)
        {
            supportsValueRevealer.ValueRevealerDelegate = ItemRevealer;
        }
        return createdStringBearer;
    }
};
